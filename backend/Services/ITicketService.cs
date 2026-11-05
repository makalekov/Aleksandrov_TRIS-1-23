using backend.Dtos;
using backend.Models;

namespace backend.Services;

public interface ITicketService
{
    Task<List<Ticket>> GetAll(int? assigneeUserId, CancellationToken cancellationToken);
    Task<Ticket?> GetById(int id, CancellationToken cancellationToken);
    Task<Ticket> Create(CreateTicketRequest request, int userId, CancellationToken cancellationToken);
    Task<(Ticket? ticket, string? error)> Assign(int id, int assigneeUserId, CancellationToken cancellationToken);
    Task<(Ticket? ticket, string? error)> ChangeStatus(int id, string newStatus, int userId, CancellationToken cancellationToken);
}
