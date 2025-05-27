#!/usr/bin/env bash

cat << 'EOF' > .git/hooks/pre-commit
#!/bin/zsh
set -eo pipefail

STAGED_FILES=$(git diff --name-only --cached --diff-filter=ACMR)
if [[ ! -z "$STAGED_FILES" ]]; then
  IFS=$'\n' files=($(echo $STAGED_FILES))
  dotnet csharpier check "${files[@]}"
  if [[ $? -ne 0 ]]; then
    echo "dotnet csharpier detected unformatted files!"
    exit 1
  fi
fi

EOF

chmod a+x .git/hooks/pre-commit
