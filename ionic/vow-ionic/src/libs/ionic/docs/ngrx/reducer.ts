import { createReducer, on } from '@ngrx/store';
import { assocPath } from 'lodash/fp';
import { DocsState } from '../models';
import {
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
    on(uploadImage, (state, { id }) =>
        assocPath(
            ['docs', id],
            { id, imgBase64: null, upload: { status: 'progress' } },
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
    )
);
