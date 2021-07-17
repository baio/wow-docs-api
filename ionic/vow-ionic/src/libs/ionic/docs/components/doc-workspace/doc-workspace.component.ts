import {
    AfterViewInit,
    ChangeDetectionStrategy,
    Component,
    Input,
    OnDestroy,
    OnInit,
    ViewChild,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IonSelect, IonTextarea, ModalController } from '@ionic/angular';
import { Store } from '@ngrx/store';
import { Observable, of, Subject } from 'rxjs';
import { map, switchMap, takeUntil, takeWhile, tap } from 'rxjs/operators';
import { Doc, DocView } from '../../models';
import { deleteDoc, editDoc } from '../../ngrx/actions';
import { selectDoc } from '../../ngrx/selectors';
import { docFormattedToView } from './doc-formatted-to-view';

export interface UploadImageModalView {
    doc: Doc;
    docView: DocView;
}

@Component({
    selector: 'app-doc-workspace',
    templateUrl: 'doc-workspace.component.html',
    styleUrls: ['doc-workspace.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppDocWorkspaceComponent implements OnInit {
    view$: Observable<UploadImageModalView>;
    segment = 'main';

    @Input() documentId: string;

    @ViewChild(IonSelect) docTypeSelect: IonSelect;
    @ViewChild(IonTextarea) commentTextArea: IonTextarea;

    constructor(
        private readonly store: Store,
        private readonly modalController: ModalController
    ) {}

    ngOnInit() {
        const id$ = of(this.documentId);
        this.view$ = id$.pipe(
            switchMap((id) => this.store.select(selectDoc(id))),
            map((doc) => ({
                doc,
                docView: doc.formatted
                    ? docFormattedToView(doc.formatted)
                    : null,
            }))
        );
    }

    onDelete(doc: Doc) {
        this.store.dispatch(deleteDoc({ id: doc.id }));

        this.modalController.dismiss({
            docType: this.docTypeSelect.value,
            comment: this.commentTextArea.value,
        });
    }

    onEdit(doc: Doc) {
        this.store.dispatch(editDoc({ id: doc.id }));
    }
}
