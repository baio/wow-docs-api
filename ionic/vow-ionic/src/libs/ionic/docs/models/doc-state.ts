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

export type DocLabel = 'passport-rf-main-page';

export interface DocLabeled {
    label: DocLabel;
}

export interface DocPassportRFMainPage {
    kind: 'passport-rf-main-page';
    lastName: string;
    firstName: string;
    middleName: string;
    identifier: string;
    issuer: string;
    issueDate: string;
    sex: string;
    dateOfBirth: string;
    placeOfBirth: string;
    departmentCode: string;
}

export type DocFormatted = DocPassportRFMainPage;

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
    date: number;
    tags: string[];
    comment: string;
}

export interface DocsState {
    docs: { [id: string]: Doc };
}
