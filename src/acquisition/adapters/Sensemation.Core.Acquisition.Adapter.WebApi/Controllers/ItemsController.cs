// <copyright file="ItemsController.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

using Sensemation.Core.Acquisition.Adapter.WebApi.Models;
using Sensemation.Core.Acquisition.Adapter.WebApi.Services;
using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.Adapter.WebApi.Controllers;

/// <summary>
/// Provides item-related endpoints.
/// </summary>
[ApiController]
[Route("api/items")]
public class ItemsController : ControllerBase
{
    private readonly IItemService itemService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemsController"/> class.
    /// </summary>
    /// <param name="itemService">The item service.</param>
    public ItemsController(IItemService itemService)
    {
        this.itemService = itemService;
    }

    /// <summary>
    /// Gets all items with their latest values.
    /// </summary>
    /// <returns>The item list.</returns>
    [HttpGet]
    public IActionResult GetItems()
    {
        try
        {
            var itemIds = this.itemService.GetAllItemIds();
            var items = itemIds.Select(id =>
            {
                var hasLatest = this.itemService.TryGetLatestDataPoint(id, out var latest);
                var dataPoint = hasLatest && latest is not null
                    ? latest
                    : new DataPoint(DateTime.UtcNow, null, Quality.Bad);

                return new SignalValue
                {
                    Id = id,
                    Value = dataPoint.Value,
                    TimestampUtc = dataPoint.TimestampUtc,
                    Quality = dataPoint.Quality.ToString()
                };
            }).ToList();

            return this.Ok(items);
        }
        catch
        {
            return this.Ok(new List<object>());
        }
    }

    /// <summary>
    /// Gets the latest datapoint for an item.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <returns>The latest datapoint.</returns>
    [HttpGet("{itemId}/latest")]
    public IActionResult GetLatestDataPoint(string itemId)
    {
        try
        {
            if (!this.itemService.TryGetLatestDataPoint(itemId, out var dataPoint) || dataPoint is null)
            {
                return this.NotFound();
            }

            var response = new SignalValue
            {
                Id = itemId,
                Value = dataPoint.Value,
                TimestampUtc = dataPoint.TimestampUtc,
                Quality = dataPoint.Quality.ToString()
            };

            return this.Ok(response);
        }
        catch
        {
            return this.NotFound();
        }
    }

    /// <summary>
    /// Gets historical datapoints for an item.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="count">The maximum number of datapoints.</param>
    /// <returns>The datapoint history.</returns>
    [HttpGet("{itemId}/history")]
    public IActionResult GetHistory(string itemId, [FromQuery] int? count = null)
    {
        try
        {
            if (!this.itemService.TryGetHistoryValues(itemId, count, out var values))
            {
                return this.NotFound();
            }

            var response = values
                .Select(dataPoint => new SignalValue
                {
                    Id = itemId,
                    Value = dataPoint.Value,
                    TimestampUtc = dataPoint.TimestampUtc,
                    Quality = dataPoint.Quality.ToString()
                })
                .ToList();

            return this.Ok(response);
        }
        catch
        {
            return this.NotFound();
        }
    }

    /// <summary>
    /// Writes a value to an item.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="request">The write request.</param>
    /// <returns>The resulting datapoint.</returns>
    [HttpPost("{itemId}/write")]
    public async Task<IActionResult> WriteValue(string itemId, [FromBody] WriteItemRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var (success, dataPoint) = await this.itemService.TryWriteValueAsync(itemId, request.Value)
            .ConfigureAwait(false);

        return !success || dataPoint is null
            ? this.NotFound()
            : this.Ok(dataPoint);
    }
}
