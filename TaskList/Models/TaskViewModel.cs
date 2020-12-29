using System;

namespace TaskList.Models
{
    public class TaskViewModel
    {
        public string Name { get; set; }

        public string AuthorName { get; set; }

        public DateTime DateOfCreation { get; set; }

        public bool IsDone { get; set; }

        public int? Time { get; set; }

        public int Id { get; set; }
    }
}
