CREATE TABLE
    IF NOT EXISTS tasks (
        id VARCHAR(64) PRIMARY KEY,
        name VARCHAR(64),
        description TEXT,
        reward INT,
        photos_required SMALLINT,
        tags VARCHAR(32)[],
        example_path TEXT[]
    );

CREATE TABLE
    IF NOT EXISTS users (
        username VARCHAR(64) PRIMARY KEY,
        display_name VARCHAR(64),
        password_hash VARCHAR(64),
        exp INT,
        lvl INT,
        money INT
    );

CREATE TABLE
    IF NOT EXISTS moderators (tg_userId TEXT PRIMARY KEY);

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'moderation_status') THEN
        CREATE TYPE moderation_status AS ENUM ('default', 'waiting', 'approved', 'reject');
    END IF;
END$$;

CREATE TABLE
    IF NOT EXISTS users_tasks (
        id SERIAL PRIMARY KEY,
        task_id VARCHAR(64),
        username VARCHAR(64),
        moderation_status moderation_status,
        start_time DATE,
        photos_path TEXT[],
        photos_count INT
    );

CREATE TABLE
    IF NOT EXISTS metrics (
        id SERIAL PRIMARY KEY,
        username VARCHAR(64),
        type VARCHAR(64),
        time TIMESTAMP
    );