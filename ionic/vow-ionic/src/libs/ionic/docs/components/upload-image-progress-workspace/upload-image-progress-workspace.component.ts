import {
    AfterViewInit,
    ChangeDetectionStrategy,
    Component,
    Input,
    OnDestroy,
    OnInit,
    ViewChild,
} from '@angular/core';
import { IonSelect, IonTextarea, ModalController } from '@ionic/angular';
import { Store } from '@ngrx/store';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, takeWhile } from 'rxjs/operators';
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
export class AppUploadImageProgressWorkspaceComponent
    implements OnInit, OnDestroy, AfterViewInit
{
    private readonly destroy$ = new Subject();
    view$: Observable<UploadImageModalView>;

    @Input() documentId: string;

    @ViewChild(IonSelect) docTypeSelect: IonSelect;
    @ViewChild(IonTextarea) commentTextArea: IonTextarea;

    constructor(
        private readonly store: Store,
        private readonly modalController: ModalController
    ) {}

    ngOnInit() {
        this.view$ = this.store.select(selectDoc(this.documentId)).pipe(
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

    onContinue() {
        this.modalController.dismiss({
            docType: this.docTypeSelect.value,
            comment: this.commentTextArea.value,
        });
    }
}
