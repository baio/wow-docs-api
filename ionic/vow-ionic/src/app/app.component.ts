import { Component, OnDestroy } from '@angular/core';
import { Platform } from '@ionic/angular';
import { DbService, SqLiteService } from 'src/libs/ionic/db';

@Component({
    selector: 'app-root',
    templateUrl: 'app.component.html',
    styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnDestroy {
    constructor(
        private readonly sqLiteService: SqLiteService,
        private readonly dbService: DbService,
        private readonly platform: Platform
    ) {
        this.initializeApp();
    }

    initializeApp() {
        this.platform.ready().then(async () => {
            const ret = await this.sqLiteService.initializePlugin();
            console.log('$$$ in App  this.initPlugin ', ret);
            const res = await this.sqLiteService.echo('Hello World');
            console.log('$$$ from Echo ' + res.value);
            await this.dbService.init();
            console.log('$$$ db initialized');
        });
    }

    ngOnDestroy() {
        console.log('on destroy !!!');
    }
}
