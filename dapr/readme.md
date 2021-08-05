## docker-compose dev

```
# create global network first !
docker network create dapr-net
docker-compose up
```

[zipkin](http://localhost:5411/)
[seq](http://localhost:5340/)

run test requests in `test.http`

## k8s

`helm install redis bitnami/redis --set auth.password="abc" --set replica.replicaCount=1`

With pulumi

[DAPR docs](https://docs.dapr.io/operations/hosting/kubernetes/kubernetes-deploy/)
[Example](https://github.com/dapr/quickstarts/tree/v1.0.0/hello-kubernetes)
[zipkin](https://docs.dapr.io/operations/monitoring/tracing/supported-tracing-backends/zipkin/)
[new relic](https://docs.dapr.io/operations/monitoring/logging/newrelic/)
[bridge to k8s](https://www.youtube.com/watch?v=ZwFOEUYe1WA&t=120s)

```
helm repo add newrelic https://helm-charts.newrelic.com
helm install newrelic-logging newrelic/newrelic-logging --set licenseKey=eu01xxac810d9fa0d7dce806f0029fba74cfNRAL --set endpoint=https://log-api.eu.newrelic.com/log/v1```

```
kubectl apply -f ./dapr/configs/tracing.yaml
```

```
docker build --build-arg PORT=3000 -t baio/vow-docs-read-file -f ./fsharp/ReadFile/Dockerfile ./fsharp
docker push baio/vow-docs-read-file:latest
kubectl apply -f ./deploy/read-file.yaml
#
docker build --build-arg PORT=3000 -t baio/vow-docs-ya-ocr -f ./fsharp/YaOCR/Dockerfile ./fsharp
docker push baio/vow-docs-ya-ocr:latest
kubectl apply -f ./deploy/ya-ocr.yaml
#
docker build --build-arg PORT=3000 -t baio/vow-docs-parse-doc -f ./fsharp/ParseDoc/Dockerfile ./fsharp
docker push baio/vow-docs-parse-doc:latest
kubectl apply -f ./deploy/parse-doc.yaml
#
docker build --build-arg PORT=3000 -t baio/vow-docs-update-store -f ./fsharp/UpdateStore/Dockerfile ./fsharp
docker push baio/vow-docs-update-store:latest
kubectl apply -f ./deploy/update-store.yaml
```

## Secrets

```
kubectl apply -f ./dapr/components/secret-store.yaml
kubectl apply -k ./dapr/secrets/dev/ya
```
