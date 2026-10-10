# Матрица требований

| Требование ЛР1 | Поле или сущность | Запрос | Критерий |
|---|---|---|---|
| Диспетчер создаёт заявку | CableTicket, title, siteId, description | POST /api/cable-tickets | Критерий 1 |
| Диспетчер назначает электромонтёра | assigneeUserId | PATCH /api/cable-tickets/{id}/assignee | Критерий 2 |
| Электромонтёр переводит в работу | status (New → InProgress) | PATCH /api/cable-tickets/{id}/status | Критерий 3 |
| Электромонтёр закрывает заявку | status (InProgress → Closed) | PATCH /api/cable-tickets/{id}/status | Критерий 3 |
| Диспетчер видит все заявки | CableTicket (список) | GET /api/cable-tickets | — |
| Электромонтёр видит только свои | assigneeUserId (фильтр) | GET /api/cable-tickets?assigneeUserId=8 | — |
| Справочник участков только чтение | NetworkSite | GET /api/network-sites | — |

Каждому из трёх критериев ЛР1 соответствует поле в модели
и конкретный HTTP-запрос.
