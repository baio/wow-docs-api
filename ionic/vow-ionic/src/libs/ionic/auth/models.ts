export type AuthProvider = 'yandex';

export interface YaAuthState {
    provider: 'yandex';
    token: string;
}

export type AuthState = YaAuthState;

export interface UserAuthState {
    authState: AuthState | null;
}
