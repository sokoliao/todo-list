using System;
using System.ComponentModel.DataAnnotations;

namespace BxTestTask.Controllers
{
  public class TodoTaskModel
  {
    [Required]
    public Guid Id { get; init; }

    [Required]
    public string Name { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Priority { get; init; }

    [Required]
    public TodoTaskModelStatus Status { get; init; }
  }
}