import { Injectable } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of, Subject, timer } from 'rxjs';
import {
    catchError,
    map,
    mapTo,
    switchMap,
    take,
    takeUntil,
    tap,
} from 'rxjs/operators';
import { AppUploadImageProgressWorkspaceComponent } from '../components/upload-image-progress-workspace/upload-image-progress-workspace.component';
import { DocsDataAccessService } from '../services/docs.data-access.service';
import { ImageService } from '../services/image.service';
import {
    setImageBase64,
    updateDocState,
    uploadImage,
    uploadImageError,
    uploadImageSuccess,
} from './actions';

@Injectable()
export class DocsEffects {
    constructor(
        private readonly actions$: Actions,
        private readonly docsDataAccess: DocsDataAccessService,
        private readonly modalController: ModalController,
        private readonly imageService: ImageService
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

    uploadImageSetBase64$ = createEffect(() =>
        this.actions$.pipe(
            ofType(uploadImage),
            switchMap(async ({ id, file }) => {
                const { base64 } = await this.imageService.resizeImageMax(
                    file,
                    250
                );
                return setImageBase64({ id, base64 });
            })
        )
    );

    pollDocState$ = createEffect(() =>
        this.actions$.pipe(
            ofType(uploadImageSuccess),
            switchMap(({ id }) => {
                // poll every 3 seconds 5 times or till state completed
                const stop$ = new Subject();
                return timer(100, 100).pipe(
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
                    await modal.present();
                    const { data } = await modal.onWillDismiss();
                    console.log('!!!', data);
                })
            ),
        { dispatch: false }
    );
}