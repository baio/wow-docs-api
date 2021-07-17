import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
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
import { AppDocDisplayComponent } from '../components/doc-display/doc-display.component';
import { AppDocEditWorkspaceComponent } from '../components/doc-edit-workspace/doc-edit-workspace.component';
import { AppDocWorkspaceComponent } from '../components/doc-workspace/doc-workspace.component';
import { AppUploadImageProgressWorkspaceComponent } from '../components/upload-image-progress-workspace/upload-image-progress-workspace.component';
import { DocsRepositoryService } from '../repository/docs.repository';
import { DocsDataAccessService } from '../services/docs.data-access.service';
import { Share } from '@capacitor/share';
import { Filesystem, Directory, Encoding } from '@capacitor/filesystem';

import {
    deleteDoc,
    displayDoc,
    editDoc,
    rehydrateDocs,
    rehydrateDocsSuccess,
    shareDoc,
    updateDocFormatted,
    updateDocState,
    uploadImage,
    uploadImageError,
    uploadImageSuccess,
} from './actions';
import { base64Str2Blob, docToText } from '../utils';

@Injectable()
export class DocsEffects {
    constructor(
        private readonly actions$: Actions,
        private readonly docsDataAccess: DocsDataAccessService,
        private readonly modalController: ModalController,
        private readonly docRepository: DocsRepositoryService,
        private readonly router: Router
    ) {}

    loadDocs$ = createEffect(() =>
        this.actions$.pipe(
            ofType(rehydrateDocs),
            switchMap(() => this.docRepository.getDocs()),
            map((docs) => rehydrateDocsSuccess({ docs }))
        )
    );

    uploadImageRequest$ = createEffect(() =>
        this.actions$.pipe(
            ofType(uploadImage),
            switchMap(({ id, base64 }) =>
                this.docsDataAccess.uploadImage(id, base64).pipe(mapTo(id))
            ),
            map((id) => uploadImageSuccess({ id })),
            catchError((_) => of(uploadImageError()))
        )
    );

    storeToDb$ = createEffect(
        () =>
            this.actions$.pipe(
                ofType(uploadImage),
                switchMap(({ id, base64 }) =>
                    this.docRepository.addDoc(id, base64)
                )
            ),
        { dispatch: false }
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

    newDocShowModal$ = createEffect(
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
                })
            ),
        { dispatch: false }
    );

    displayDocShowModal$ = createEffect(
        () =>
            this.actions$.pipe(
                ofType(displayDoc),
                tap(async ({ id }) => {
                    const modal = await this.modalController.create({
                        component: AppDocWorkspaceComponent,
                        componentProps: {
                            documentId: id,
                        },
                    });
                    await modal.present();
                    const { data } = await modal.onWillDismiss();
                })
            ),
        { dispatch: false }
    );

    editDocShowModal$ = createEffect(
        () =>
            this.actions$.pipe(
                ofType(editDoc),
                tap(async ({ id }) => {
                    const modal = await this.modalController.create({
                        component: AppDocEditWorkspaceComponent,
                        componentProps: {
                            documentId: id,
                        },
                    });
                    await modal.present();
                    const { data } = await modal.onWillDismiss();
                })
            ),
        { dispatch: false }
    );

    updateDocState$ = createEffect(
        () =>
            this.actions$.pipe(
                ofType(updateDocState),
                tap(({ id, docState }) =>
                    this.docRepository.updateDocState(id, docState)
                )
            ),
        { dispatch: false }
    );

    deleteDoc$ = createEffect(
        () =>
            this.actions$.pipe(
                ofType(deleteDoc),
                tap(({ id }) => {
                    this.router.navigate(['/tabs', 'docs']);
                    this.docRepository.deleteDoc(id);
                })
            ),
        { dispatch: false }
    );

    updateDocFormatted$ = createEffect(
        () =>
            this.actions$.pipe(
                ofType(updateDocFormatted),
                tap(({ id, docFormatted }) =>
                    this.docRepository.updateDocFormatted(id, docFormatted)
                )
            ),
        { dispatch: false }
    );

    shareDoc$ = createEffect(
        () =>
            this.actions$.pipe(
                ofType(shareDoc),
                tap(async ({ doc, share }) => {
                    const text = docToText(doc);
                    const base64 = doc.imgBase64.split(',')[1];
                    let filePath: string;
                    let url: string;
                    if (share !== 'doc-only') {
                        filePath = `vow-doc-${new Date().getTime()}.jpg`;
                        const res = await Filesystem.writeFile({
                            path: filePath,
                            data: base64,
                            directory: Directory.Cache,
                        });
                        url = res.uri;
                    }
                    await Share.share({
                        title: 'Документ',
                        text: share !== 'image-only' ? text : null,
                        url,
                        dialogTitle: 'Отпраивть данные',
                    });
                    if (url) {
                        await Filesystem.deleteFile({
                            path: filePath,
                            directory: Directory.Cache,
                        });
                    }
                })
            ),
        { dispatch: false }
    );
}
