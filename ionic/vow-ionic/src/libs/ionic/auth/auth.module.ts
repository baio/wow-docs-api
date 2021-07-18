import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { AppAuthenticateComponent } from './components/authenticate/authenticate.component';
import { AppEnterPinComponent } from './components/enter-pin/enter-pin.component';
import { AppPinDisplayComponent } from './components/pin-display/pin-display.component';
import { AppPinKeyboardComponent } from './components/pin-keyboard/pin-keyboard.component';
import { AppSetupPinComponent } from './components/setup-pin/setup-pin.component';
import { AuthService } from './services/auth.service';

@NgModule({
    imports: [IonicModule, CommonModule],
    declarations: [
        AppAuthenticateComponent,
        AppPinKeyboardComponent,
        AppPinDisplayComponent,
        AppSetupPinComponent,
        AppEnterPinComponent,
    ],
    exports: [],
    providers: [AuthService],
})
export class AppAuthModule {}
