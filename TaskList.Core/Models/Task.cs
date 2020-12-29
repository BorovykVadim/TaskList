using System;

namespace TaskList.Core.Models
{
    public class Task : Entity
    { 
        public string Name { get; set; }

        public User Author { get; set; }

        public int AuthorId { get; set; }

        public DateTime DateOfCreation { get; set; }

        public bool IsDone { get; set; }

        public int? Time { get; set; }
    }
}
