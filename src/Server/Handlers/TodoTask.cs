using System;

namespace BxTestTask.Handlers
{
  public class TodoTask
  {
    public Guid Id { get; init; }
    public string Name { get; init; }
    public int Priority { get; init; }
    public TodoTaskStatus Status { get; init; }
  }
}