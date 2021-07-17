import {
    Doc,
    DocFormatted,
    DocPassportRFMainPage,
    DocView,
} from '../../models';
import { passportRFMainPage } from './passport-rf-main-page';

export const docFormattedToView = (doc: DocFormatted): DocView => {
    if (doc.kind === 'passport-rf-main-page') {
        return passportRFMainPage(doc);
    } else {
        return null;
    }
};
