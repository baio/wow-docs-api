import { createReducer, on } from '@ngrx/store';
import { fromPairs } from 'lodash';
import { assocPath } from 'lodash/fp';
import { DocsState } from '../models';
import {
    rehydrateDocsSuccess,
    setImageBase64,
    updateDocState,
    uploadImage,
    uploadImageSuccess,
} from './actions';

export const initialState: DocsState = {
    docs: {},
};

export const docsReducer = createReducer(
    initialState,
    on(uploadImage, (state, { id, base64 }) =>
        assocPath(
            ['docs', id],
            { id, imgBase64: base64, upload: { status: 'progress' } },
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
    })
);
