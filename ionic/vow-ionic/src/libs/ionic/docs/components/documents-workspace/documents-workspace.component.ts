import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { selectAuthState } from '../../ngrx/selectors';
import { v4 } from 'uuid';
import { uploadImage } from '../../ngrx/actions';

@Component({
    selector: 'app-documents-workspace',
    templateUrl: 'documents-workspace.component.html',
    styleUrls: ['documents-workspace.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppDocumentsWorkspaceComponent {
    readonly view$: Observable<any>;

    constructor(private readonly store: Store) {
        this.view$ = store
            .select(selectAuthState)
            .pipe(map((authState) => ({ authState })));
    }

    onFileSelected({ file, base64 }: { file: File; base64: string }) {
        const id = v4();
        this.store.dispatch(uploadImage({ id, file, base64 }));
    }
}
