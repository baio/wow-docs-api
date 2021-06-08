import { createFeatureSelector, createSelector } from '@ngrx/store';
import { DocsState } from '../models';

export const selectUserAuthState = createFeatureSelector<DocsState>('docs');

export const selectAuthState = createSelector(
    selectUserAuthState,
    (state) => state.docs
);
