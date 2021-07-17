import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Doc } from '../../models';

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
