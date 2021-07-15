export const schemaV1 = `
CREATE TABLE IF NOT EXISTS docs (
    id TEXT PRIMARY KEY NOT NULL,
    imgBase64 TEXT,
    docStoredProvider TEXT,
    docStoredUrl TEXT,
    docParsedWords TEXT,
    docLabeledLabel TEXT
);
CREATE TABLE IF NOT EXISTS parsedPassportDocs (
    id TEXT PRIMARY KEY NOT NULL,
    firstName: TEXT,
    issueDate: TEXT
);

PRAGMA user_version = 1;
`;
