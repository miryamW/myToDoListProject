using System.Collections.Generic;
using Task =myToDoList.Models.Task;


namespace myToDoList.Interfaces
{
    public  interface ITasksListService
    {
       List<Task> GetAll();
     
       Task GetById(int id);
     
       int Add(Task newTask);

       bool Update(int id, Task newTask);
   
       bool Delete(int id);
      
    }
}