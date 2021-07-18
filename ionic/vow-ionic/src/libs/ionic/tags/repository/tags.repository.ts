import { Injectable } from '@angular/core';
import { DbService } from '../../db';
import { Tag } from '../models';

@Injectable()
export class TagsRepositoryService {
    constructor(private readonly db: DbService) {}

    async addTag(tag: Tag) {
        // add one user with statement and values
        const sqlcmd = 'INSERT INTO tags (id,createDate) VALUES (?,?)';
        const values = [tag.name, tag.date];
        const res = await this.db.runCommand(sqlcmd, values);
        console.log('$$$ addTag result', res);
    }

    async removeTag(name: string) {
        // add one user with statement and values
        const sqlcmd = 'DELETE FROM tags WHERE id = ?';
        const values = [name];
        const res = await this.db.runCommand(sqlcmd, values);
        console.log('$$$ deleteTag result', res);
    }

    async getTags() {
        // add one user with statement and values
        const sqlcmd = 'SELECT * FROM tags ORDER BY createDate DESC;';
        const res = await this.db.runQuery(sqlcmd);
        console.log('$$$ getTags result', res);
        const values = res.values;
        return values.map(
            (m) =>
                ({
                    name: m.id,
                    date: m.createDate,
                } as Tag)
        );
    }
}
