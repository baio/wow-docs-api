import { Injectable } from '@angular/core';
import { DbService } from '../../db';
import { Doc, DocFormatted, DocPassportRFMainPage, DocState } from '../models';

const getDocFormattedPassportRFMainPageUpdateValues = (
    data: DocPassportRFMainPage
) => ({
    lastName: data.lastName,
    firstMiddleName:
        data.firstName || data.middleName
            ? [data.firstName, data.middleName].join()
            : null,
});

const getDocFormattedUpdateValues = (docFormatted: DocFormatted) => {
    switch (docFormatted.kind) {
        case 'passport-rf-main-page':
            return getDocFormattedPassportRFMainPageUpdateValues(docFormatted);
        default:
            return null;
    }
};
@Injectable()
export class DocsRepositoryService {
    constructor(private readonly db: DbService) {}

    async addDoc(id: string, imgBase64: string) {
        // add one user with statement and values
        const sqlcmd =
            'INSERT INTO docs (id,imgBase64,createDate) VALUES (?,?,?)';
        const values = [id, imgBase64, new Date().getTime()];
        const res = await this.db.runCommand(sqlcmd, values);
        console.log('$$$ addDoc result', res);
    }

    async updateDocState(id: string, docState: DocState) {
        // add one user with statement and values
        const sqlcmd = `UPDATE docs SET storedProvider = ?, storedUrl = ?, parsedWords = ?, labeledLabel = ? WHERE id = ?`;
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

    async updateDocFormatted(id: string, docFormatted: DocFormatted) {
        if (!docFormatted) {
            const sqlCmd =
                'UPDATE docs SET lastName = null, firstMiddleName = null, content = null, labeledLabel = null  WHERE id = ?';
            const res = await this.db.runCommand(sqlCmd, [id]);
            console.log('$$$ updateDocState result', res);
        } else {
            const values = getDocFormattedUpdateValues(docFormatted);
            if (values) {
                const content = JSON.stringify(docFormatted);
                const sqlCmd =
                    'UPDATE docs SET lastName = ?, firstMiddleName = ?, content = ?, labeledLabel = ?  WHERE id = ?';
                const cmdValues = [
                    values.lastName,
                    values.firstMiddleName,
                    content,
                    docFormatted && docFormatted.kind,
                    id,
                ];
                const res = await this.db.runCommand(sqlCmd, cmdValues);
                console.log('$$$ updateDocState result', res);
            } else {
                console.warn(
                    'updateDocFormatted values not found !!!',
                    docFormatted
                );
            }
        }
    }

    async setDocTags(id: string, tags: string[]) {
        const sqlCmd = 'UPDATE docs SET tags = ? WHERE id = ?';
        const cmdValues = [tags && tags.length > 0 ? tags.join(',') : null, id];
        const res = await this.db.runCommand(sqlCmd, cmdValues);
        console.log('$$$ setDocTags result', res);
    }

    async setDocComment(id: string, comment: string) {
        const sqlCmd = 'UPDATE docs SET comment = ? WHERE id = ?';
        const cmdValues = [comment || null, id];
        const res = await this.db.runCommand(sqlCmd, cmdValues);
        console.log('$$$ setDocComment result', res);
    }

    async deleteDoc(id: string) {
        // add one user with statement and values
        const sqlcmd = 'DELETE FROM docs WHERE id = ?';
        const values = [id];
        const res = await this.db.runCommand(sqlcmd, values);
        console.log('$$$ deleteDoc result', res);
    }

    async getDocs() {
        // add one user with statement and values
        const sqlcmd = 'SELECT * FROM docs ORDER BY createDate DESC;';
        const res = await this.db.runQuery(sqlcmd);
        console.log('$$$ getDocs result', res);
        const values = res.values;
        return values.map(
            (m) =>
                ({
                    id: m.id,
                    imgBase64: m.imgBase64,
                    date: m.createDate,
                    upload: {},
                    stored: m.storedProvider
                        ? {
                              provider: m.storedProvider,
                              url: m.storedUrl,
                          }
                        : null,
                    parsed: m.parsedWords
                        ? {
                              words: m.parsedWords.split(','),
                          }
                        : null,
                    labeled: m.labeledLabel
                        ? {
                              label: m.labeledLabel,
                          }
                        : null,
                    formatted: m.content ? JSON.parse(m.content) : null,
                    tags: m.tags ? m.tags.split(',') : [],
                    comment: m.comment,
                } as Doc)
        );
    }
}
