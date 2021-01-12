using System.Collections.Generic;
using TaskList.Core.Models;

namespace TaskList.Core.Interfaces.Services
{
    public interface ITaskService
    {
        Task CreateTasks(string taskName, int userId);

        IEnumerable<Task> GetTasks(int pageSize, int pageNumber);

        int GetTasksCount();

        Task UpdateTaskStatus(int id);
    }
}
