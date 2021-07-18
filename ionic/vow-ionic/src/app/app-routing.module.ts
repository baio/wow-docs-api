import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { AppAuthenticateComponent, AppAuthGuard } from 'src/libs/ionic/auth';
import { routes as tabsRoutes } from './tabs/tabs-routing.module';

const routes: Routes = [
    {
        path: 'login',
        component: AppAuthenticateComponent,
    },
    {
        path: '',
        children: tabsRoutes,
        canActivateChild: [AppAuthGuard],
    },
];
@NgModule({
    imports: [
        RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules }),
    ],
    exports: [RouterModule],
})
export class AppRoutingModule {}
