import { createFeatureSelector, createSelector } from '@ngrx/store';
import { UserAuthState } from '../models';

export const selectUserAuthState = createFeatureSelector<UserAuthState>('auth');

export const selectAuthState = createSelector(
    selectUserAuthState,
    (state) => state.authState
);
