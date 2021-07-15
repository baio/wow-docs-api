import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppDocEditWorkspaceComponent } from 'src/libs/ionic/docs/components/doc-edit-workspace/doc-edit-workspace.component';
import { Tab1Page } from './tab1.page';

export const routes: Routes = [
    {
        path: '',
        component: Tab1Page,
    },
    {
        path: ':id',
        component: AppDocEditWorkspaceComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class Tab1PageRoutingModule {}
