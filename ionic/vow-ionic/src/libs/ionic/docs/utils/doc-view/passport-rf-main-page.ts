import { DocPassportRFMainPage, DocView } from '../../models';
import { format } from 'date-fns';

export const passportRFMainPage = (doc: DocPassportRFMainPage): DocView => ({
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
                value:
                    doc.issueDate &&
                    format(new Date(doc.issueDate), 'dd.MM.yyyy'),
            },
            col2: {
                label: 'Код подразделения',
                value: doc.departmentCode,
            },
        },
        {
            col1: {
                label: 'Пол',
                value: doc.sex === 'male' ? 'мужской' : 'женский',
            },
            col2: {
                label: 'Дата рождения',
                value:
                    doc.dateOfBirth &&
                    format(new Date(doc.dateOfBirth), 'dd.MM.yyyy'),
            },
        },
        {
            label: 'Место рождения',
            value: doc.placeOfBirth,
        },
    ],
});
