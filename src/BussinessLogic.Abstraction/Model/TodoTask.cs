namespace BussinessLogic.Abstraction.Model
{
    public class TodoTask
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public TodoTaskStatus Status { get; set; }
    }
}