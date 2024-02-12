using myTodoList.Models;
using System.Collections.Generic;

namespace myTodoList.Interface
{
    public interface IUsersService
    {
        List<User> GetAll();
        User GetById(int id);
        int Add(User User);
        bool Delete(int id);
        bool IsAdmin(User User);
        bool Update(int x,User pizza);
        // int Count {get;}
    }
}

