import {
    ChangeDetectionStrategy,
    Component,
    Input,
    OnInit,
} from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { fromPairs } from 'lodash/fp';
import { Doc, DocFormatted, DocLabel, OptItem } from '../../models';

export interface UploadImageModalView {
    doc: Doc;
}

export interface DocFormFieldBase {
    name: string;
    label: string;
}

export interface DocFormTextField extends DocFormFieldBase {
    kind: 'text';
}

export interface DocFormNumberField extends DocFormFieldBase {
    kind: 'number';
}

export interface DocFormDateField extends DocFormFieldBase {
    kind: 'date';
}

export interface DocFormTextAreaField extends DocFormFieldBase {
    kind: 'text-area';
}

export interface DocFormSelectField extends DocFormFieldBase {
    kind: 'select';
    items: OptItem[];
}

export type DocFormField =
    | DocFormTextField
    | DocFormDateField
    | DocFormNumberField
    | DocFormTextAreaField
    | DocFormSelectField;

export interface DocForm {
    fields: DocFormField[];
}

const docFormRFPassportMainPage: DocForm = {
    fields: [
        {
            kind: 'text',
            name: 'lastName',
            label: 'Фамилия',
        },
        {
            kind: 'text',
            name: 'firstName',
            label: 'Имя',
        },
        {
            kind: 'text',
            name: 'middleName',
            label: 'Отчество',
        },
        {
            kind: 'number',
            name: 'identifier',
            label: 'Номер',
        },
        {
            kind: 'text-area',
            name: 'issuer',
            label: 'Пасспорт выдан',
        },
        {
            kind: 'date',
            name: 'issueDate',
            label: 'Дата выдачи',
        },
        {
            kind: 'select',
            name: 'sex',
            label: 'Пол',
            items: [
                {
                    key: 'male',
                    label: 'male',
                },
                {
                    key: 'female',
                    label: 'female',
                },
            ],
        },
        {
            kind: 'date',
            name: 'dateOfBirth',
            label: 'Дата рождения',
        },
        {
            kind: 'text-area',
            name: 'placeOfBirth',
            label: 'Место рождения',
        },
    ],
};

const docFormsHash = {
    passportRFMainPage: docFormRFPassportMainPage,
};

const createForm = (formBuilder: FormBuilder, doc: DocForm) => {
    const formGroupConfig = fromPairs(
        doc.fields.map((field) => [field.name, []])
    );

    const form = formBuilder.group(formGroupConfig);

    return form;
};

@Component({
    selector: 'app-doc-edit-form',
    templateUrl: 'doc-edit-form.component.html',
    styleUrls: ['doc-edit-form.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppDocEditFormComponent implements OnInit {
    formGroup: FormGroup;
    docForm: DocForm;
    _docLabel: DocLabel;
    @Input() set docLabel(val: DocLabel) {
        if (this._docLabel !== val) {
            this._docLabel = val;
            this.updateForm();
        }
    }
    get docLabel() {
        return this._docLabel;
    }
    @Input() docFormatted: DocFormatted;

    constructor(private readonly fb: FormBuilder) {}

    ngOnInit() {
        this.updateForm();
    }

    trackByField(_, field: DocFormField) {
        return field.name;
    }

    trackByOptItem(_, optItem: OptItem) {
        return optItem.key;
    }

    private updateForm() {
        if (this.docLabel === 'passport-rf-main-page') {
            this.docForm = docFormsHash.passportRFMainPage;
            this.formGroup = createForm(this.fb, this.docForm);
            if (this.docFormatted) {
                this.formGroup.patchValue(this.docFormatted);
            }
        } else {
            this.docForm = null;
            this.formGroup = null;
        }
    }
}
