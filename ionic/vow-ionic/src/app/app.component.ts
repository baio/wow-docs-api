import { Component, OnDestroy } from '@angular/core';
import { AlertController, Platform } from '@ionic/angular';
import { Store } from '@ngrx/store';
import { DbService, SqLiteService } from 'src/libs/ionic/db';
import { rehydrateDocs } from 'src/libs/ionic/docs/ngrx/actions';
import { GateKeeperService } from 'src/libs/ionic/gate-keeper';
import { rehydrateTags } from 'src/libs/ionic/tags/ngrx/actions';

@Component({
    selector: 'app-root',
    templateUrl: 'app.component.html',
    styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnDestroy {
    constructor(
        private readonly sqLiteService: SqLiteService,
        private readonly dbService: DbService,
        private readonly platform: Platform,
        private readonly store: Store,
        private readonly gateKeeper: GateKeeperService,
        private readonly alertController: AlertController
    ) {
        this.initializeApp();
    }

    async initializeApp() {
        await this.platform.ready();
        const credentials = await this.gateKeeper.tryEnter();
        if (credentials) {
            const ret = await this.sqLiteService.initializePlugin();
            console.log('$$$ in App  this.initPlugin ', ret);
            const res = await this.sqLiteService.echo('Hello World');
            console.log('$$$ from Echo ' + res.value);
            await this.dbService.init();
            console.log('$$$ db initialized');
            this.store.dispatch(rehydrateDocs());
            this.store.dispatch(rehydrateTags());
        } else {
        }
    }

    ngOnDestroy() {
        console.log('on destroy !!!');
    }
}
