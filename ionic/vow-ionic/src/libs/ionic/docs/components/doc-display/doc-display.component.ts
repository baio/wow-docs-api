import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Doc, DocView, DocViewField, DocViewFieldOrRow } from '../../models';

@Component({
    selector: 'app-doc-display',
    templateUrl: 'doc-display.component.html',
    styleUrls: ['doc-display.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppDocDisplayComponent {
    @Input() imgBase64: string;
    @Input() docView: DocView;

    getFieldType(field: DocViewFieldOrRow): 'one-col' | 'two-col' {
        if ('col1' in field) {
            return 'two-col';
        } else {
            return 'one-col';
        }
    }
}
