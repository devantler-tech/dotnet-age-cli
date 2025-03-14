#!/bin/bash
set -e

get() {
  local url=$1
  local binary=$2
  local target_dir=$3
  local target_name=$4
  local archiveType=$5

  echo "Downloading $target_name from $url"
  if [ "$archiveType" = "tar" ]; then
    curl -LJ "$url" | tar xvz -C "$target_dir" "$binary"
    mv "$target_dir/$binary" "${target_dir}/$target_name"
  elif [ "$archiveType" = "zip" ]; then
    curl -LJ "$url" -o "$target_dir/$target_name.zip"
    unzip -o "$target_dir/$target_name.zip" -d "$target_dir"
    mv "$target_dir/$binary" "${target_dir}/$target_name"
    rm "$target_dir/$target_name.zip"
  elif [ "$archiveType" = false ]; then
    curl -LJ "$url" -o "$target_dir/$target_name"
  fi
  chmod +x "$target_dir/$target_name"
}

get "https://getbin.io/FiloSottile/age?os=darwin&arch=amd64" "age/age-keygen" "src/Devantler.AgeCLI/runtimes/osx-x64/native" "age-keygen-osx-x64" "tar"
get "https://getbin.io/FiloSottile/age?os=darwin&arch=arm64" "age/age-keygen" "src/Devantler.AgeCLI/runtimes/osx-arm64/native" "age-keygen-osx-arm64" "tar"
get "https://getbin.io/FiloSottile/age?os=linux&arch=amd64" "age/age-keygen" "src/Devantler.AgeCLI/runtimes/linux-x64/native" "age-keygen-linux-x64" "tar"
get "https://getbin.io/FiloSottile/age?os=linux&arch=arm64" "age/age-keygen" "src/Devantler.AgeCLI/runtimes/linux-arm64/native" "age-keygen-linux-arm64" "tar"
get "https://getbin.io/FiloSottile/age?os=windows&arch=amd64" "age/age-keygen.exe" "src/Devantler.AgeCLI/runtimes/win-x64/native" "age-keygen-win-x64.exe" "zip"
rm -rf src/Devantler.AgeCLI/runtimes/*/native/age

