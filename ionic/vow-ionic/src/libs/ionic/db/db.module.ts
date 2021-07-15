import { NgModule } from '@angular/core';
import { DbService } from './db.service';
import { SqLiteService } from './sq-lite.service';

@NgModule({
    providers: [DbService, SqLiteService],
})
export class DbModule {}
