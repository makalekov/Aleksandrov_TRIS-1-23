using backend.Dtos;
using backend.Models;

namespace backend.Services;

public interface ITicketService
{
    List<Ticket> GetAll();
    Ticket? GetById(int id);
    Ticket Create(CreateTicketRequest request, int userId);
    Ticket? Assign(int id, int assigneeUserId);
    (Ticket? ticket, string? error) ChangeStatus(int id, string newStatus, int userId);
}
