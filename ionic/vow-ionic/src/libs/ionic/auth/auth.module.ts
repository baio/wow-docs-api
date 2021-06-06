import { IonicModule } from '@ionic/angular';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppLoginComponent } from './components/login/login.component';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { authReducer } from './ngrx/reducer';
import { AuthEffects } from './ngrx/effects';
import {
    YaAuthConfig,
    YaAuthService,
    YA_AUTH_CONFIG,
} from './services/ya-auth.service';

export interface AuthConfig {
    yandex: YaAuthConfig;
}

@NgModule({
    imports: [
        IonicModule,
        CommonModule,
        StoreModule.forFeature('auth', authReducer),
        EffectsModule.forFeature([AuthEffects]),
    ],
    declarations: [AppLoginComponent],
    exports: [AppLoginComponent],
    providers: [YaAuthService],
})
export class AppAuthModule {
    static forRoot(config: AuthConfig): ModuleWithProviders<AppAuthModule> {
        return {
            ngModule: AppAuthModule,
            providers: [
                {
                    provide: YA_AUTH_CONFIG,
                    useValue: config.yandex,
                },
            ],
        };
    }
}
