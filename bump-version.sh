#!/usr/bin/env bash

new_version=${1?:First parameter must be the new version}

csproj_files=(
  "AspSecurityHeaders/AspSecurityHeaders.csproj"
  "AspSecurityHeaders.OrchardModule/AspSecurityHeaders.OrchardModule.csproj"
  "AspSecurityHeaders.Generators/AspSecurityHeaders.Generators.csproj"
) 
for csproj_file in "${csproj_files[@]}"; do
  sed -i '' -E "s#<(Assembly|File|Package)Version>[0-9\.]+</(Assembly|File|Package)Version>#<\1Version>$new_version</\2Version>#g" "$csproj_file"
done

manifest_files=("AspSecurityHeaders.OrchardModule/Manifest.cs")
for manifest_file in "${manifest_files[@]}"; do
  sed -i '' -E "s/Version = \"[0-9\.]+\",/Version = \"$new_version\",/" "$manifest_file"
done

find . -name "packages.lock.json" -exec rm {} \;
dotnet restore --use-lock-file