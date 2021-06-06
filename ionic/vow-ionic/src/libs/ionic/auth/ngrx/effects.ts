import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
    login,
    loginRedirectSuccess,
    rehydrateAuthState,
    rehydrateAuthStateError,
    rehydrateAuthStateSuccess,
} from './actions';
import { filter, map, take, tap } from 'rxjs/operators';
import { NavigationEnd, Router } from '@angular/router';
import { YaAuthService } from '../services/ya-auth.service';

const AUTH_PROVIDER_KEY = 'auth_provider';
const AUTH_TOKEN_KEY = 'auth_token';

@Injectable()
export class AuthEffects {
    constructor(
        private readonly actions$: Actions,
        private readonly router: Router,
        private readonly yaAuthService: YaAuthService
    ) {}

    login$ = createEffect(
        () =>
            this.actions$.pipe(
                ofType(login),
                tap(({ provider }) => {
                    switch (provider) {
                        case 'yandex':
                            this.yaAuthService.login();
                            break;
                        default:
                            console.error('provider is not found', provider);
                    }
                })
            ),
        {
            dispatch: false,
        }
    );

    routerFirstChange$ = createEffect(() =>
        this.router.events.pipe(
            filter((evt) => evt instanceof NavigationEnd),
            take(1),
            map((res: NavigationEnd) => {
                const token = this.yaAuthService.tryExtractTokenFromUrl(
                    res.url
                );
                if (token) {
                    return loginRedirectSuccess({
                        authState: { provider: 'yandex', token },
                    });
                } else {
                    return rehydrateAuthState();
                }
            })
        )
    );

    loginRedirectSuccess$ = createEffect(
        () =>
            this.actions$.pipe(
                ofType(loginRedirectSuccess),
                tap(({ authState: { provider, token } }) => {
                    localStorage.setItem(AUTH_TOKEN_KEY, token);
                    localStorage.setItem(AUTH_PROVIDER_KEY, provider);
                })
            ),
        {
            dispatch: false,
        }
    );

    rehydrateAuthState$ = createEffect(() =>
        this.actions$.pipe(
            ofType(rehydrateAuthState),
            map(() => {
                const token = localStorage.getItem(AUTH_TOKEN_KEY);
                const provider = localStorage.getItem(AUTH_PROVIDER_KEY);
                if (token && provider) {
                    return rehydrateAuthStateSuccess({
                        authState: {
                            provider: provider as any,
                            token: token,
                        },
                    });
                } else {
                    return rehydrateAuthStateError();
                }
            })
        )
    );
}
