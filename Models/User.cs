namespace myTodoList.Models;
public enum UserType{User,Manager};

public class User{
    public int Id { get; set; } 
    public string? Name {get; set;}
    public string? Password{get;set;}
    public UserType UserType { get; set; }

}