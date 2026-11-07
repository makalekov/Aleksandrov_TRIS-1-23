# Отчёт по лабораторной работе №10

**Тема:** EF Core: сервер пишет в базу  
**Предметная область:** заявки на повреждение кабеля  
**Студент:** Александров Максим Игоревич  
**Группа:** ТРИС-1-23

## Цель

Перевести серверное хранение заявок с коллекции в памяти на PostgreSQL через Entity Framework Core. Проверить, что заявка, созданная через API, остаётся доступной после перезапуска API. Строка подключения не должна храниться в Git.

## Реализация

- Добавлены EF Core Design и провайдер Npgsql, совместимые с `net10.0`; миграции запускаются инструментом `dotnet-ef` версии 10.0.0.
- `AppDbContext` содержит наборы заявок, участков и пользователей. Модель явно задаёт имена таблиц и столбцов, identity-ключи, связи, длины полей, `CHECK`, уникальные ограничения и индексы.
- В `backend/Migrations/` добавлены `InitialCreate` и снимок `AppDbContextModelSnapshot`.
- Строка подключения читается по `ConnectionStrings:Default` и хранится в .NET User Secrets вне репозитория. В `appsettings.json` пароль не записывается.
- Сервис выполняет LINQ-запросы и `SaveChangesAsync`; прежние маршруты и JSON-контракт сохранены. `DELETE` не добавлялся.

## Соответствие модели схеме SQL

| Свойство модели | Столбец PostgreSQL | Настройка |
|---|---|---|
| `Ticket.Id` | `cable_tickets.id` | integer identity, первичный ключ |
| `Ticket.SiteId` | `cable_tickets.site_id` | обязательный FK на `network_sites.id`, удаление ограничено |
| `Ticket.CreatedAt` | `cable_tickets.created_at` | `timestamp with time zone`, значение по умолчанию `now()` |
| `Ticket.AssigneeUserId` | `cable_tickets.assignee_user_id` | nullable FK на `users.id`, удаление ограничено |
| `Ticket.CreatedByUserId` | `cable_tickets.created_by_user_id` | обязательный FK на `users.id`, удаление ограничено |
| `Ticket.Title`, `Ticket.Description` | `title`, `description` | длины 5–80 и 10–500 символов |
| `Ticket.Status` | `status` | допустимы `New`, `InProgress`, `Closed`, `Cancelled` |

Пример LINQ-запроса и соответствующего SQL для списка заявок назначенного исполнителя:

```csharp
db.Tickets.AsNoTracking()
    .Where(ticket => ticket.AssigneeUserId == assigneeUserId.Value)
    .OrderBy(ticket => ticket.Id)
    .ToListAsync(cancellationToken);
```

```sql
SELECT c.id, c.assignee_user_id, c.created_at, c.created_by_user_id,
       c.description, c.number, c.site_id, c.status, c.title
FROM cable_tickets AS c
WHERE c.assignee_user_id = @assigneeUserId
ORDER BY c.id;
```

## Применение миграции

Миграция `20261105140652_InitialCreate` применена к локальной базе `course_tickets` на PostgreSQL 18.6. Таблицы после применения сверены с SQL-моделью ЛР9; демонстрационные данные загружены из `db/seed.sql`. Повторный запуск `dotnet ef database update` сообщил, что база актуальна и миграции не требуются.

## Проверка API и рестарт-тест

Проверка выполнена на `http://localhost:5000`:

1. `POST /api/cable-tickets` вернул `201 Created`; создана заявка `id=5`, `number=КБ-105`, `title=Проверка записи EF`, `status=New`.
2. `PATCH /api/cable-tickets/5/assignee` назначил исполнителя `2` и вернул `200`.
3. `PATCH /api/cable-tickets/5/status` последовательно установил `InProgress` и `Closed`; оба запроса вернули `200`.
4. API остановлен и запущен повторно. `GET /api/cable-tickets/5` вернул `200` с тем же ID, номером, темой, исполнителем и статусом `Closed`.
5. `GET /api/cable-tickets/99999` вернул `404`.
6. `GET /api/cable-tickets?assigneeUserId=2` вернул записи из PostgreSQL, включая заявку `id=5`; SQL-лог EF Core подтвердил фильтр по исполнителю и сортировку по ID.

В таблице `__EFMigrationsHistory` присутствует `20261105140652_InitialCreate`; строка `id=5` после рестарта найдена в `cable_tickets` со статусом `Closed` и `assignee_user_id=2`.

| Поле | Ответ POST до перезапуска | Ответ GET после перезапуска |
|---|---|---|
| `id` | `5` | `5` |
| `number` | `КБ-105` | `КБ-105` |
| `title` | `Проверка записи EF` | `Проверка записи EF` |
| `status` | `New` при создании, далее PATCH установил `Closed` | `Closed` |
| `assigneeUserId` | PATCH установил `2` | `2` |

## Результат

Сборка backend завершилась без предупреждений и ошибок. NuGet-проверка не обнаружила уязвимых пакетов. Создание, чтение, назначение, смена статуса и сохранение после перезапуска подтверждены фактическими HTTP-запросами к API и запросом к PostgreSQL.
