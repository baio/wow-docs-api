import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { chunk } from 'lodash';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { v4 } from 'uuid';
import { Doc } from '../../models';
import { uploadImage } from '../../ngrx/actions';
import { selectDocs, selectDocsAsSortedList } from '../../ngrx/selectors';

export interface DocsRow {
    first: Doc;
    second: Doc;
}

export interface DocumentsWorkspaceView {
    rows: DocsRow[];
}

@Component({
    selector: 'app-documents-workspace',
    templateUrl: 'documents-workspace.component.html',
    styleUrls: ['documents-workspace.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppDocumentsWorkspaceComponent {
    readonly view$: Observable<DocumentsWorkspaceView>;

    constructor(private readonly store: Store) {
        const rows$ = store
            .select(selectDocsAsSortedList)
            .pipe(
                map((list) =>
                    chunk(list, 2).map(([first, second]) => ({ first, second }))
                )
            );
        this.view$ = rows$.pipe(map((rows) => ({ rows })));
    }

    onFileSelected(file: File) {
        const id = v4();
        this.store.dispatch(
            uploadImage({ id, file, date: new Date().toUTCString() })
        );
    }

    onDockClicked(doc: Doc) {
        console.log('+++ doc clicked', doc);
    }

    trackByRow(_, row: DocsRow) {
        return row.first.id;
    }
}
