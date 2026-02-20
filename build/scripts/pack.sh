#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"

dotnet pack "$ROOT_DIR/Sensemation.Core.sln" -c Release