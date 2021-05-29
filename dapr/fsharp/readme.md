#Run solution with DAPR

## Run with DAPR

Run DAPR side container to pubsub
`dapr run --app-id testdapr --dapr-http-port 3500`

Be aware Windows env example, use analog for unix

```
$Env:PORT=3000
dapr run --app-id read-file --app-port $Env:Port -- dotnet watch run --project read-file
$Env:PORT=3001
dapr run --app-id store-doc --app-port $Env:Port -- dotnet watch run --project StoreDoc
$Env:PORT=3002
dapr run --app-id doc-events-store --app-port $Env:Port -- dotnet watch run --project doc-events-store
```

## Local env

Run elasticsearch & kibana

```
docker network create elastic
docker run -d --name elastic --net elastic --restart unless-stopped -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" elasticsearch:7.12.1
docker run -d --name elastic --net elastic --restart unless-stopped --link elastic:elasticsearch -p 5601:5601 docker.elastic.co/kibana/kibana:7.12.1
```

+ [Localhost logging](http://localhost:5601/)
+ [Localhost tracing](http://localhost:9411/)

## DAPR docs

### Usefull links
+ [dotnet-sdk](https://github.com/dapr/dotnet-sdk)
+ [Observability](https://docs.microsoft.com/en-us/dotnet/architecture/dapr-for-net-developers/observability)
+ [Service invocation API](https://docs.dapr.io/reference/api/service_invocation_api/)
+ [Pub Sub API](https://docs.dapr.io/reference/api/pubsub_api/)
+ [Pub Sub](https://docs.dapr.io/developing-applications/building-blocks/pubsub/howto-publish-subscribe/)
