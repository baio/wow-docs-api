import { createReducer, on } from '@ngrx/store';
import { UserAuthState } from '../models';
import { loginRedirectSuccess, rehydrateAuthStateSuccess } from './actions';
import { assoc } from 'lodash/fp';

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
    )
);

export function authReducer(state, action) {
    return _authReducer(state, action);
}
