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
    private string fileUsersName;
    private string fileAdminName;
    ITasksListService tasksListService;
    public UsersService(ITasksListService tasksListService)
    {
        this.tasksListService = tasksListService;
        this.fileUsersName = Path.Combine(/*webHost.ContentRootPath,*/ "Data", "User.json");
        this.fileAdminName = Path.Combine(/*webHost.ContentRootPath,*/ "Data", "Admin.json");

        using (var jsonFile = File.OpenText(fileUsersName))
        {
            Users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        }
        using (var jsonFile = File.OpenText(fileAdminName))
        {
            admin = JsonSerializer.Deserialize<User>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        }
    }

    private void saveToFile(string file)
    {
        if (file == fileUsersName)
            File.WriteAllText(file, JsonSerializer.Serialize(Users));
        else
            File.WriteAllText(file, JsonSerializer.Serialize(admin));

    }

    public List<User> GetAll()=>Users;

    public User GetUser(User user)
    {
        if (this.IsAdmin(user))
            return admin;
        return Users.Find(u => u.Name == user.Name && u.Password == user.Password);
    }

    public User GetById(int id)
    {
        if (id == admin.Id)
            return admin;
        return Users.FirstOrDefault(u => u.Id == id);
    }

    public int Add(User newUser)
    {
        if (Users.Count == 0)
            newUser.Id = 1;
        else
            newUser.Id = Users.Max(p => p.Id) + 1;
        Users.Add(newUser);
        saveToFile(fileUsersName);
        return newUser.Id;
    }

    public bool Update(int id, User newUser)
    {
        if (id != newUser.Id)
            return false;

        var existingUser = GetById(id);
        if (existingUser == null)
            return false;
        if (IsAdmin(existingUser))
        {
            admin = newUser;
            saveToFile(fileAdminName);
        }
        else
        {
            var index = Users.IndexOf(existingUser);
            if (index == -1)
                return false;

            Users[index] = newUser;
            saveToFile(fileUsersName);
        }
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
        saveToFile(fileUsersName);
        return true;
    }

    public bool IsAdmin(User user)
    {
        if (user.Name == admin.Name && user.Password == admin.Password)
            return true;
        return false;
    }

    public int GetAdminId()
    {
        return admin.Id;
    }
}
public static class UsersListUtil
{
    public static void AddUser(this IServiceCollection services)
    {
        services.AddSingleton<IUsersService, UsersService>();
    }
}
