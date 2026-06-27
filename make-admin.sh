#!/usr/bin/env bash
# Grant admin to a ROTF account by its email, so you can use /spawn etc.
# Usage:  ./make-admin.sh you@email.com      (then re-login in the client)
set -e
EMAIL="${1:?usage: ./make-admin.sh <account-email>}"
UP=$(printf '%s' "$EMAIL" | tr '[:lower:]' '[:upper:]')
JSON=$(redis-cli hget logins "$UP" 2>/dev/null)
[ -z "$JSON" ] && { echo "No account found for '$EMAIL'. Register it in the client first."; exit 1; }
ID=$(printf '%s' "$JSON" | python3 -c "import sys,json;print(json.load(sys.stdin)['AccountId'])")
redis-cli hset "account.$ID" admin 1 >/dev/null
echo "account.$ID ($EMAIL) is now ADMIN."
echo "Re-login in the client (disconnect/reconnect), then in-game type:"
echo '    /spawn The Illusionist'
