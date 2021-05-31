## docker-compose dev

```
# create global network first !
docker network create dapr-net
docker-compose up
```

[zipkin](http://localhost:5411/)
[seq](http://localhost:5340/)

run test requests in `test.http`

## debug with docker 

```
$Env:PORT=3004
dapr run --app-id label-doc --app-protocol grpc --app-port $Env:Port -- py label-doc.py
```