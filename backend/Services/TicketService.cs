using backend.Dtos;
using backend.Models;

namespace backend.Services;

public class TicketService : ITicketService
{
    private readonly List<Ticket> _tickets = new();
    private readonly object _sync = new();
    private int _nextId = 1;

    public List<Ticket> GetAll()
    {
        lock (_sync) return _tickets.Select(Clone).ToList();
    }

    public Ticket? GetById(int id)
    {
        lock (_sync)
        {
            var ticket = _tickets.FirstOrDefault(t => t.Id == id);
            return ticket is null ? null : Clone(ticket);
        }
    }

    public Ticket Create(CreateTicketRequest request, int userId)
    {
        lock (_sync)
        {
            var id = _nextId++;
            var ticket = new Ticket
            {
                Id = id,
                Number = $"КБ-{100 + id}",
                Title = request.Title.Trim(),
                SiteId = request.SiteId,
                Description = request.Description.Trim(),
                Status = "New",
                CreatedByUserId = userId
            };
            _tickets.Add(ticket);
            return Clone(ticket);
        }
    }

    public Ticket? Assign(int id, int assigneeUserId)
    {
        lock (_sync)
        {
            var ticket = _tickets.FirstOrDefault(t => t.Id == id);
            if (ticket is null) return null;
            ticket.AssigneeUserId = assigneeUserId;
            return Clone(ticket);
        }
    }

    public (Ticket? ticket, string? error) ChangeStatus(int id, string newStatus, int userId)
    {
        lock (_sync)
        {
            var ticket = _tickets.FirstOrDefault(t => t.Id == id);
            if (ticket is null) return (null, "Заявка не найдена");

            if (newStatus == "InProgress" && ticket.AssigneeUserId is null)
                return (null, "Нельзя взять в работу без назначенного исполнителя");
            if ((newStatus == "InProgress" || newStatus == "Closed") && ticket.AssigneeUserId != userId)
                return (null, "Вы не назначены на эту заявку");
            if (newStatus == "Closed" && ticket.Status != "InProgress")
                return (null, "Закрыть можно только заявку в статусе InProgress");
            if (newStatus == "Cancelled" && ticket.Status != "New")
                return (null, "Отменить можно только новую заявку");
            if (newStatus == "InProgress" && ticket.Status != "New")
                return (null, "В работу можно взять только новую заявку");

            ticket.Status = newStatus;
            return (Clone(ticket), null);
        }
    }

    private static Ticket Clone(Ticket ticket) => new()
    {
        Id = ticket.Id,
        Number = ticket.Number,
        Title = ticket.Title,
        SiteId = ticket.SiteId,
        Description = ticket.Description,
        Status = ticket.Status,
        AssigneeUserId = ticket.AssigneeUserId,
        CreatedByUserId = ticket.CreatedByUserId
    };
}
