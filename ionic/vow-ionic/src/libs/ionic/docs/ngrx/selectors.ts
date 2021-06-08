import { createFeatureSelector, createSelector } from '@ngrx/store';
import { Doc, DocsState } from '../models';

export const selectDocsState = createFeatureSelector<DocsState>('docs');

export const selectDocs = createSelector(
    selectDocsState,
    (state) => state.docs
);

export const selectDoc = (id: string) =>
    createSelector(selectDocs, (docs) => docs[id]);
