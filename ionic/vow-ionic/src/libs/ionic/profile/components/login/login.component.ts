import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Output,
} from '@angular/core';
import { Store } from '@ngrx/store';
import { AuthProvider } from '../../models';
import { login } from '../../ngrx/actions';

@Component({
    selector: 'app-login',
    templateUrl: 'login.component.html',
    styleUrls: ['login.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppLoginComponent {
    @Output() login = new EventEmitter<AuthProvider>();

    constructor() {}
}
