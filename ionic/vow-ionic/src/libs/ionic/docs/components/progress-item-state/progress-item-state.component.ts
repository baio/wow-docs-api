import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Input,
    Output,
} from '@angular/core';
import { ImageService } from '../../services/image.service';

@Component({
    selector: 'app-progress-item-state',
    templateUrl: 'progress-item-state.component.html',
    styleUrls: ['progress-item-state.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppProgressItemStateComponent {
    @Input() title: string;
    @Input() state: 'progress' | 'success' | 'error';
    constructor() {}
}
