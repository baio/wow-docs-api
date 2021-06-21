import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';
import { IonicModule, IonicRouteStrategy } from '@ionic/angular';
import { EffectsModule } from '@ngrx/effects';
import { ActionReducer, StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { AppAuthModule } from 'src/libs/ionic/auth';
import { environment } from '../environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { storeLogger } from 'ngrx-store-logger';

// eslint-disable-next-line prefer-arrow/prefer-arrow-functions
export function logger(reducer: ActionReducer<any>): any {
    // default, no options
    return storeLogger()(reducer);
}

export const metaReducers = environment.production ? [] : [logger];

@NgModule({
    declarations: [AppComponent],
    entryComponents: [],
    imports: [
        BrowserModule,
        StoreModule.forRoot({}, { metaReducers }),
        StoreDevtoolsModule.instrument(),
        IonicModule.forRoot(),
        AppAuthModule.forRoot(environment.auth),
        EffectsModule.forRoot(),
        AppRoutingModule,
    ],
    providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy }],
    bootstrap: [AppComponent],
})
export class AppModule {
    constructor() {}
}
