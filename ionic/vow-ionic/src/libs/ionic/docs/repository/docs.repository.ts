import { Injectable } from '@angular/core';
import { DbService } from '../../db';
import { Doc, DocState } from '../models';

@Injectable()
export class DocsRepositoryService {
    constructor(private readonly db: DbService) {}

    async addDoc(id: string, imgBase64: string) {
        // add one user with statement and values
        const sqlcmd = 'INSERT INTO docs (id,imgBase64) VALUES (?,?)';
        const values = [id, imgBase64];
        const res = await this.db.runCommand(sqlcmd, values);
        console.log('$$$ addDoc result', res);
    }

    async updateDocState(id: string, docState: DocState) {
        // add one user with statement and values
        const sqlcmd =
            'UPDATE docs SET docStoredProvider = ?, docStoredUrl = ?, docParsedWords = ?, docLabeledLabel = ? WHERE id = ?';
        const values = [
            docState.stored?.provider,
            docState.stored?.url,
            docState.parsed?.words && docState.parsed?.words.join(','),
            docState.labeled?.label,
            id,
        ];
        const res = await this.db.runCommand(sqlcmd, values);
        console.log('$$$ updateDocState result', res);
    }

    async getDocs() {
        // add one user with statement and values
        const sqlcmd = 'SELECT * FROM docs;';
        const res = await this.db.runQuery(sqlcmd);
        console.log('$$$ getDocs result', res);
        const values = res.values;
        return values.map(
            (m) =>
                ({
                    id: m.id,
                    imgBase64: m.imgBase64,
                    date: new Date().toISOString(),
                    upload: {},
                } as Doc)
        );
    }
}
