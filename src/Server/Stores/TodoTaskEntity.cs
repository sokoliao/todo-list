using System;

namespace BxTestTask.Stores
{
  public class TodoTaskEntity
  {
    public Guid Id { get; init; }
    public int Priority { get; init; }
    public string Name { get; init; }
    public TodoTaskEntityStatus Status { get; init; }
  }
  public enum TodoTaskEntityStatus
  {
    NotStarted,
    InProgress,
    Completed
  }
}