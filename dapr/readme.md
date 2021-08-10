## Configuration

Set store strategy to 'name' in order to share state between apps !!!
https://docs.dapr.io/developing-applications/building-blocks/state-management/howto-share-state/

## Gettings started with k8s

+ Install k8s
+ dapr init -k
+ ./scripts/pi/pi-apply-infra.yaml
+ ./scripts/pi/pi-apply-all.ps1
## Dapr dev slim mode

```
redis-server
dapr init --slim
.\scripts\slim-run-all.ps1
```

`statestore.yaml`

(disable)[https://docs.dapr.io/operations/configuration/configuration-overview/] zipkin in local config !!!

```
cd C:\dev\vow-docs\dapr
dapr run --app-id testdapr --dapr-http-port 3500 --components-path ./dapr/slim-components
```


## docker-compose dev

```
# create global network first !
docker network create dapr-net
docker-compose up
```

[zipkin](http://localhost:5411/)
[seq](http://localhost:5340/)

run test requests in `test.http`

`$Env:KUBECONFIG="C:/dev/vow/secrets/mail_ru/kubeconfig.yaml;$HOME\.kube\config"`

## k8s
`https://blog.dapr.io/posts/2020/10/30/dapr-on-raspberry-pi-with-k3s/`
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
#
docker build --build-arg PORT=3000 -t baio/vow-docs-query-store -f ./fsharp/QueryStore/Dockerfile ./fsharp
docker push baio/vow-docs-query-store:latest
kubectl apply -f ./deploy/query-store.yaml
```

## Secrets

```
kubectl apply -f ./dapr/components/secret-store.yaml
kubectl apply -k ./dapr/secrets/dev/ya
```

## Remote k8s cluster

https://kubernetes.io/docs/tasks/access-application-cluster/configure-access-multiple-clusters/

Debug k8s `k run -it --rm --restart=Never busybox --image=arm64v8/busybox sh`
Debug k8s `k run -it --rm test --image=baio/vow-docs-ya-ocr-arm sh`