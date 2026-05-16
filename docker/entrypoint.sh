#!/usr/bin/env bash
set -e

SECRETS_DIR="/app/secrets"
SECRETS_FILE="$SECRETS_DIR/vaultsecret-generated-secrets.txt"

DB_HOST="${DB_HOST:-postgres}"
DB_PORT="${DB_PORT:-5432}"
DB_USER="${DB_USER:-postgres}"
DB_PASSWORD="${DB_PASSWORD:-Estroncio90!}"
DB_NAME="${DB_NAME:-vaultsecret}"

mkdir -p "$SECRETS_DIR"

if [ -z "$ConnectionStrings__Postgres" ]; then
  export ConnectionStrings__Postgres="Host=$DB_HOST;Port=$DB_PORT;Database=$DB_NAME;Username=$DB_USER;Password=$DB_PASSWORD"
fi

generate_base64() {
  openssl rand -base64 "$1"
}

if [ ! -f "$SECRETS_FILE" ]; then
  cat >"$SECRETS_FILE" <<EOF
Encryption__MasterKey=$(generate_base64 32)
ApiKeys__Pepper=$(generate_base64 32)
Jwt__SigningKey=$(generate_base64 64)
EOF

  chmod 600 "$SECRETS_FILE"
  echo "VaultSecret runtime secrets generated."
fi

set -a
. "$SECRETS_FILE"
set +a

export PGPASSWORD="$DB_PASSWORD"

echo "Waiting for PostgreSQL..."

until pg_isready -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER"; do
  sleep 2
done

echo "PostgreSQL is ready."

echo "Creating database if it does not exist..."

DB_EXISTS=$(psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -d postgres -tAc "SELECT 1 FROM pg_database WHERE datname = '$DB_NAME'")

if [ "$DB_EXISTS" != "1" ]; then
  psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -d postgres -c "CREATE DATABASE \"$DB_NAME\";"
  echo "Database '$DB_NAME' created."
else
  echo "Database '$DB_NAME' already exists."
fi

echo "Applying EF Core migrations..."

dotnet Emu.Api.dll migrate

echo "Starting Emu System API..."

exec dotnet Emu.Api.dll
