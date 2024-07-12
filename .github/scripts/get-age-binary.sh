#!/bin/bash
set -e

get() {
  local url=$1
  local binary=$2
  local target_dir=$3
  local target_name=$4
  local isTar=$5

  if [ "$isTar" = true ]; then
    curl -LJ "$url" | tar xvz -C "$target_dir" "$binary"
    mv "$target_dir/$binary" "${target_dir}/$target_name"
  elif [ "$isTar" = false ]; then
    curl -LJ "$url" -o "$target_dir/$target_name"
  fi
  chmod +x "$target_dir/$target_name"
}

get "https://getbin.io/FiloSottile/age?os=darwin&arch=amd64" "age/age-keygen" "Devantler.AgeCLI/assets/binaries" "age-keygen-darwin-amd64" true
get "https://getbin.io/FiloSottile/age?os=darwin&arch=arm64" "age/age-keygen" "Devantler.AgeCLI/assets/binaries" "age-keygen-darwin-arm64" true
get "https://getbin.io/FiloSottile/age?os=linux&arch=amd64" "age/age-keygen" "Devantler.AgeCLI/assets/binaries" "age-keygen-linux-amd64" true
get "https://getbin.io/FiloSottile/age?os=linux&arch=arm64" "age/age-keygen" "Devantler.AgeCLI/assets/binaries" "age-keygen-linux-arm64" true
curl -s "https://api.github.com/repos/FiloSottile/age/releases" | grep "browser_download.*windows-amd64.zip" | cut -d '"' -f 4 | sort -V | tail -n 2 | head -1 | xargs curl -LJ | unzip -p - "age/age-keygen.exe" >"Devantler.AgeCLI/assets/binaries/age-keygen-windows-amd64.exe"
rm -rf Devantler.AgeCLI/assets/binaries/age
