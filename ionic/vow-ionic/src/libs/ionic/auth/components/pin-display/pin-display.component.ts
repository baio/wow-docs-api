import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
    selector: 'app-pin-display',
    templateUrl: 'pin-display.component.html',
    styleUrls: ['pin-display.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppPinDisplayComponent {
    @Input() pin: string;
}
