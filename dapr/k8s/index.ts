import * as k8s from '@pulumi/kubernetes';

const createRedisDeployment = () => {
    const appName = 'dapr-vow-docs-redis';
    const labels = {
        app: appName,
    };

    const deployment = new k8s.apps.v1.Deployment(
        appName,
        {
            spec: {
                selector: { matchLabels: labels },
                replicas: 1,
                template: {
                    metadata: { labels: labels },
                    spec: {
                        containers: [
                            {
                                name: appName,
                                image: 'bitnami/redis:latest',
                                env: [
                                    {
                                        name: 'ALLOW_EMPTY_PASSWORD',
                                        value: 'yes',
                                    },
                                ],
                            },
                        ],
                    },
                },
            },
        },
        { deleteBeforeReplace: true },
    );
    return deployment;
};

const createReadFileDeployment = (appName: string, appPort: number) => {
    const appContainerName = `baio/vow-docs-${appName}:latest`;
    const labels = {
        app: appName,
    };

    const deployment = new k8s.apps.v1.Deployment(appName, {
        spec: {
            selector: { matchLabels: labels },
            replicas: 1,
            template: {
                metadata: {
                    labels: labels,
                    annotations: {
                        'dapr.io/enabled': 'true',
                        'dapr.io/app-id': appName,
                        'dapr.io/app-port': appPort.toString(),
                    },
                },
                spec: {
                    containers: [
                        {
                            name: appName,
                            image: appContainerName,
                            env: [{ name: 'PORT', value: appPort.toString() }],
                            ports: [{ containerPort: appPort }]
                        },
                    ],
                },
            },
        },
    });
    return deployment;
};

export const createNodePort = (
    name: string,
    port: number,
    targetPort: number,
) => {
    const service = new k8s.core.v1.Service(name, {
        metadata: {
            name: name,
            labels: {
                name: name,
                app: name,
            },
        },
        spec: {
            type: 'NodePort',
            ports: [
                {
                    name: `${port}-${targetPort}`,
                    port,
                    targetPort,
                },
            ],
            selector: {
                app: name,
            },
        },
    });
    return service;
};

const createDaprDeployment = () => {
    // const redisDeployment = createRedisDeployment();

    
    const readFilePort = 3000;
    const readFileDeployment = createReadFileDeployment(
        'read-file',
        readFilePort,
    );
    /*
    const readFileNodePort = createNodePort(
        'read-file',
        readFilePort,
        readFilePort,
    );
    */

    return {
        readFileDeployment,
        //readFileNodePort
        //readFileDeployment,
        //readFileNodePort,
    };
};

createDaprDeployment();
