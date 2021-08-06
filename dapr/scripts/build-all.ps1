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
