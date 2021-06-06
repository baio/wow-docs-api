import { createReducer, on } from '@ngrx/store';
import { assoc } from 'lodash/fp';
import { UserAuthState } from '../models';
import {
    loginRedirectSuccess,
    logout,
    rehydrateAuthStateSuccess,
} from './actions';

export const initialState: UserAuthState = {
    authState: null,
};

const _authReducer = createReducer(
    initialState,
    on(loginRedirectSuccess, (state, { authState }) =>
        assoc('authState', authState, state)
    ),
    on(rehydrateAuthStateSuccess, (state, { authState }) =>
        assoc('authState', authState, state)
    ),
    on(logout, (state) => assoc('authState', null as any, state))
);

export function authReducer(state, action) {
    return _authReducer(state, action);
}
