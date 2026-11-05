-- Учебные записи для демонстрации. Идентификаторы заданы явно для
-- понятных примеров; последовательности identity синхронизируются ниже.
INSERT INTO network_sites (id, name) VALUES
    (1, 'Москва — Замоскворечье'),
    (2, 'Северный участок — Север-3')
ON CONFLICT (id) DO UPDATE SET name = EXCLUDED.name;

INSERT INTO users (id, login, role) VALUES
    (1, 'dispatcher.demo', 'Dispatcher'),
    (2, 'electrician.demo', 'Electrician')
ON CONFLICT (id) DO UPDATE SET login = EXCLUDED.login, role = EXCLUDED.role;

INSERT INTO cable_tickets (
    id, number, title, description, site_id, status, created_at,
    created_by_user_id, assignee_user_id
) VALUES
    (1, 'КБ-201', 'Проверить обрыв на линии Москва',
     'Поступило сообщение о потере сигнала после грозы.', 1, 'New',
     '2026-11-01 09:00:00+03', 1, NULL),
    (2, 'КБ-202', 'Восстановить кабель Север-3',
     'Повреждена магистральная линия, бригада выехала.', 2, 'InProgress',
     '2026-11-01 10:00:00+03', 1, 2),
    (3, 'КБ-203', 'Заменить повреждённый участок',
     'Кабель заменён, связь на участке восстановлена.', 1, 'Closed',
     '2026-11-01 11:00:00+03', 1, 2),
    (4, 'КБ-204', 'Проверить резервную линию',
     'Заявка отменена после проверки диспетчером.', 2, 'Cancelled',
     '2026-11-01 12:00:00+03', 1, NULL)
ON CONFLICT (id) DO UPDATE SET
    number = EXCLUDED.number,
    title = EXCLUDED.title,
    description = EXCLUDED.description,
    site_id = EXCLUDED.site_id,
    status = EXCLUDED.status,
    created_at = EXCLUDED.created_at,
    created_by_user_id = EXCLUDED.created_by_user_id,
    assignee_user_id = EXCLUDED.assignee_user_id;

SELECT setval(pg_get_serial_sequence('network_sites', 'id'),
              (SELECT MAX(id) FROM network_sites), true);
SELECT setval(pg_get_serial_sequence('users', 'id'),
              (SELECT MAX(id) FROM users), true);
SELECT setval(pg_get_serial_sequence('cable_tickets', 'id'),
              (SELECT MAX(id) FROM cable_tickets), true);
