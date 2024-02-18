using Task = myTodoList.Models.Task;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace myTodoList.Interface
{
    public interface ITasksListService
    {
        List<Task> GetAll(int id);
        Task GetById(int id);
        int Add(Task task);
        bool Delete(int id);
        bool Update(int id,Task task);
        void DeleteUserItems(int userId);
        // int Count {get;}
    }
}

