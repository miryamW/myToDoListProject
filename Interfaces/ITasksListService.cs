using Task = myTodoList.Models.Task;
using System.Collections.Generic;

namespace myTodoList.Interface
{
    public interface ITasksListService
    {
        List<Task> GetAll();
        Task GetById(int id);
        int Add(Task Task);
        bool Delete(int id);
        bool Update(int x,Task pizza);
        // int Count {get;}
    }
}

