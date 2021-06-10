export interface DocUpload {
    status: 'progress' | 'success' | 'error';
    error?: string;
}

export interface DocStored {
    provider: 'yandex';
    url: string;
}

export interface DocParsed {
    words: string[];
}

export type DocLabel = 'passport';

export interface DocLabeled {
    label: DocLabel;
}

export interface DocPassportFormatted {
    kind: 'doc-passport-formatted';
    name: string;
    issueDate: string;
}

export type DocFormatted = DocPassportFormatted;

export interface DocState {
    stored?: DocStored;
    parsed?: DocParsed;
    labeled?: DocLabeled;
    formatted?: DocFormatted;
}

export interface Doc extends DocState {
    id: string;
    imgBase64: string;
    upload: DocUpload;
    date: string;
}

export interface DocsState {
    docs: { [id: string]: Doc };
}
