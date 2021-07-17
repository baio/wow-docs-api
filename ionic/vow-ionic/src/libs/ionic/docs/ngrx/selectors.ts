import { createFeatureSelector, createSelector } from '@ngrx/store';
import { orderBy, sortBy } from 'lodash/fp';
import { Doc, DocsState } from '../models';

export const selectDocsState = createFeatureSelector<DocsState>('docs');

export const selectDocs = createSelector(
    selectDocsState,
    (state) => state.docs
);

export const selectDoc = (id: string) =>
    createSelector(selectDocs, (docs) => docs[id]);

export const selectDocsAsSortedList = createSelector(selectDocs, (docs) =>
    orderBy((a) => new Date(a.date), 'desc', Object.values(docs))
);
