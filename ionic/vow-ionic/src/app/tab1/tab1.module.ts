import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { AppDocsModule } from 'src/libs/ionic/docs/docs.module';
import { ExploreContainerComponentModule } from '../explore-container/explore-container.module';
import { Tab1Page } from './tab1.page';

@NgModule({
    imports: [
        IonicModule,
        CommonModule,
        FormsModule,
        ExploreContainerComponentModule,
        // Tab1PageRoutingModule,
        AppDocsModule,
    ],
    declarations: [Tab1Page],
})
export class Tab1PageModule {}
