import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { AppLoginComponent } from './components/login/login.component';
import { AppPinDisplayComponent } from './components/pin-display/pin-display.component';
import { AppPinKeyboardComponent } from './components/pin-keyboard/pin-keyboard.component';
import { AuthService } from './services/auth.service';

@NgModule({
    imports: [IonicModule, CommonModule],
    declarations: [
        AppLoginComponent,
        AppPinKeyboardComponent,
        AppPinDisplayComponent,
    ],
    exports: [],
    providers: [AuthService],
})
export class AppAuthModule {}
