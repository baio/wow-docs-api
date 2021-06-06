import { Injectable } from '@angular/core';

import { AuthConfig, OAuthService } from 'angular-oauth2-oidc';

export const authCodeFlowConfig: AuthConfig = {
    loginUrl: 'https://oauth.yandex.com/authorize',

    // Url of the Identity Provider
    issuer: 'https://oauth.yandex.com/authorize',

    // URL of the SPA to redirect the user to after login
    redirectUri: window.location.origin,

    // The SPA's id. The SPA is registerd with this id at the auth-server
    // clientId: 'server.code',
    clientId: '7489e5aae33b4568bf21cb434060df4d',

    // Just needed if your auth server demands a secret. In general, this
    // is a sign that the auth server is not configured with SPAs in mind
    // and it might not enforce further best practices vital for security
    // such applications.
    // dummyClientSecret: 'secret',

    responseType: 'token',

    // requestAccessToken: true,

    // set the scope for the permissions the client should request
    // The first four are defined by OIDC.
    // Important: Request offline_access to get a refresh token
    // The api scope is a usecase specific one
    scope: 'cloud_api:disk.app_folder cloud_api:disk.read cloud_api:disk.write cloud_api:disk.info',

    showDebugInformation: true,

    disablePKCE: true,

    customQueryParams: {
        device_id: '789789798009',
        device_name: 'ionic-browser-dev',
        force_confirm: 'yes',
    },
};

function jsonToQueryString(json) {
    return (
        '?' +
        Object.keys(json)
            .map(function (key) {
                return (
                    encodeURIComponent(key) +
                    '=' +
                    encodeURIComponent(json[key])
                );
            })
            .join('&')
    );
}
@Injectable()
export class AuthService {
    constructor() {}

    login() {
        /*
        try {
            const x = this.authService.initLoginFlow();
            console.log('success', x);
        } catch (err) {
            console.log('error', err);
        }
        */

        const qs = {
            response_type: 'token',
            client_id: '7489e5aae33b4568bf21cb434060df4d',
            device_id: '789789798009',
            device_name: 'ionic-browser-dev',
            redirect_uri: 'http://localhost:4200',
            scope: 'cloud_api:disk.app_folder cloud_api:disk.read cloud_api:disk.write cloud_api:disk.info',
            force_confirm: 'yes',
            display: 'popup',
        };
        const url = `https://oauth.yandex.com/authorize${jsonToQueryString(
            qs
        )}`;
        console.log('wtf ???', url);
        window.location.href = url;
    }
}
