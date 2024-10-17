#!/bin/bash
set -e

get() {
  local url=$1
  local binary=$2
  local target_dir=$3
  local target_name=$4
  local isTar=$5

  echo "Downloading $target_name from $url"
  if [ "$isTar" = true ]; then
    curl -LJ "$url" | tar xvz -C "$target_dir" "$binary"
    mv "$target_dir/$binary" "${target_dir}/$target_name"
  elif [ "$isTar" = false ]; then
    curl -LJ "$url" -o "$target_dir/$target_name"
  fi
  chmod +x "$target_dir/$target_name"
}

get "https://getbin.io/FiloSottile/age?os=darwin&arch=amd64" "age/age-keygen" "Devantler.AgeCLI/runtimes/osx-x64/native" "age-keygen-osx-x64" true
get "https://getbin.io/FiloSottile/age?os=darwin&arch=arm64" "age/age-keygen" "Devantler.AgeCLI/runtimes/osx-arm64/native" "age-keygen-osx-arm64" true
get "https://getbin.io/FiloSottile/age?os=linux&arch=amd64" "age/age-keygen" "Devantler.AgeCLI/runtimes/linux-x64/native" "age-keygen-linux-x64" true
get "https://getbin.io/FiloSottile/age?os=linux&arch=arm64" "age/age-keygen" "Devantler.AgeCLI/runtimes/linux-arm64/native" "age-keygen-linux-arm64" true
get "https://getbin.io/FiloSottile/age?os=windows&arch=amd64" "age/age-keygen.exe" "Devantler.AgeCLI/runtimes/win-x64/native" "age-keygen-win-x64.exe" true
rm -rf Devantler.AgeCLI/runtimes/*/native/age
