import { createAction, props } from '@ngrx/store';
import { DocState } from '../models';

export const uploadImage = createAction(
    '[Docs] Upload Image',
    props<{ id: string; file: File; base64: string }>()
);

export const uploadImageSuccess = createAction(
    '[Docs] Upload Image Success',
    props<{ id: string }>()
);

export const uploadImageError = createAction('[Docs] Upload Image Error');

export const updateDocState = createAction(
    '[Docs] Update Doc State',
    props<{ id: string; docState: DocState }>()
);
