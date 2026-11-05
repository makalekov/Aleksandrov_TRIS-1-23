Лабораторные работы Барс Груп


**Группа: ТРИС-1-23**

**Студент: Александров Максим Игоревич**

**Курс: 4**

Тема: Учет состояния сети с прогнозированием потенциальных проблем 
(подсистема «Управление техническим обслуживанием» АО «Компания ТрансТелеКом»)
Подходящая тема из списка №14. Повреждение кабеля



## Лабораторная работа 1.

| № | Тема | Статус | Ссылка |
|---|------|--------|--------|
| 1 | Анализ, требования и выбор темы | В работе |[docs/lab-01-requirements.md]|

Стек:

- Vue 3
- ASP.NET Core
- PostgreSQL
Структура репозитория

## Лабораторная работа 2. Архитектура и модель данных

- [Архитектура](docs/architecture.md)
- [Модель данных](docs/data-model.md)
- [Контракт API](docs/api-contract.md)
- [Матрица требований](docs/requirements-matrix.md)
- [Отчёт](docs/reports/lab-02.md)

## Лабораторная работа 3. Vue: каркас и экраны

- [Проект](frontend/src/App.vue)


Открыть в браузере: http://localhost:5173

Остановить сервер: Ctrl+C в терминале

## Лабораторная работа 4. Vue: компоненты и маршруты

- Адрес `/` открывает список.
- Адрес `/new` открывает форму создания.
- Адрес `/login` открывает форму входа.
- Шапка с меню остается видимой на всех маршрутах.
- Таблица получает массив через props, источник — `ticketsStore.js`.

## Лабораторная работа 5. Vue: форма и список в памяти

- Поля формы связаны через `v-model`.
- Для `siteId` используется модификатор `.number`.
- Отправка формы не перезагружает страницу (`@submit.prevent`).
- Новая запись со статусом `New` появляется в общем списке.
- Тема короче 5 символов отклоняется с текстом ошибки.
- Данные хранятся в памяти и очищаются после F5.

## Лабораторная работа 6. Vue: клиент API

- Список и форма получают данные через асинхронные функции `getTickets` и `createTicket`.
- Реализована имитация сервера (mock) с задержкой и разными кодами ответа.
- Список имеет 4 состояния: загрузка, пустой, данные, ошибка.
- Кнопка создания блокируется во время запроса (защита от двойного клика).
- `id` и `number` новой заявки берутся из ответа сервера, а не придумываются на клиенте.
- Настроены переменные окружения (`.env`), файл с секретами исключен из Git.

## Лабораторная работа 7. Каркас ASP.NET Core Web API

- Создан проект `backend/` (ASP.NET Core Web API).
- Настроен порт `5000`.
- Добавлены служебные адреса: `/health` (проверка живости) и `/info` (информация).
- Доступен Swagger UI: `http://localhost:5000/swagger`.

Открыть в браузере: http://localhost:5000/swagger

Запуск backend из корня репозитория:

```bash
cd backend
dotnet restore
dotnet run
```

## Лабораторная работа 8. REST API заявок

- `POST /api/cable-tickets` — создать заявку (ответ `201`).
- `GET /api/cable-tickets` и `GET /api/cable-tickets/{id}` — список и одна заявка.
- `PATCH /api/cable-tickets/{id}/assignee` — назначить электромонтёра.
- `PATCH /api/cable-tickets/{id}/status` — изменить статус отдельно от назначения.
- Коллекция запросов Postman: [docs/api-collection/cable-tickets.postman_collection.json](docs/api-collection/cable-tickets.postman_collection.json).

Для ЛР8 API работает по адресу `http://localhost:5000`, Swagger — `http://localhost:5000/swagger`.
Заявки хранятся в памяти процесса и исчезают после перезапуска.
`GET /health` проверяет только, что процесс отвечает; он не проверяет создание заявки.

## Лабораторная работа 9. PostgreSQL и SQL

- Схема: [db/schema.sql](db/schema.sql).
- Учебные строки: [db/seed.sql](db/seed.sql).
- Фильтрация, сортировка, JOIN и GROUP BY: [db/queries.sql](db/queries.sql).

Для локального запуска:

```bash
createdb course_tickets
psql -d course_tickets -f db/schema.sql
psql -d course_tickets -f db/seed.sql
psql -d course_tickets -f db/queries.sql
```

Имя БД можно заменить. Пароль — тот, который задан при установке PostgreSQL.

## Лабораторная работа 10. EF Core и PostgreSQL

- `backend/Data/AppDbContext.cs` описывает схему приложения и её ограничения.
- `backend/Migrations/` содержит начальную миграцию и снимок модели.
- `backend/Models/` содержит заявки, участки и пользователей.

Для запуска API задайте локально строку подключения:

```bash
cd backend
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Port=5432;Database=course_tickets;Username=ВАШ_ПОЛЬЗОВАТЕЛЬ;Password=ВАШ_ПАРОЛЬ"
dotnet ef database update
psql -d course_tickets -f ../db/seed.sql
dotnet run
```

В репозиторий попадает только идентификатор хранилища User Secrets, учётные данные хранятся локально.
