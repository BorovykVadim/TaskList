using System.Collections.Generic;

namespace TaskList.Models
{
    public class TasksAndCountViewModel
    {
        public IEnumerable<TaskViewModel> Tasks { get; set; }

        public int TasksCount { get; set; }
    }
}
