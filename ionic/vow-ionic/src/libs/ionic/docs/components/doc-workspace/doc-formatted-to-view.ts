import {
    Doc,
    DocFormatted,
    DocPassportRFMainPage,
    DocView,
} from '../../models';

const passportRFMainPage = (doc: DocPassportRFMainPage): DocView => ({
    title: 'Гражданский Пасспорт РФ (главная)',
    fields: [
        {
            label: 'Фамилия Имя Очество',
            value:
                doc.lastName || doc.firstName || doc.middleName
                    ? [doc.lastName, doc.firstName, doc.middleName].join(' ')
                    : null,
        },
        {
            label: 'Серия Номер',
            value: doc.identifier,
        },
        {
            label: 'Паспорт выдан',
            value: doc.issuer,
        },
        {
            col1: {
                label: 'Дата выдачи',
                value: doc.issueDate,
            },
            col2: {
                label: 'Код подразделения',
                value: doc.departmentCode,
            },
        },
        {
            col1: {
                label: 'Пол',
                value: doc.sex,
            },
            col2: {
                label: 'Дата рождения',
                value: doc.dateOfBirth,
            },
        },
        {
            label: 'Место рождения',
            value: doc.placeOfBirth,
        },
    ],
});

export const docFormattedToView = (doc: DocFormatted): DocView => {
    if (doc.kind === 'passport-rf-main-page') {
        return passportRFMainPage(doc);
    } else {
        return null;
    }
};
