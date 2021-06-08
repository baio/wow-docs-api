import { Injectable } from '@angular/core';
import { timer } from 'rxjs';
import { map, mapTo } from 'rxjs/operators';

@Injectable()
export class DocsDataAccessService {
    uploadImage(id: string, file: File) {
        return timer(500).pipe(mapTo(null));
    }
}
