import { createFeatureSelector, createSelector } from '@ngrx/store';
import { orderBy, sortBy } from 'lodash/fp';
import { TagsState } from '../models';

export const selectTagsState = createFeatureSelector<TagsState>('tags');

export const selectTags = createSelector(
    selectTagsState,
    (state) => state.tags
);

export const selectTag = (id: string) =>
    createSelector(selectTags, (tags) => tags[id]);

export const selectTagsAsSortedList = createSelector(selectTags, (tags) =>
    orderBy((a) => new Date(a.date), 'desc', Object.values(tags))
);
