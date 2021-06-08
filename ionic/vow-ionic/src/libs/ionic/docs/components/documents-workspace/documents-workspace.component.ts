import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { v4 } from 'uuid';
import { uploadImage } from '../../ngrx/actions';

@Component({
    selector: 'app-documents-workspace',
    templateUrl: 'documents-workspace.component.html',
    styleUrls: ['documents-workspace.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppDocumentsWorkspaceComponent {
    constructor(private readonly store: Store) {}

    onFileSelected({ file, base64 }: { file: File; base64: string }) {
        const id = v4();
        this.store.dispatch(uploadImage({ id, file, base64 }));
    }
}
