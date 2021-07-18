import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { DbModule } from '../db/db.module';
import { AppTagsSelectorWorkspaceComponent } from './components/tags-selector-workspace/tags-selector-workspace.component';
import { TagsEffects } from './ngrx/effects';
import { tagsReducer } from './ngrx/reducer';
import { TagsSelectorService } from './services/tags-selector.service';

@NgModule({
    imports: [
        IonicModule,
        CommonModule,
        StoreModule.forFeature('tags', tagsReducer),
        EffectsModule.forFeature([TagsEffects]),
        DbModule,
        FormsModule,
        ReactiveFormsModule,
    ],
    declarations: [AppTagsSelectorWorkspaceComponent],
    providers: [TagsSelectorService],
})
export class AppTagsModule {}
