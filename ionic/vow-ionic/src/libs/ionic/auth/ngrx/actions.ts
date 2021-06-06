import { createAction, props } from '@ngrx/store';
import { AuthProvider, AuthState } from '../models';

export const rehydrateAuthState = createAction('[Auth] Rehydrate Auth State');

export const rehydrateAuthStateSuccess = createAction(
    '[Auth] Rehydrate Auth State Success',
    props<{ authState: AuthState }>()
);

export const rehydrateAuthStateError = createAction(
    '[Auth] Rehydrate Auth State Error'
);

export const login = createAction(
    '[Auth] Login',
    props<{ provider: AuthProvider }>()
);

export const loginRedirectSuccess = createAction(
    '[Auth] Login Redirect Success',
    props<{ authState: AuthState }>()
);
