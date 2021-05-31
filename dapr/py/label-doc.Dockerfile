FROM python:3.8-slim-buster
WORKDIR /app

RUN pip install dapr dapr-ext-grpc

COPY ["label-doc.py", "label-doc.py"]
ENTRYPOINT ["python", "label-doc.py"]

