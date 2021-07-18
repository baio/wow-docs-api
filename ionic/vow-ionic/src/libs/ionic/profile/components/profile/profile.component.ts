import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Input,
    Output,
} from '@angular/core';
import { Store } from '@ngrx/store';
import { AuthState } from '../../models';
import { login } from '../../ngrx/actions';

@Component({
    selector: 'app-profile',
    templateUrl: 'profile.component.html',
    styleUrls: ['profile.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppProfileComponent {
    @Input() authState: AuthState;

    @Output() logout = new EventEmitter();

    constructor() {}
}
