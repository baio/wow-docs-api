import { Injectable } from '@angular/core';
import { Directory, Encoding, Filesystem } from '@capacitor/filesystem';
import { Doc } from '../models';

@Injectable()
export class DocsRepositoryService {
    async updateDoc(doc: Doc, updateImage = false) {
        // create record in db
        const data = JSON.stringify(doc);
        await Filesystem.writeFile({
            path: `docs/${doc.id}.txt`,
            data,
            directory: Directory.Data,
            encoding: Encoding.UTF8,
        });
    }

    async readDoc() {
        Filesystem.readdir({
            directory: Directory.Data,
            path: 'docs'
        });
    }
}
