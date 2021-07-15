import { Injectable } from '@angular/core';
import { Doc } from '../docs/models';
import { schemaV1 } from './db.schema';
import { SqLiteService } from './sq-lite.service';
import { SQLiteDBConnection } from '@capacitor-community/sqlite';

@Injectable()
export class DbService {
    private db: SQLiteDBConnection;
    constructor(private readonly sqLite: SqLiteService) {
    }

    async init() {
        if (this.db) {
            throw new Error('Db is already initialized');
        }
        const dbName = 'testEncryption';
        await this.sqLite.deleteOldDatabases();
        await this.sqLite.checkConnectionsConsistency();
        this.db = await this.sqLite.createConnection(
            dbName,
            false,
            'no-encryption',
            1
        );

        // open db testEncryption
        await this.db.open();

        // create tables in db
        const execSchemaResult = await this.db.execute(schemaV1);
        console.log(
            '$$$ ret.changes.changes in db ' + execSchemaResult.changes.changes
        );

        if (execSchemaResult.changes.changes < 0) {
            return Promise.reject(new Error('Execute createSyncTable failed'));
        } else if (execSchemaResult.changes.changes > 0) {
            const now = new Date().toISOString();
            await this.db.setSyncDate(now);
            console.log('$$$ schema synced ', now);
        }
    }

    runCommand(command: string, params: any[]) {
        return this.db.run(command, params);
    }

    runQuery(command: string) {
        return this.db.query(command);
    }
}
