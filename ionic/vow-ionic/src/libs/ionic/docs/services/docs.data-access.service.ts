import { Injectable } from '@angular/core';
import { timer } from 'rxjs';
import { map, mapTo } from 'rxjs/operators';
import { DocState } from '../models';

@Injectable()
export class DocsDataAccessService {
    uploadImage(id: string, file: File) {
        return timer(5000).pipe(mapTo(null));
    }

    getDocumentState(id: string) {
        return timer(1000).pipe(
            mapTo({
                stored: {
                    provider: 'yandex',
                    url: 'https://mock',
                },
                parsed: {
                    words: ['lol', 'kek'],
                },
                labeled: {
                    label: 'passport',
                },
                formatted: {
                    kind: 'doc-passport-formatted',
                    name: 'max max',
                    issueDate: '10/10/2021',
                },
            } as DocState)
        );
    }
}
