#Run solution with DAPR

[dapr python SDK](https://github.com/dapr/python-sdk)

```
$Env:PORT=3004
dapr run --app-id label-doc --app-port $Env:Port -- py label-doc.py
```

