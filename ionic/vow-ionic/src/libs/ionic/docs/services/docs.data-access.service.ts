import { Injectable } from '@angular/core';
import { timer } from 'rxjs';
import { map, mapTo } from 'rxjs/operators';
import { DocState } from '../models';

@Injectable()
export class DocsDataAccessService {
    uploadImage(id: string, base64: string) {
        return timer(50).pipe(mapTo(null));
    }

    getDocumentState(id: string) {
        return timer(100).pipe(
            mapTo({
                stored: {
                    provider: 'yandex',
                    url: 'https://mock',
                },
                parsed: {
                    words: ['lol', 'kek'],
                },
                labeled: {
                    label: 'passport-rf-main-page',
                },
                formatted: {
                    kind: 'passport-rf-main-page',
                    lastName: 'putilov',
                    firstName: 'max',
                    middleName: 'alexandrovich',
                    identifier: '1111 123456',
                    issuer: 'kek lol',
                    issueDate: '11.03.1980',
                    sex: 'male',
                    dateOfBirth: '11.05.1980',
                    placeOfBirth: 'russia',
                    departmentCode: '111-222'
                },
            } as DocState)
        );
    }
}
