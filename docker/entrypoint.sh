#!/usr/bin/env bash
set -e

SECRETS_DIR="/app/secrets"
SECRETS_FILE="$SECRETS_DIR/vaultsecret-generated-secrets.txt"

mkdir -p "$SECRETS_DIR"

generate_base64() {
  openssl rand -base64 "$1"
}

if [ ! -f "$SECRETS_FILE" ]; then
  echo "Generating VaultSecret runtime secrets..."

  ENCRYPTION_MASTER_KEY="$(generate_base64 32)"
  API_KEYS_PEPPER="$(generate_base64 32)"
  JWT_SIGNING_KEY="$(generate_base64 64)"

  cat >"$SECRETS_FILE" <<EOF
Encryption__MasterKey=$ENCRYPTION_MASTER_KEY
ApiKeys__Pepper=$API_KEYS_PEPPER
Jwt__SigningKey=$JWT_SIGNING_KEY
EOF

  chmod 600 "$SECRETS_FILE"

  echo "Secrets generated and saved at: $SECRETS_FILE"
else
  echo "Using existing generated secrets from: $SECRETS_FILE"
fi

set -a
. "$SECRETS_FILE"
set +a

export Encryption__MasterKey
export ApiKeys__Pepper
export Jwt__SigningKey

exec dotnet VaultSecret.Api.dll
