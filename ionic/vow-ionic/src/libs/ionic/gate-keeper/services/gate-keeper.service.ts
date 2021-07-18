import { Injectable } from '@angular/core';
import { AvailableResult, NativeBiometric } from 'capacitor-native-biometric';

@Injectable()
export class GateKeeperService {
    async tryEnter() {
        const result = await NativeBiometric.isAvailable();
        const isAvailable = result.isAvailable;
        if (isAvailable) {
            try {
                // Authenticate using biometrics before logging the user in
                const identity = await NativeBiometric.verifyIdentity({
                    reason: 'Безопасный вход',
                    title: 'Для использования приложения необходимо авторизоваться',
                    subtitle: 'Данные авторизации будут проверены устройством',
                });
                console.log('!!!! identity', identity);

                // Get user's credentials
                const credentials = await NativeBiometric.getCredentials({
                    server: 'www.vow-docs.com',
                });

                console.log('credentials !!!', credentials);
            } catch (err) {
                if (err.message === 'No credentials found') {
                    this.setCredentials();
                }
                console.warn('gate keeper error !!!', err);
            }
        } else {
            console.warn('NativeBiometric is not available !!!');
        }
    }

    setCredentials() {
        return NativeBiometric.setCredentials({
            server: 'www.vow-docs.com',
            username: 'vow-user',
            password: '1234',
        });
    }
}
