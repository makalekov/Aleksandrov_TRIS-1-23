using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace backend.Services;

public class TicketService(AppDbContext db) : ITicketService
{
    public async Task<List<Ticket>> GetAll(int? assigneeUserId, CancellationToken cancellationToken)
    {
        var query = db.Tickets.AsNoTracking();
        if (assigneeUserId.HasValue)
            query = query.Where(ticket => ticket.AssigneeUserId == assigneeUserId.Value);
        return await query.OrderBy(ticket => ticket.Id).ToListAsync(cancellationToken);
    }

    public Task<Ticket?> GetById(int id, CancellationToken cancellationToken) =>
        db.Tickets.AsNoTracking().FirstOrDefaultAsync(ticket => ticket.Id == id, cancellationToken);

    public async Task<Ticket> Create(CreateTicketRequest request, int userId, CancellationToken cancellationToken)
    {
        var maxId = await db.Tickets.Select(ticket => (int?)ticket.Id).MaxAsync(cancellationToken) ?? 0;
        var ticket = new Ticket
        {
            Number = $"КБ-{100 + maxId + 1}",
            Title = request.Title.Trim(),
            SiteId = request.SiteId,
            Description = request.Description.Trim(),
            Status = "New",
            CreatedByUserId = userId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        db.Tickets.Add(ticket);
        try
        {
            await db.SaveChangesAsync(cancellationToken);
            return ticket;
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new TicketNumberConflictException("Не удалось сформировать уникальный номер заявки", exception);
        }
    }

    public async Task<(Ticket? ticket, string? error)> Assign(int id, int assigneeUserId, CancellationToken cancellationToken)
    {
        var ticket = await db.Tickets.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (ticket is null) return (null, "Заявка не найдена");
        if (ticket.Status != "New") return (null, "Назначить исполнителя можно только новой заявке");
        ticket.AssigneeUserId = assigneeUserId;
        await db.SaveChangesAsync(cancellationToken);
        return (ticket, null);
    }

    public async Task<(Ticket? ticket, string? error)> ChangeStatus(int id, string newStatus, int userId, CancellationToken cancellationToken)
    {
        var ticket = await db.Tickets.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (ticket is null) return (null, "Заявка не найдена");

        if (newStatus == "InProgress")
        {
            if (ticket.Status != "New") return (null, "В работу можно взять только новую заявку");
            if (ticket.AssigneeUserId is null) return (null, "Нельзя взять в работу без назначенного исполнителя");
            if (ticket.AssigneeUserId != userId) return (null, "Вы не назначены на эту заявку");
        }
        else if (newStatus == "Closed")
        {
            if (ticket.Status != "InProgress") return (null, "Закрыть можно только заявку в статусе InProgress");
            if (ticket.AssigneeUserId != userId) return (null, "Вы не назначены на эту заявку");
        }
        else if (newStatus == "Cancelled")
        {
            if (ticket.Status != "New") return (null, "Отменить можно только новую заявку");
        }
        else
        {
            return (null, "Возврат заявки к статусу New запрещён");
        }

        ticket.Status = newStatus;
        await db.SaveChangesAsync(cancellationToken);
        return (ticket, null);
    }
}

public sealed class TicketNumberConflictException(string message, Exception innerException) : Exception(message, innerException);
