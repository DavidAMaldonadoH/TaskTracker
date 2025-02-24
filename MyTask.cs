public class MyTask
{
    public int Id { get; set; }
    public string Description { get; set; }
    public MyTaskStatus Status { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }

    public MyTask(int id, string description)
    {
        Id = id;
        Description = description;
        Status = MyTaskStatus.TODO;
        CreatedAt = DateTime.UtcNow.ToString();
        UpdatedAt = DateTime.UtcNow.ToString();
    }

    public void SetDescription(string description)
    {
        Description = description;
        UpdatedAt = DateTime.UtcNow.ToString();
    }

    public void SetStatus(MyTaskStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow.ToString();
    }


    public override string ToString()
    {
        return $"{Id} | {Description} | {Status.ToString()} | {CreatedAt} | {UpdatedAt}";
    }
}

public enum MyTaskStatus
{
    DONE,
    TODO,
    IN_PROGRESS
}