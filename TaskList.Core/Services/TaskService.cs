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
        private readonly IRabbitMQService rabbitMQService;

        public TaskService(IRepository repository, IRabbitMQService rabbitMQService)
        {
            this.repository = repository;
            this.rabbitMQService = rabbitMQService;
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

        public Task UpdateTaskStatus(int id)
        {
            Random random = new Random();
            var time = random.Next(0, 10);

            rabbitMQService.Send(time.ToString()).Wait();
            
            var task = this.repository.GetById<Task>(id);

            task.IsDone = true;
            task.Time = time;

            this.repository.Save();

            return task;
        }
    }
}
