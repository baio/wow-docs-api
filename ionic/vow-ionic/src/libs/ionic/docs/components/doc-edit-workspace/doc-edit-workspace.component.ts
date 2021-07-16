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
import { Doc } from '../../models';
import { deleteDoc } from '../../ngrx/actions';
import { selectDoc } from '../../ngrx/selectors';

export interface UploadImageModalView {
    doc: Doc;
}

@Component({
    selector: 'app-doc-edit-workspace',
    templateUrl: 'doc-edit-workspace.component.html',
    styleUrls: ['doc-edit-workspace.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppDocEditWorkspaceComponent
    implements OnInit, OnDestroy, AfterViewInit
{
    private readonly destroy$ = new Subject();
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
        const id$ = of(this.documentId); //this.activatedRoute.params.pipe(map(({ id }) => id));
        this.view$ = id$.pipe(
            switchMap((id) => this.store.select(selectDoc(id))),
            map((doc) => ({
                doc,
            }))
        );
    }

    ngAfterViewInit() {
        const sysDocType$ = this.view$.pipe(
            map((m) => m.doc && m.doc.labeled && m.doc.labeled.label)
        );

        sysDocType$
            .pipe(
                takeUntil(this.destroy$),
                takeWhile(() => !this.docTypeSelect.value)
            )
            .subscribe((docType) => {
                this.docTypeSelect.value = docType;
            });
    }

    ngOnDestroy() {
        this.destroy$.next();
    }

    nullableToState(obj?: any) {
        return obj ? 'success' : 'progress';
    }

    onDelete(doc: Doc) {
        this.store.dispatch(deleteDoc({ id: doc.id }));

        this.modalController.dismiss({
            docType: this.docTypeSelect.value,
            comment: this.commentTextArea.value,
        });
    }

    onSegmentChanged(ev: any) {
        this.segment = ev.detail.value;
    }
}
