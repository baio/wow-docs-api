import { Inject, Injectable } from '@angular/core';

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

export interface YaAuthConfig {
    clientId: string;
    redirectUrl: string;
    scope: string;
}

export const YA_AUTH_CONFIG = 'YA_AUTH_CONFIG';

const YA_DEVICE_ID_KEY = 'YA_DEVICE_ID';

const YA_VOW_DOCS_AUTH_STATE = 'ya';

const newGuid = () =>
    'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (c) => {
        var r = (Math.random() * 16) | 0,
            v = c == 'x' ? r : (r & 0x3) | 0x8;
        return v.toString(16);
    });

@Injectable()
export class YaAuthService {
    constructor(
        @Inject(YA_AUTH_CONFIG) private readonly config: YaAuthConfig
    ) {}

    getDeviceId() {
        const deviceId = localStorage.getItem(YA_DEVICE_ID_KEY);
        if (!deviceId) {
            const guid = newGuid();
            localStorage.setItem(YA_DEVICE_ID_KEY, guid);
            return deviceId;
        } else {
            return deviceId;
        }
    }

    login() {
        const deviceId = this.getDeviceId();
        console.log('???', deviceId);
        const qs = {
            response_type: 'token',
            client_id: this.config.clientId,
            device_id: deviceId,
            device_name: deviceId,
            redirect_uri: this.config.redirectUrl,
            scope: this.config.scope,
            force_confirm: 'yes',
            display: 'popup',
            state: YA_VOW_DOCS_AUTH_STATE,
        };
        const url = `https://oauth.yandex.com/authorize${jsonToQueryString(
            qs
        )}`;

        window.location.href = url;
    }

    tryExtractTokenFromUrl(hash: string) {
        const state = hash && (/state=([^&]+)/.exec(hash) || [])[1];
        if (state === YA_VOW_DOCS_AUTH_STATE) {
            return hash && (/access_token=([^&]+)/.exec(hash) || [])[1];
        } else {
            return null;
        }
    }
}
