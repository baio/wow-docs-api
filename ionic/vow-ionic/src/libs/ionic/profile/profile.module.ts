import { CommonModule } from '@angular/common';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { AppProfileWorkspaceComponent } from './components/profile-workspace/profile-workspace.component';
import { AppProfileComponent } from './components/profile/profile.component';
import { AppSocialProvidersComponent } from './components/social-providers/social-providers.component';
import { AuthEffects } from './ngrx/effects';
import { authReducer } from './ngrx/reducer';
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
    declarations: [
        AppSocialProvidersComponent,
        AppProfileComponent,
        AppProfileWorkspaceComponent,
    ],
    exports: [AppProfileWorkspaceComponent],
    providers: [YaAuthService],
})
export class AppProfileModule {
    static forRoot(config: AuthConfig): ModuleWithProviders<AppProfileModule> {
        return {
            ngModule: AppProfileModule,
            providers: [
                {
                    provide: YA_AUTH_CONFIG,
                    useValue: config.yandex,
                },
            ],
        };
    }
}
