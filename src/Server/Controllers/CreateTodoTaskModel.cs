using System.ComponentModel.DataAnnotations;

namespace BxTestTask.Controllers
{
  public class CreateTodoTaskModel
  {
    [Required]
    public string Name { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Priority { get; init; }

    [Required]
    public TodoTaskModelStatus Status { get; init; }
  }
}