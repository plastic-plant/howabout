#!/bin/bash
# Build and publish all available configurations to releases folder.
set -euo pipefail
../src/make/make.sh -bc all