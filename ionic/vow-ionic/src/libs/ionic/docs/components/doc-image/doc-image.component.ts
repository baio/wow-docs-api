import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Input,
    Output,
} from '@angular/core';
import { Doc } from '../../models';

@Component({
    selector: 'app-doc-image',
    templateUrl: 'doc-image.component.html',
    styleUrls: ['doc-image.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppDocImageComponent {
    @Input() doc: Doc;
    constructor() {}
}
