export interface DocUpload {
    status: 'progress' | 'success' | 'error';
    error?: string;
}

export interface DocStored {
    provider: 'yandex';
    url: string;
}
export interface Doc {
    id: string;
    imgBase64: string;
    upload: DocUpload;
    stored?: DocStored;
}

export interface DocsState {
    docs: { [id: string]: Doc };
}
