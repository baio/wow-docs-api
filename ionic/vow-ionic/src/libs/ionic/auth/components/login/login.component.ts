import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
    selector: 'app-login',
    templateUrl: 'login.component.html',
    styleUrls: ['login.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppLoginComponent {
    pin = '';
    constructor() {
        console.log('??? login');
    }

    onButtonClick(value: string) {
        console.log('***');
    }
}
