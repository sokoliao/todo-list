using System;
using System.ComponentModel.DataAnnotations;

namespace BxTestTask.Controllers
{
  public class CreateTodoTaskResultModel
  {
    [Required]
    public Guid Id { get; init; }
  }
}