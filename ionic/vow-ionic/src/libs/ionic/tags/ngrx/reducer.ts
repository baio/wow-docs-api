import { createReducer, on } from '@ngrx/store';
import { assocPath, fromPairs, omit } from 'lodash/fp';
import { TagsState } from '../models';
import { createTag, rehydrateTagsSuccess, removeTag } from './actions';

export const initialState: TagsState = {
    tags: {},
};

export const tagsReducer = createReducer(
    initialState,
    on(createTag, (state, { name, date }) =>
        assocPath(['tags', name], { date, name }, state)
    ),
    on(removeTag, (state, { name }) =>
        assocPath(['tags'], omit(name, state.tags), state)
    ),
    on(rehydrateTagsSuccess, (state, { tags }) => {
        const hash = fromPairs(tags.map((m) => [m.name, m]));
        return assocPath(['tags'], hash, state);
    })
);
