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
    private List<User> users;
    private User admin;
    private string fileUsersName = "User.json";
    private string fileAdminName = "Admin.json";

    public UsersService()
    {
        this.fileUsersName = Path.Combine(/*webHost.ContentRootPath,*/ "Data", "User.json");

        using (var jsonFile = File.OpenText(fileUsersName))
        {
            users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
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

    private void saveToFile()
    {
        File.WriteAllText(fileUsersName, JsonSerializer.Serialize(users));
    }

    public List<User> GetAll() => users;

    public User GetById(int id)
    {
        return users.FirstOrDefault(u => u.Id == id);
    }

    public int Add(User newUser)
    {
        if (users.Count == 0)

        {
            newUser.Id = 1;
        }
        else
        {
            newUser.Id = users.Max(p => p.Id) + 1;

        }

        users.Add(newUser);
        saveToFile();
        return newUser.Id;
    }

    public bool Update(int id, User newUser)
    {
        if (id != newUser.Id)
            return false;

        var existingUser = GetById(id);
        if (existingUser == null)
            return false;

        var index = users.IndexOf(existingUser);
        if (index == -1)
            return false;

        users[index] = newUser;
        saveToFile();
        return true;
    }


    public bool Delete(int id)
    {
        var existingUser = GetById(id);
        if (existingUser == null)
            return false;

        var index = users.IndexOf(existingUser);
        if (index == -1)
            return false;

        users.RemoveAt(index);
        saveToFile();
        return true;
    }
    public bool IsAdmin(User user)
    {
        if (user.Id == admin.Id && user.Name == admin.Name && user.Password == admin.Password)
            return true;
        else
            return false;
    }


}

public static class usersListUtil
{
    public static void AddUser(this IServiceCollection services)
    {
        services.AddSingleton<IUsersService, UsersService>();
    }
}
