import { createReducer, on } from '@ngrx/store';
import { assoc, assocPath } from 'lodash/fp';
import { DocsState } from '../models';
import { uploadImage } from './actions';

export const initialState: DocsState = {
    docs: {},
};

const _docsReducer = createReducer(
    initialState,
    on(uploadImage, (state, { id, base64 }) =>
        assocPath(['docs', id], { id, imgBase64: base64 }, state)
    )
);

export function docsReducer(state, action) {
    return _docsReducer(state, action);
}
