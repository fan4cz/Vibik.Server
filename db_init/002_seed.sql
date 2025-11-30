INSERT INTO
    tasks (
        id,
        name,
        description,
        reward,
        photos_required,
        tags,
        example_path
    )
VALUES
    ('Test1', 'TestNo1', 'Test1 Desc', 1, 1, ARRAY['tag1'], ARRAY['Test1/']),
    ('Test2', 'TestNo2', 'Test2 Desc', 1, 1, ARRAY['tag1'], ARRAY['Test2/']),
    ('Test3', 'TestNo3', 'Test3 Desc', 1, 1, ARRAY['tag1'], ARRAY['Test3/']),
    ('Test4', 'TestNo4', 'Test1 Desc', 1, 1, ARRAY['tag1'], ARRAY['Test4/']);

INSERT INTO
    users_tasks (
     task_id,
     username,
     is_completed,
     start_time,
     photos_path,
     photos_count
    )
VALUES
    ('Test1', 'TestName', '0', NOW(), ARRAY['Test1/'], 1),
    ('Test2', 'TestName', '0', NOW(), ARRAY['Test2/'], 1),
    ('Test3', 'TestName', '0', NOW(), ARRAY['Test3/'], 1),
    ('Test4', 'TestName', '0', NOW(), ARRAY['Test4/'], 1);

INSERT INTO
    users (
     username,
     display_name,
     password_hash,
     exp,
     lvl
    )
VALUES
    ('TestName', 'TestName display', '1234789', 10, 2);