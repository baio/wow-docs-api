import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';
import { IonicModule, IonicRouteStrategy } from '@ionic/angular';
import { EffectsModule } from '@ngrx/effects';
import { ActionReducer, StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { storeLogger } from 'ngrx-store-logger';
import { AppAuthModule } from 'src/libs/ionic/profile';
import { DbModule } from 'src/libs/ionic/db/db.module';
import { AppGateKeeperModule } from 'src/libs/ionic/gate-keeper';
import { environment } from '../environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TabsPageModule } from './tabs/tabs.module';

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
        DbModule,
        TabsPageModule,
        AppGateKeeperModule,
    ],
    providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy }],
    bootstrap: [AppComponent],
})
export class AppModule {
    constructor() {}
}
