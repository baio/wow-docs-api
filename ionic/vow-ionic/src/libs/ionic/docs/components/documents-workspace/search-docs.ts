import { sortBy } from 'lodash/fp';
import { Doc, DocLabel } from '../../models';
import Fuse from 'fuse.js';

const getDocTypeName = (docLabel: DocLabel) => {
    switch (docLabel) {
        case 'passport-rf-main-page':
            return 'паспорт';
        default:
            return null;
    }
};

export const searchDocs = (docs: Doc[], search: string) => {
    if (search) {
        search = search.toLowerCase();

        const options = {
            includeScore: true,
            keys: [
                {
                    name: 'typeName',
                    weight: 0.5,
                },
                {
                    name: 'formatted.lastName',
                    weight: 0.9,
                },
                {
                    name: 'formatted.firstName',
                    weight: 0.3,
                },
                {
                    name: 'formatted.middleName',
                    weight: 0.1,
                },
                {
                    name: 'tags',
                    weight: 0.3,
                },
            ],
        };
        const typedDocs = docs.map((m) => ({
            typeName: m.labeled ? getDocTypeName(m.labeled.label) : null,
            ...m,
        }));
        const fuse = new Fuse(typedDocs, options);
        const searchRes = fuse.search(search);
        const res = searchRes.map((m) => m.item);
        return res;
    } else {
        return docs;
    }
};
