import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Camera, CameraResultType } from '@capacitor/camera';
import { Store } from '@ngrx/store';
import { chunk } from 'lodash';
import { BehaviorSubject, combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { v4 } from 'uuid';
import { Doc, DocFormatted } from '../../models';
import {
    displayDoc,
    editDoc,
    rehydrateDocs,
    uploadImage,
} from '../../ngrx/actions';
import { selectDocsAsSortedList } from '../../ngrx/selectors';
import { searchDocs } from './search-docs';

export interface DocCaption {
    title: string;
    subTitle: string;
}

export interface DocView extends Doc {
    caption?: DocCaption;
}

export interface DocsRow {
    first: DocView;
    second: DocView;
}

export interface DocumentsWorkspaceView {
    rows: DocsRow[];
}

const getCaption = (formatted: DocFormatted): DocCaption => {
    if (formatted.kind === 'passport-rf-main-page') {
        return {
            title:
                formatted.lastName ||
                formatted.firstName ||
                formatted.middleName
                    ? [
                          formatted.lastName,
                          formatted.firstName,
                          formatted.middleName,
                      ]
                          .join(' ')
                          .trim()
                    : null,
            subTitle: 'Паспорт РФ',
        };
    }
};

const getDocView = (doc: Doc): DocView => {
    const caption = doc.formatted ? getCaption(doc.formatted) : null;
    return {
        ...doc,
        caption,
    };
};

@Component({
    selector: 'app-documents-workspace',
    templateUrl: 'documents-workspace.component.html',
    styleUrls: ['documents-workspace.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppDocumentsWorkspaceComponent implements OnInit {
    readonly search$ = new BehaviorSubject<string>(null);
    readonly view$: Observable<DocumentsWorkspaceView>;

    constructor(private readonly store: Store) {
        const docs$ = store.select(selectDocsAsSortedList);
        const rows$ = combineLatest([docs$, this.search$]).pipe(
            map(([docs, search]) => searchDocs(docs, search)),
            map((list) =>
                chunk(list, 2).map(([first, second]) => ({
                    first: getDocView(first),
                    second: second && getDocView(second),
                }))
            )
        );
        this.view$ = rows$.pipe(map((rows) => ({ rows })));
    }

    ngOnInit() {
        setTimeout(() => this.store.dispatch(rehydrateDocs()), 1000);
    }

    onDocClick(doc: Doc) {
        this.store.dispatch(displayDoc({ id: doc.id }));
    }

    trackByRow(_, row: DocsRow) {
        return row.first.id;
    }

    async onFileSelected(fileInput: HTMLInputElement) {
        try {
            const image = await Camera.getPhoto({
                quality: 90,
                // allowEditing: true,
                resultType: CameraResultType.Base64,
                saveToGallery: false,
                preserveAspectRatio: true,
                width: 1500,
            });

            // image.webPath will contain a path that can be set as an image src.
            // You can access the original file using image.path, which can be
            // passed to the Filesystem API to read the raw data of the image,
            // if desired (or pass resultType: CameraResultType.Base64 to getPhoto)

            const base64 = `data:image/${image.format};base64,${image.base64String}`;
            const id = v4();
            this.store.dispatch(
                uploadImage({ id, base64, date: new Date().getTime() })
            );
        } catch (err) {
            console.warn(err);
        }
    }

    onSearchChange(evt: any) {
        const search = evt.detail.value;
        this.search$.next(search);
    }
}
