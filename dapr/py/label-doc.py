from cloudevents.sdk.event import v1
from dapr.ext.grpc import App
from dapr.clients import DaprClient

import os
import json

app = App()


# @app.route('/dapr/subscribe', methods=['GET'])
# def subscribe():
#     subscriptions = [{'pubsubname': 'pubsub',
#                       'topic': 'doc-text-extracted',
#                       'route': 'pub'}]
#     return jsonify(subscriptions)

@app.subscribe(pubsub_name='pubsub', topic='doc-text-extracted')
def docTextExtracted(event: v1.Event) -> None:
    print('start doc-text-extracted', flush=True)
    data = json.loads(event.Data())
    print(
        f'Subscriber received: docKey={data["docKey"]}, words={data["docExtractedText"]["words"]}', flush=True)
    doc_labeled_event = {
        'docKey': data["docKey"],
        'docLabeled': {
            'label': 'Passport',
            'provider': 'custom-py'
        }
    }
    with DaprClient() as d:
        d.publish_event(
            pubsub_name='pubsub',
            topic_name='doc-text-extracted',
            data=json.dumps(doc_labeled_event),
            data_content_type='application/json',
        )

port = os.environ['PORT']
port = port if port else 5001
print(f'Start doc-text-extracted on port {port}', flush=True)
app.run(port)
