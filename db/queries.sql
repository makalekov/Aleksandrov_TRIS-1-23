-- 1. Фильтр: только новые заявки.
SELECT number, title, status, created_at
FROM cable_tickets
WHERE status = 'New';

-- 2. Сортировка: сначала самые свежие заявки.
SELECT number, title, status, created_at
FROM cable_tickets
ORDER BY created_at DESC;

-- 3. JOIN: показать название участка вместо одного site_id.
SELECT t.number, t.title, s.name AS site_name, t.status
FROM cable_tickets AS t
JOIN network_sites AS s ON s.id = t.site_id
ORDER BY t.created_at DESC;

-- 4. GROUP BY: посчитать заявки по каждому статусу.
SELECT status, COUNT(*) AS ticket_count
FROM cable_tickets
GROUP BY status
ORDER BY status;

-- Проверка CHECK (запускать отдельно, запрос должен завершиться ошибкой):
-- INSERT INTO cable_tickets
--     (number, title, description, site_id, status, created_by_user_id)
-- VALUES
--     ('КБ-999', 'Тест недопустимого статуса', 'Проверочная запись для CHECK.',
--      1, 'Done', 1);
