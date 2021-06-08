import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mapTo, switchMap } from 'rxjs/operators';
import { DocsDataAccessService } from '../services/docs.data-access.service';
import { uploadImage, uploadImageError, uploadImageSuccess } from './actions';

@Injectable()
export class DocsEffects {
    constructor(
        private readonly actions$: Actions,
        private readonly docsDataAccess: DocsDataAccessService
    ) {}

    uploadImageRequest$ = createEffect(() =>
        this.actions$.pipe(
            ofType(uploadImage),
            switchMap(({ id, file }) =>
                this.docsDataAccess.uploadImage(id, file).pipe(mapTo(id))
            ),
            map((id) => uploadImageSuccess({ id })),
            catchError((_) => of(uploadImageError()))
        )
    );
}
