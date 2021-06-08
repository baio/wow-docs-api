export interface Doc {
    id: string;
    imgBase64: string;
}

export interface DocsState {
    docs: { [id: string]: Doc };
}
