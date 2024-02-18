using myTodoList.Models;
using System.Collections.Generic;

namespace myTodoList.Interface
{
    public interface IUsersService
    {
        List<User> GetAll();
        User GetById(int id);
        int Add(User user);
        bool Delete(int id);
        bool IsAdmin(User user);
        int GetAdminId();
        bool Update(int id,User user);
        User GetUser(User user);
        
        // int Count {get;}
    }
}

