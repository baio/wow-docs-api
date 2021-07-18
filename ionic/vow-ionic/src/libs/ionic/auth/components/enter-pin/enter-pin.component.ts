import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Input,
    Output,
} from '@angular/core';

@Component({
    selector: 'app-enter-pin',
    templateUrl: 'enter-pin.component.html',
    styleUrls: ['enter-pin.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppEnterPinComponent {
    @Input() useBiometric = false;
    @Input() pinError = false;
    @Output() requestBiometric = new EventEmitter();
    @Output() pinEntered = new EventEmitter<string>();

    constructor() {
    }

    onPinChanged(pin: string) {
        this.pinError = false;
        if (pin.length >= 4) {
            this.pinEntered.emit(pin);
        }
    }
}
