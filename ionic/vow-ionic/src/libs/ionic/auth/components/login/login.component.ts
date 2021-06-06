import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { login } from '../../ngrx/actions';

@Component({
    selector: 'app-login',
    templateUrl: 'login.component.html',
    styleUrls: ['login.component.scss'],
})
export class AppLoginComponent {
    constructor(private readonly store: Store) {}

    onYaLogin() {
        this.store.dispatch(login({ provider: 'yandex' }));
    }
}
