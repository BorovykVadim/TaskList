using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TaskList.Core.Interfaces.Services;
using TaskList.Models;

namespace TaskList.Controllers
{
    [Route("api/tasks")]
    public class TaskController : Controller
    {
        private readonly ITaskService taskService;
        private readonly IMapper mapper;

        public TaskController(ITaskService taskService, IMapper mapper)
        {
            this.taskService = taskService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateTask([FromBody] TaskViewModel model)
        {
            int.TryParse(this.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value, out int userId);

            var task = mapper.Map<TaskViewModel>(this.taskService.CreateTasks(model.Name, userId));

            this.StartProcessing(task.Id);

            return this.Ok(task);
        }

        [HttpGet]
        public IActionResult GetTasks(int pageSize, int pageNumber)
        {
            var tasks = mapper.Map<IEnumerable<TaskViewModel>>(this.taskService.GetTasks(pageSize, pageNumber));
            var count = this.taskService.GetTasksCount();

            var result = new TasksAndCountViewModel { Tasks = tasks, TasksCount = count };

            return this.Ok(result);
        }

        private void StartProcessing(int taskId)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    channel.QueueDeclare(queue: "task_queue",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var body = Encoding.UTF8.GetBytes(taskId.ToString());

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "",
                                         routingKey: "task_queue",
                                         basicProperties: properties,
                                         body: body);

                    Console.WriteLine(" [x] Sent {0}", taskId);
                }


                Console.WriteLine(" Press [enter] to exit.");
            }
        }
    }
}
