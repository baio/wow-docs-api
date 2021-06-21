export const schemaV1 = `
CREATE TABLE IF NOT EXISTS docs (
    id TEXT PRIMARY KEY NOT NULL,
    imgBase64 TEXT
);
PRAGMA user_version = 1;
`;
