#!/usr/bin/env bash

new_version=${1?:First parameter must be the new version}

sed \
  -i '' \
  -E "s#<(Assembly|File|Package|Informational)Version>[0-9\.]+</(Assembly|File|Package|Informational)Version>#<\1Version>$new_version</\2Version>#g" \
  Directory.Build.targets

sed \
  -i '' \
  -E "s/Version = \"[0-9\.]+\",/Version = \"$new_version\",/" \
  AspSecurityHeaders.OrchardModule/Manifest.cs

find . -name "packages.lock.json" -exec rm {} \;
dotnet restore --use-lock-file