import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthProvider, AuthState } from '../../models';
import { login, logout } from '../../ngrx/actions';
import { selectAuthState } from '../../ngrx/selectors';

export interface AppAuthWorkspaceView {
    authState: AuthState;
}

@Component({
    selector: 'app-auth-workspace',
    templateUrl: 'auth-workspace.component.html',
    styleUrls: ['auth-workspace.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppAuthWorkspaceComponent {
    readonly view$: Observable<AppAuthWorkspaceView>;

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
