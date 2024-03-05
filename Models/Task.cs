namespace myTodoList.Models;
public class Task
{
    public int Id { get; set; } 
    public string? Description { get; set; }   
    public bool? IsDone { get; set; }
    public int UserId { get; set; }
}