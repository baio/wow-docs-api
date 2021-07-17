import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { chunk } from 'lodash';
import { Observable } from 'rxjs';
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
    readonly view$: Observable<DocumentsWorkspaceView>;

    constructor(private readonly store: Store) {
        const rows$ = store
            .select(selectDocsAsSortedList)
            .pipe(
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

    onFileSelected(base64: string) {
        const id = v4();
        this.store.dispatch(
            uploadImage({ id, base64, date: new Date().toUTCString() })
        );
    }

    onDocClick(doc: Doc) {
        this.store.dispatch(displayDoc({ id: doc.id }));
    }

    trackByRow(_, row: DocsRow) {
        return row.first.id;
    }
}
