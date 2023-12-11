@echo off

docker-compose stop
docker network create gatx_network

if "%1"=="-d" (
    docker-compose up -d
) else (
    docker-compose up
)

