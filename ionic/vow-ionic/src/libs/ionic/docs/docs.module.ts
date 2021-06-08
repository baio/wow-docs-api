import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { AppDocumentsWorkspaceComponent } from './components/documents-workspace/documents-workspace.component';
import { AppUploadImageButtonComponent } from './components/upload-image-button/upload-image-button.component';
import { DocsEffects } from './ngrx/effects';
import { docsReducer } from './ngrx/reducer';
import { DocsDataAccessService } from './services/docs.data-access.service';
import { ImageService } from './services/image.service';

@NgModule({
    imports: [
        IonicModule,
        CommonModule,
        StoreModule.forFeature('docs', docsReducer),
        EffectsModule.forFeature([DocsEffects]),
    ],
    declarations: [
        AppDocumentsWorkspaceComponent,
        AppUploadImageButtonComponent,
    ],
    providers: [DocsDataAccessService, ImageService],
    exports: [AppDocumentsWorkspaceComponent],
})
export class AppDocsModule {}
