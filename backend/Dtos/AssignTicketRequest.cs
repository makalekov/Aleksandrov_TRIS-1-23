using System.ComponentModel.DataAnnotations;

namespace backend.Dtos;

public class AssignTicketRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Идентификатор исполнителя должен быть больше нуля")]
    public int AssigneeUserId { get; set; }
}
