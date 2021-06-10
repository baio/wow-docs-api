import { createReducer, on } from '@ngrx/store';
import { assocPath } from 'lodash/fp';
import { DocsState } from '../models';
import { updateDocState, uploadImage, uploadImageSuccess } from './actions';

export const initialState: DocsState = {
    docs: {},
};

const _docsReducer = createReducer(
    initialState,
    on(uploadImage, (state, { id, base64 }) =>
        assocPath(
            ['docs', id],
            { id, imgBase64: base64, upload: { status: 'progress' } },
            state
        )
    ),
    on(uploadImageSuccess, (state, { id }) =>
        assocPath(['docs', id, 'upload', 'status'], 'success', state)
    ),
    on(updateDocState, (state, { id, docState }) =>
        assocPath(['docs', id], { ...state.docs[id], ...docState }, state)
    )
);

export function docsReducer(state, action) {
    return _docsReducer(state, action);
}
