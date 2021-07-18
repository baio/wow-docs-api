import { createReducer, on } from '@ngrx/store';
import { assocPath, fromPairs, omit, pipe } from 'lodash/fp';
import { TagsState } from '../models';
import { createTag } from './actions';

export const initialState: TagsState = {
    tags: {},
};

export const tagsReducer = createReducer(
    initialState,
    on(createTag, (state, { name, date }) => {
        name = name.toLowerCase();
        return assocPath(['tags', name], { date, name }, state);
    })
);
