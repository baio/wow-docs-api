import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Doc } from '../../models';

@Component({
    selector: 'app-doc-display',
    templateUrl: 'doc-display.component.html',
    styleUrls: ['doc-display.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppDocDisplayComponent {
    @Input() doc: Doc;
}
