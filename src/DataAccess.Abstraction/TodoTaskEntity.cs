namespace DataAccess.Abstraction
{
    public class TodoTaskEntity
    {
        public string Id { get; set; }
        public int Priority { get; set; }
        public string Name { get; set; }
        public TodoTaskEntityStatus Status { get; set; }
    }
    public enum TodoTaskEntityStatus
    {
        NotStarted,
        InProgress,
        Completed
    }
}