import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { GateKeeperService } from './services/gate-keeper.service';

@NgModule({
    imports: [IonicModule, CommonModule],
    declarations: [],
    providers: [GateKeeperService],
})
export class AppGateKeeperModule {}
