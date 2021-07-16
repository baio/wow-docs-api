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
    selector: 'app-doc-edit-passport-form',
    templateUrl: 'doc-edit-passport-form.component.html',
    styleUrls: ['doc-edit-passport-form.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppDocEditPassportFormComponent {}
