import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Input,
    Output,
} from '@angular/core';
import { Doc } from '../../models';

@Component({
    selector: 'app-full-screen-image',
    templateUrl: 'full-screen-image.component.html',
    styleUrls: ['full-screen-image.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppFullScreenImageComponent {
    @Input() imgBase64: string;
    constructor() {}
}
