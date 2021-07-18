import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { AppLoginComponent } from './components/login/login.component';
import { AuthService } from './services/auth.service';

@NgModule({
    imports: [IonicModule, CommonModule],
    declarations: [AppLoginComponent],
    exports: [],
    providers: [AuthService],
})
export class AppAuthModule {}
