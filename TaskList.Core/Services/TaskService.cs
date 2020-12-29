using System;
using System.Collections.Generic;
using TaskList.Core.Interfaces.Repositories;
using TaskList.Core.Interfaces.Services;
using TaskList.Core.Models;

namespace TaskList.Core.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository repository;

        public TaskService(IRepository repository)
        {
            this.repository = repository;
        }

        public Task CreateTasks(string taskName, int userId)
        {
            var task = new Task
            {
                AuthorId = userId,
                DateOfCreation = DateTime.Now,
                Name = taskName,
                IsDone = false
            };

            var res = this.repository.Add(task);
            this.repository.Save();

            var includedTask = this.repository.Include(res, nameof(Task.Author));
            return includedTask;
        }

        public IEnumerable<Task> GetTasks(int pageSize, int pageNumber)
        {
            return this.repository.GetPagedListIncude<Task>(pageSize, pageNumber, nameof(Task.Author));
        }

        public int GetTasksCount()
        {
            return this.repository.Count<Task>();
        }

        public void UpdateTaskStatus(int id, int time)
        {
            var task = this.repository.GetById<Task>(id);

            task.IsDone = true;
            task.Time = time;

            this.repository.Save();
        }
    }
}
