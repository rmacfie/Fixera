#!/usr/bin/env bash

set -euo pipefail

function package() {
  dotnet pack --include-symbols --include-source -c Release -o ./dist -p:SymbolPackageFormat=snupkg $1
}

function publish() {
  dotnet nuget push -s nuget.org ./dist/*.nupkg
}

package "Fixera"
package "Fixera.NSubstitute"
publish
