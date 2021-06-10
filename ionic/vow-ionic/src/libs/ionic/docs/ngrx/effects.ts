import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { NEVER, of, Subject, timer } from 'rxjs';
import {
    catchError,
    map,
    mapTo,
    switchMap,
    tap,
    take,
    takeUntil,
} from 'rxjs/operators';
import { DocsDataAccessService } from '../services/docs.data-access.service';
import {
    updateDocState,
    uploadImage,
    uploadImageError,
    uploadImageSuccess,
} from './actions';
import { ModalController } from '@ionic/angular';
import { AppUploadImageProgressWorkspaceComponent } from '../components/upload-image-progress-workspace/upload-image-progress-wprkspace.component';

@Injectable()
export class DocsEffects {
    constructor(
        private readonly actions$: Actions,
        private readonly docsDataAccess: DocsDataAccessService,
        private readonly modalController: ModalController
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

    pollDocState$ = createEffect(() =>
        this.actions$.pipe(
            ofType(uploadImageSuccess),
            switchMap(({ id }) => {
                // poll every 3 seconds 5 times or till state completed
                const stop$ = new Subject();
                return timer(1000, 1000).pipe(
                    takeUntil(stop$),
                    take(5),
                    switchMap(() => this.docsDataAccess.getDocumentState(id)),
                    switchMap((result) => {
                        if (
                            result.formatted &&
                            result.labeled &&
                            result.parsed &&
                            result.stored
                        ) {
                            stop$.next();
                        }
                        return of({ id, docState: result });
                    })
                );
            }),
            map((payload) => updateDocState(payload))
        )
    );

    uploadImageShowProgressModal$ = createEffect(
        () =>
            this.actions$.pipe(
                ofType(uploadImage),
                tap(async ({ id }) => {
                    const modal = await this.modalController.create({
                        component: AppUploadImageProgressWorkspaceComponent,
                        componentProps: {
                            documentId: id,
                        },
                    });
                    return await modal.present();
                })
            ),
        { dispatch: false }
    );
}
