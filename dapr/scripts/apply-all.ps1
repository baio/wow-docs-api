kubectl apply -f ./dapr/components/pubsub.yaml
kubectl apply -f ./dapr/components/secret-store.yaml
kubectl apply -f ./dapr/components/statestore.yaml
kubectl apply -f ./dapr/components/tracing.yaml
kubectl apply -k ./dapr/secrets/dev/ya

##

kubectl apply -f ./deploy/read-file.yaml
kubectl apply -f ./deploy/ya-ocr.yaml
kubectl apply -f ./deploy/parse-doc.yaml
kubectl apply -f ./deploy/update-store.yaml
kubectl apply -f ./deploy/query-store.yaml
