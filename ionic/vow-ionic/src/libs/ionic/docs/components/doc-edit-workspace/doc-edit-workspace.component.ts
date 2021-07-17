import {
    ChangeDetectionStrategy,
    Component,
    Input,
    OnInit,
} from '@angular/core';
import { ModalController } from '@ionic/angular';
import { Store } from '@ngrx/store';
import { BehaviorSubject, combineLatest, Observable, of } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';
import { Doc, DocFormatted, DocLabel, OptItem } from '../../models';
import {
    deleteDoc,
    showFullScreenImage,
    updateDocFormatted,
} from '../../ngrx/actions';
import { selectDoc } from '../../ngrx/selectors';

export interface UploadImageModalView {
    doc: Doc;
    activeDocLabel: DocLabel;
    activeDocFormatted: DocFormatted;
}

@Component({
    selector: 'app-doc-edit-workspace',
    templateUrl: 'doc-edit-workspace.component.html',
    styleUrls: ['doc-edit-workspace.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppDocEditWorkspaceComponent implements OnInit {
    view$: Observable<UploadImageModalView>;
    activeDocLabel$ = new BehaviorSubject<DocLabel>(null);

    @Input() title: string;
    @Input() documentId: string;

    readonly formTypes: OptItem[] = [
        {
            key: 'passport-rf-main-page',
            label: 'Гражданский Пасспорт РФ (главная)',
        },
    ];

    constructor(
        private readonly store: Store,
        private readonly modalController: ModalController
    ) {}

    ngOnInit() {
        const id$ = of(this.documentId); //this.activatedRoute.params.pipe(map(({ id }) => id));
        const doc$ = id$.pipe(
            switchMap((id) => this.store.select(selectDoc(id)))
        );
        this.view$ = combineLatest([doc$, this.activeDocLabel$]).pipe(
            tap(console.log),
            map(([doc, activeDocLabel]) => ({
                doc,
                activeDocLabel: !activeDocLabel
                    ? doc.labeled?.label
                    : activeDocLabel,
                activeDocFormatted: doc.formatted,
            })),
            tap(console.log)
        );
    }

    onDelete(doc: Doc) {
        this.store.dispatch(deleteDoc({ id: doc.id }));
        this.modalController.dismiss();
    }

    trackByOptItem(_, optItem: OptItem) {
        return optItem.key;
    }

    onSave(doc: Doc, docLabel: DocLabel, docFormatted: DocFormatted) {
        this.store.dispatch(
            updateDocFormatted({
                id: doc.id,
                docFormatted: { ...docFormatted, kind: docLabel },
            })
        );

        this.modalController.dismiss();
    }

    onDocTypeChanged(docType: any) {
        this.activeDocLabel$.next(docType.detail.value);
    }

    onViewImage(doc: Doc) {
        this.store.dispatch(showFullScreenImage({ doc }));
    }
}
