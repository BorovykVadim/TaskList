using AutoMapper;
using TaskList.Core.Models;
using TaskList.Models;

namespace TaskList.Mappers
{
    public class MappingTasks : Profile
    {
        public MappingTasks()
        {
            CreateMap<Task, TaskViewModel>().ForMember("AuthorName", config => config.MapFrom(c => c.Author.UserName));
        }
    }
}
