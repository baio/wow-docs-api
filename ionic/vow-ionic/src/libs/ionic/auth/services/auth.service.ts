import { Injectable } from '@angular/core';
import { AvailableResult, NativeBiometric } from 'capacitor-native-biometric';
import { v4 } from 'uuid';
import { SecureStoragePlugin } from 'capacitor-secure-storage-plugin';

const SECURITY_KEY = 'VOW_DOCS_SECURITY_KEY';
const PIN_KEY = 'VOW_DOCS_PIN_KEY';

@Injectable()
export class AuthService {
    isAuthenticated = false;
    /*
    async tryEnter(): Promise<EnterSuccess> {
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

                // Get user's credentials
                const credentials = await NativeBiometric.getCredentials({
                    server: 'www.vow-docs.com',
                });

                console.log('credentials !!!', credentials);
                return credentials;
            } catch (err) {
                if (err.message === 'No credentials found') {
                    return this.setCredentials();
                } else {
                    console.warn('gate keeper error !!!', err);
                    return null;
                }
            }
        } else {
            console.warn('NativeBiometric is not available !!!');
            return null;
        }
    }

    setCredentials() {
        const username = v4();
        const password = v4();
        return NativeBiometric.setCredentials({
            server: 'www.vow-docs.com',
            username,
            password,
        });
    }
    */
    login() {}

    async isBiometricAvailable() {
        const result = await NativeBiometric.isAvailable();
        return result.isAvailable;
    }

    getPin() {
        return this.getSecureValue(PIN_KEY);
    }

    getSecurityKey() {
        return this.getSecureValue(SECURITY_KEY);
    }

    async authenticateBiometric(setFirstTimeCredentials: boolean) {
        const result = await NativeBiometric.isAvailable();
        const isAvailable = result.isAvailable;
        if (isAvailable) {
            try {
                // Authenticate using biometrics before logging the user in
                await NativeBiometric.verifyIdentity({
                    reason: 'Безопасный вход',
                    title: 'Для использования приложения необходимо авторизоваться',
                    subtitle: 'Данные авторизации будут проверены устройством',
                });
                if (setFirstTimeCredentials) {
                    await this.setSecurityKey();
                }
                return true;
            } catch (err) {
                return false;
            }
        } else {
            console.warn('NativeBiometric is not available !!!');
            return false;
        }
    }

    async setPin(pin: string, setFirstTimeCredentials: boolean) {
        await SecureStoragePlugin.set({
            key: PIN_KEY,
            value: pin,
        });
        if (setFirstTimeCredentials) {
            await this.setSecurityKey();
        }
    }

    private async setSecurityKey() {
        const securityKey = await this.getSecurityKey();
        if (!securityKey) {
            const newSecurityKey = v4();
            await SecureStoragePlugin.set({
                key: SECURITY_KEY,
                value: newSecurityKey,
            });
        }
    }

    private async getSecureValue(key: string) {
        try {
            const result = await SecureStoragePlugin.get({ key });
            return result.value;
        } catch {
            return null;
        }
    }

    setAuthenticated() {
        this.isAuthenticated = true;
    }
}
