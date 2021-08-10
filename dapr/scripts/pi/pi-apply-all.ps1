kubectl apply -f ./dapr/pi-components/pubsub.yaml
kubectl apply -f ./dapr/pi-components/secret-store.yaml
kubectl apply -f ./dapr/pi-components/statestore.yaml
kubectl apply -f ./dapr/pi-components/tracing.yaml
###

kubectl apply -k ./dapr/secrets/dev/ya

##

kubectl apply -f ./pi-deploy/read-file.yaml
kubectl apply -f ./pi-deploy/ya-ocr.yaml
kubectl apply -f ./pi-deploy/parse-doc.yaml
kubectl apply -f ./pi-deploy/update-store.yaml
kubectl apply -f ./pi-deploy/query-store.yaml

##
kubectl apply -f ./pi-deploy/ingress.yaml