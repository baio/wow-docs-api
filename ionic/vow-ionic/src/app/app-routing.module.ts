import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { routes as tabsRoutes } from './tabs/tabs-routing.module';

const routes: Routes = [
    {
        path: '',
        children: tabsRoutes,
        //loadChildren: () => import('./tabs/tabs.module').then(m => m.TabsPageModule)
    },
];
@NgModule({
    imports: [
        RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules }),
    ],
    exports: [RouterModule],
})
export class AppRoutingModule {}
