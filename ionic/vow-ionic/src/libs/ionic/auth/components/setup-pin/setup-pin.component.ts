import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Input,
    Output,
} from '@angular/core';

@Component({
    selector: 'app-setup-pin',
    templateUrl: 'setup-pin.component.html',
    styleUrls: ['setup-pin.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppSetupPinComponent {
    pinsNotMatch = false;
    @Input() useBiometric = false;
    @Output() requestBiometric = new EventEmitter<string>();
    @Output() pinSet = new EventEmitter<string>();
    pin = '';
    constructor() {
    }

    onButtonClick(value: string) {
        this.pinsNotMatch = false;
        if (value === 'backspace') {
            if (this.pin && this.pin.length > 0) {
                this.pin = this.pin.substr(0, this.pin.length - 1);
            }
        } else {
            if (this.pin.length < 8) {
                this.pin += value;
            } else {
                if (this.pin1 !== this.pin2) {
                    this.pinsNotMatch = true;
                } else {
                    this.pinSet.emit(this.pin1);
                }
            }
        }
    }

    get pin1() {
        return this.pin ? this.pin.substr(0, 4) : '';
    }

    get pin2() {
        return this.pin && this.pin.length > 4 && this.pin.substr(4);
    }
}
