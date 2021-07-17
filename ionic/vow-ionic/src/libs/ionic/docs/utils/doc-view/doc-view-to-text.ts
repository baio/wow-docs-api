import { flatten } from 'lodash/fp';
import { Doc, DocFormatted, DocView } from '../../models';
import { docFormattedToView } from './doc-formatted-to-view';

export const docViewToText = (docView: DocView): string => {
    const content = flatten(
        docView.fields.map((m) => ('col1' in m ? [m.col1, m.col2] : [m]))
    ).reduce(
        (acc, field) =>
            field.value ? acc + '\n' + `${field.label} : ${field.value}` : acc,
        ''
    );
    return `${docView.title}${content ? '\n' + content : ''}`;
};

export const docToText = (doc: Doc): string => {
    const docFormatted =
        doc.formatted ||
        (doc.labeled?.label
            ? ({ kind: doc.labeled?.label } as DocFormatted)
            : null);
    return docFormatted
        ? docViewToText(docFormattedToView(docFormatted))
        : null;
};
