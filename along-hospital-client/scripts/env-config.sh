#!/bin/sh
# Script to generate env-config.js at runtime

ENV_FILE="/run/env-config.js"

echo "window.__ENV__ = {" > $ENV_FILE

printenv | grep "^VITE_" | while read -r line; do
    key=$(echo "$line" | cut -d '=' -f 1)
    value=$(echo "$line" | cut -d '=' -f 2-)
    
    escaped_value=$(echo "$value" | sed 's/\\/\\\\/g; s/"/\\"/g')
    echo "  $key: \"$escaped_value\"," >> $ENV_FILE
done

echo "};" >> $ENV_FILE

echo "Generated $ENV_FILE with runtime environment variables"
cat $ENV_FILE
