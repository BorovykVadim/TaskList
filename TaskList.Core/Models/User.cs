using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskList.Core.Models
{
    public class User : IdentityUser<int>
    {
        public IEnumerable<Task> Tasks { get; set; }
    }
}
