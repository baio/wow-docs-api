import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Input,
    Output,
} from '@angular/core';

@Component({
    selector: 'app-pin-keyboard',
    templateUrl: 'pin-keyboard.component.html',
    styleUrls: ['pin-keyboard.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppPinKeyboardComponent {
    pin = '';

    @Input() useBiometric = false;

    @Output() pinChanged = new EventEmitter<string>();
    @Output() requestBiometric = new EventEmitter<string>();

    constructor() {}

    onButtonClick(value: string) {
        if (value === 'backspace') {
            if (this.pin && this.pin.length > 0) {
                this.pin = this.pin.substr(0, this.pin.length - 1);
                console.log('????', this.pin);
            }
        } else {
            if (this.pin.length < 4) {
                this.pin += value;
            }
        }
        this.pinChanged.emit(this.pin);
    }
}
