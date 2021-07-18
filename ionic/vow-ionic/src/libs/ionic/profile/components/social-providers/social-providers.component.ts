import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Output,
} from '@angular/core';
import { AuthProvider } from '../../models';

@Component({
    selector: 'app-social-providers',
    templateUrl: 'social-providers.component.html',
    styleUrls: ['social-providers.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppSocialProvidersComponent {
    @Output() login = new EventEmitter<AuthProvider>();

    constructor() {}
}
