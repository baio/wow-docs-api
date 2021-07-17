import { createAction, props } from '@ngrx/store';
import { Doc, DocFormatted, DocState } from '../models';

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

export const displayDoc = createAction(
    '[Docs] Display Doc',
    props<{ id: string }>()
);

export const editDoc = createAction('[Docs] Edit Doc', props<{ id: string }>());

export const deleteDoc = createAction(
    '[Docs] Delete Doc',
    props<{ id: string }>()
);

export const updateDocFormatted = createAction(
    '[Docs] Update Doc Formatted',
    props<{ id: string; docFormatted: DocFormatted }>()
);

export const shareDoc = createAction(
    '[Docs] Share Doc',
    props<{ doc: Doc; share: 'doc-only' | 'image-only' | 'doc-and-image' }>()
);

export const copyClipboard = createAction(
    '[Docs] Doc Copy Clipboard',
    props<{ doc: Doc }>()
);

export const showFullScreenImage = createAction(
    '[Docs] Show Full Screen Image',
    props<{ doc: Doc }>()
);
