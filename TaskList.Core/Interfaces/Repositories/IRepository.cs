using System.Collections.Generic;
using TaskList.Core.Models;

namespace TaskList.Core.Interfaces.Repositories
{
    public interface IRepository
    {
        T GetById<T>(int id) where T : Entity;

        T Add<T>(T entity) where T : Entity;

        T Include<T>(T entity, string includeProperty) where T : Entity;

        int Count<T>() where T : Entity;

        IEnumerable<T> GetPagedListIncude<T>(int pageSize, int pageNumber, string includeProperty) where T : Entity;

        void Save();
    }
}
