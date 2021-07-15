import { createAction, props } from '@ngrx/store';
import { Doc, DocState } from '../models';

export const rehydrateDocs = createAction('[Docs] Rehydrate Docs');

export const rehydrateDocsSuccess = createAction(
    '[Docs] Rehydrate Docs Success',
    props<{ docs: Doc[] }>()
);

export const uploadImage = createAction(
    '[Docs] Upload Image',
    props<{ id: string; base64: string; date: string }>()
);

export const setImageBase64 = createAction(
    '[Docs] Set Image Base64',
    props<{ id: string; base64: string }>()
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
