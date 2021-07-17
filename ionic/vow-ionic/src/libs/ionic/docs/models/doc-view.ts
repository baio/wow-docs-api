export interface DocViewField {
    label: string;
    value: string;
}

export interface DocView2ColRow {
    col1: DocViewField;
    col2: DocViewField;
}

export type DocViewFieldOrRow = DocViewField | DocView2ColRow;

export interface DocView {
    title: string;
    fields: DocViewFieldOrRow[];
}
