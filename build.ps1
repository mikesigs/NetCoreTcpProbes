docker-compose build
docker-compose push
kc apply -f .\samples\CustomHealthChecksExample\deployment.yaml
kc apply -f .\samples\SimpleExampleWithDefaults\deployment.yaml