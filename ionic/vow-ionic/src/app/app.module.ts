import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { NavigationEnd, Router, RouteReuseStrategy } from '@angular/router';
import { OAuthModule } from 'angular-oauth2-oidc';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { AuthConfig } from 'angular-oauth2-oidc';
import { HttpClientModule } from '@angular/common/http';
import { filter } from 'rxjs/operators';

export const authCodeFlowConfig: AuthConfig = {
    // Url of the Identity Provider
    issuer: 'https://oauth.yandex.com/authorize',

    // URL of the SPA to redirect the user to after login
    redirectUri: window.location.origin + '/index.html',

    // The SPA's id. The SPA is registerd with this id at the auth-server
    // clientId: 'server.code',
    clientId: '7489e5aae33b4568bf21cb434060df4d',

    // Just needed if your auth server demands a secret. In general, this
    // is a sign that the auth server is not configured with SPAs in mind
    // and it might not enforce further best practices vital for security
    // such applications.
    // dummyClientSecret: 'secret',

    responseType: 'code',

    requestAccessToken: true,

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
        display: 'popup',
    },
};

@NgModule({
    declarations: [AppComponent],
    entryComponents: [],
    imports: [
        BrowserModule,
        IonicModule.forRoot(),
        AppRoutingModule,
        HttpClientModule,
    ],
    providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy }],
    bootstrap: [AppComponent],
})
export class AppModule {
    constructor(router: Router) {
        router.events
            .pipe(filter((evt) => evt instanceof NavigationEnd))
            .subscribe((res: NavigationEnd) => {
                console.log('nav end !!!', res.url);
                const token =
                    res.url && (/#access_token=([^&]+)/.exec(res.url) || [])[1];
                console.log('nav end token !!!', token);
            });
    }
}
