import { createAction, props } from '@ngrx/store';

export const uploadImage = createAction(
    '[Docs] Upload Image',
    props<{ id: string; file: File; base64: string }>()
);

export const uploadImageSuccess = createAction(
    '[Docs] Upload Image Success',
    props<{ id: string }>()
);

export const uploadImageError = createAction('[Docs] Upload Image Error');
