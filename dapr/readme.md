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

```
docker build --build-arg PORT=3000 -t baio/vow-docs-read-file -f ./fsharp/ReadFile/Dockerfile ./fsharp
kubectl apply -f ./deploy/read-file.yaml
```

```
cd k8s
p up
k apply -f ../dapr/components/pubsub.yaml
k apply -f ../dapr/components/statestore.yaml
```

