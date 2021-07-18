import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthProvider, AuthState } from '../../models';
import { login, logout } from '../../ngrx/actions';
import { selectAuthState } from '../../ngrx/selectors';

export interface AppProfileWorkspaceView {
    authState: AuthState;
}

@Component({
    selector: 'app-profile-workspace',
    templateUrl: 'profile-workspace.component.html',
    styleUrls: ['profile-workspace.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppProfileWorkspaceComponent {
    readonly view$: Observable<AppProfileWorkspaceView>;

    constructor(private readonly store: Store) {
        this.view$ = store
            .select(selectAuthState)
            .pipe(map((authState) => ({ authState })));
    }

    onLogin(provider: AuthProvider) {
        this.store.dispatch(login({ provider }));
    }

    onLogout() {
        this.store.dispatch(logout());
    }
}
