#!/bin/bash
set -euo pipefail

# Get the name of the current script without extension to use as a build config name.
current_script=$(basename "$0" .sh)

# Build a release by its preset build configuration.
../../src/make/make.sh --buildconfig "$current_script"