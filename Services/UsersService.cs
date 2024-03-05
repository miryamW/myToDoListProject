using myTodoList.Models;
using myTodoList.Interface;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text.Json;

namespace myTodoList.Service;


public class UsersService : IUsersService
{
    private List<User> Users;
    private User admin;
    private string fileName;
    ITasksListService tasksListService;
    public UsersService(ITasksListService tasksListService)
    {
        this.tasksListService = tasksListService;
        this.fileName = Path.Combine(/*webHost.ContentRootPath,*/ "Data", "User.json");
        using (var jsonFile = File.OpenText(fileName))
        {
            Users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        
        }
    }

    private void saveToFile(string file)
    {
        File.WriteAllText(file, JsonSerializer.Serialize(Users));
    }

    public List<User> GetAll()=>Users;

    public User GetUser(User user)
    {
        return Users.Find(u => u.Name == user.Name && u.Password == user.Password);
    }

    public User GetById(int id)
    {
        return Users.FirstOrDefault(u => u.Id == id);
    }

    public int Add(User newUser)
    {
        if (Users.Count == 0)
            newUser.Id = 1;
        else
            newUser.Id = Users.Max(p => p.Id) + 1;
        Users.Add(newUser);
        saveToFile(fileName);
        return newUser.Id;
    }

    public bool Update(int id, User newUser)
    {
        if (id != newUser.Id)
            return false;

        var existingUser = GetById(id);
        if (existingUser == null)
            return false;
        var index = Users.IndexOf(existingUser);
        if (index == -1)
            return false;

        Users[index] = newUser;
        saveToFile(fileName);
        
        return true;
    }

    public bool Delete(int id)
    {
        tasksListService.DeleteUserItems(id);
        var existingUser = GetById(id);
        if (existingUser == null)
            return false;

        var index = Users.IndexOf(existingUser);
        if (index == -1)
            return false;

        Users.RemoveAt(index);
        saveToFile(fileName);
        return true;
    }

}
public static class UsersListUtil
{
    public static void AddUser(this IServiceCollection services)
    {
        services.AddSingleton<IUsersService, UsersService>();
    }
}
