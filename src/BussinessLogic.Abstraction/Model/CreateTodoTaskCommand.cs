namespace BussinessLogic.Abstraction.Model
{
    public class CreateTodoTaskCommand
    {
        public string Name { get; set; }
        public int Priority { get; set; }
        public TodoTaskStatus Status { get; set; }
    }
}