#!/usr/bin/env bash
set -e

host="$1"
shift
cmd="$@"

if [ -z "$host" ]; then
  echo "Usage: $0 <host> <command...>"
  exit 2
fi

port=1433
echo "⏳ Aguardando o SQL Server em $host:$port..."

while ! bash -c "</dev/tcp/$host/$port" >/dev/null 2>&1; do
  >&2 echo "🕒 Aguardando o SQL Server em $host:$port..."
  sleep 2
done

>&2 echo "✅ SQL Server disponível — executando comando: $cmd"
exec $cmd
