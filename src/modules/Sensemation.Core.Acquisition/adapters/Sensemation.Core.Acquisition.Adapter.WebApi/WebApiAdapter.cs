// <copyright file="WebApiAdapter.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Sensemation.Core.Acquisition.Abstractions.Attributes;
using Sensemation.Core.Acquisition.Abstractions.Models;
using Sensemation.Core.Acquisition.Adapter.WebApi.Services;

namespace Sensemation.Core.Acquisition.Adapter.WebApi;

/// <summary>
/// Adapter that exposes item values over an HTTP Web API.
/// </summary>
[PluginType("webapi")]
public sealed class WebApiAdapter : BaseAdapter
{
    private string bindTo = "0.0.0.0";
    private int port = 7999;
    private bool enabled = true;
    private IHost? webHost;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebApiAdapter"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="id">The adapter identifier.</param>
    /// <param name="parameters">The adapter parameters.</param>
    public WebApiAdapter(ILogger<WebApiAdapter> logger, string id, Dictionary<string, string> parameters)
        : base(logger, id, parameters)
    {
    }

    /// <inheritdoc />
    protected override void InitializeCore()
    {
        if (this.Parameters.TryGetValue("bindTo", out var bindToValue))
        {
            this.bindTo = bindToValue;
        }

        if (this.Parameters.TryGetValue("port", out var portValue) && int.TryParse(portValue, out var parsedPort))
        {
            this.port = parsedPort;
        }

        if (this.Parameters.TryGetValue("enabled", out var enabledValue))
        {
            _ = bool.TryParse(enabledValue, out this.enabled);
        }
    }

    /// <inheritdoc />
    protected override async Task StartCoreAsync()
    {
        if (!this.enabled)
        {
            return;
        }

        this.webHost = Host.CreateDefaultBuilder(Array.Empty<string>())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                _ = webBuilder.UseKestrel()
                    .UseUrls($"http://{this.bindTo}:{this.port}")
                    .ConfigureServices(services =>
                    {
                        _ = services.AddSingleton<IItemService>(provider =>
                        {
                            return new ItemService(this.ValueAccessors ?? []);
                        });

                        _ = services.AddControllers();
                    })
                    .Configure(app =>
                    {
                        _ = app.UseRouting();
                        _ = app.UseEndpoints(endpoints =>
                        {
                            _ = endpoints.MapControllers();
                        });
                    });
            })
            .Build();

        await this.webHost.StartAsync().ConfigureAwait(false);
        await base.StartCoreAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    protected override async Task StopCoreAsync()
    {
        if (this.webHost is not null)
        {
            await this.webHost.StopAsync().ConfigureAwait(false);

            this.webHost.Dispose();
            this.webHost = null;
        }

        await base.StopCoreAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.webHost?.Dispose();
        }

        base.Dispose(disposing);
    }
}
