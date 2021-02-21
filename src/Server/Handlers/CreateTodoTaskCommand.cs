namespace BxTestTask.Handlers
{
  public class CreateTodoTaskCommand
  {
    public string Name { get; init; }
    public int Priority { get; init; }
    public TodoTaskStatus Status { get; init; }
  }
}