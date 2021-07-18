import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { AppAuthGuard } from 'src/libs/ionic/auth';
import { AppLoginComponent } from 'src/libs/ionic/auth/components/login/login.component';
import { routes as tabsRoutes } from './tabs/tabs-routing.module';

const routes: Routes = [
    {
        path: 'login',
        component: AppLoginComponent,
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
