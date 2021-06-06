import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';
import { IonicModule, IonicRouteStrategy } from '@ionic/angular';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { AppAuthModule } from 'src/libs/ionic/auth';
import { environment } from '../environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

@NgModule({
    declarations: [AppComponent],
    entryComponents: [],
    imports: [
        BrowserModule,
        IonicModule.forRoot(),
        AppAuthModule.forRoot(environment.auth),
        StoreModule.forRoot({}),
        EffectsModule.forRoot(),
        AppRoutingModule,
        StoreDevtoolsModule.instrument({
            logOnly: environment.production, // Restrict extension to log-only mode
        }),
    ],
    providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy }],
    bootstrap: [AppComponent],
})
export class AppModule {
    constructor() {}
}
