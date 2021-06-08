import {
    ChangeDetectionStrategy,
    Component,
    Input,
    OnInit,
} from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Doc } from '../../models';
import { selectDoc } from '../../ngrx/selectors';

export interface UploadImageModalView {
    doc: Doc;
}

@Component({
    selector: 'app-upload-image-progress-workspace',
    templateUrl: 'upload-image-progress-workspace.component.html',
    styleUrls: ['upload-image-progress-workspace.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppUploadImageProgressWorkspaceComponent implements OnInit {
    view$: Observable<UploadImageModalView>;

    @Input() documentId: string;

    constructor(private readonly store: Store) {}

    ngOnInit() {
        console.log('111', this.documentId);
        this.view$ = this.store.select(selectDoc(this.documentId)).pipe(
            map((doc) => ({
                doc,
            }))
        );
    }
}
