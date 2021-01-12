using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        public async Task<IActionResult> CreateTask([FromBody] TaskViewModel model)
        {
            int.TryParse(this.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value, out int userId);

            var task = mapper.Map<TaskViewModel>(this.taskService.CreateTasks(model.Name, userId));

            await Task.Run(() => taskService.UpdateTaskStatus(task.Id));

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
    }
}
