// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
    production: false,
    auth: {
        yandex: {
            clientId: '7489e5aae33b4568bf21cb434060df4d',
            redirectUrl: 'http://localhost:4200',
            scope: 'cloud_api:disk.app_folder cloud_api:disk.read cloud_api:disk.write cloud_api:disk.info',
        },
    },
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
