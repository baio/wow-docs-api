import { createReducer, on } from '@ngrx/store';
import { assocPath, fromPairs, omit, pipe } from 'lodash/fp';
import { DocsState, DocState } from '../models';
import {
    addDocTag,
    deleteDoc,
    rehydrateDocsSuccess,
    removeDocTag,
    setImageBase64,
    updateDocFormatted,
    updateDocState,
    uploadImage,
    uploadImageSuccess,
} from './actions';

export const initialState: DocsState = {
    docs: {},
};

export const docsReducer = createReducer(
    initialState,
    on(uploadImage, (state, { id, base64, date }) =>
        assocPath(
            ['docs', id],
            { id, imgBase64: base64, date, upload: { status: 'progress' } },
            state
        )
    ),
    on(setImageBase64, (state, { id, base64 }) =>
        assocPath(['docs', id, 'imgBase64'], base64, state)
    ),
    on(uploadImageSuccess, (state, { id }) =>
        assocPath(['docs', id, 'upload', 'status'], 'success', state)
    ),
    on(updateDocState, (state, { id, docState }) =>
        assocPath(['docs', id], { ...state.docs[id], ...docState }, state)
    ),
    on(rehydrateDocsSuccess, (state, { docs }) => {
        const hash = fromPairs(docs.map((m) => [m.id, m]));
        return assocPath(['docs'], hash, state);
    }),
    on(deleteDoc, (state, { id }) =>
        assocPath(['docs'], omit(id, state.docs), state)
    ),
    on(
        updateDocFormatted,
        (state, { id, docFormatted }) =>
            pipe(
                assocPath(['docs', id, 'formatted'], docFormatted),
                assocPath(
                    ['docs', id, 'labeled', 'label'],
                    docFormatted ? docFormatted.kind : null
                )
            )(state) as any
    ),
    on(addDocTag, (state, { id, tag }) => {
        const doc = state.docs[id];
        if (doc) {
            const tags = doc?.tags || [];
            if (!tags.includes(tag)) {
                return assocPath(['docs', id, 'tags'], [tag, ...tags], state);
            } else {
                return state;
            }
        } else {
            return state;
        }
    }),
    on(removeDocTag, (state, { id, tag }) => {
        const doc = state.docs[id];
        if (doc) {
            const tags = doc?.tags || [];
            if (tags.includes(tag)) {
                return assocPath(
                    ['docs', id, 'tags'],
                    tags.filter((f) => f !== tag),
                    state
                );
            } else {
                return state;
            }
        } else {
            return state;
        }
    })
);
