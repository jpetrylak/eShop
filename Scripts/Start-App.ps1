param(
    [switch]$d
)

docker compose stop

if (-not (docker network ls -q -f "name=^gatx_network$")) {
    docker network create gatx_network | Out-Null
}

if ($d) {
    docker compose up -d
} else {
    docker compose up
}
