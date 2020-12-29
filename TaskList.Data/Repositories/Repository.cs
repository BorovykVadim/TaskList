using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.Core.Interfaces.Repositories;
using TaskList.Core.Models;

namespace TaskList.Data.Repositories
{
    public class Repository : IRepository
    {
        private readonly TaskListDbContext context;

        public Repository(TaskListDbContext context)
        {
            this.context = context;
        }
        protected TaskListDbContext Context
        {
            get { return this.context; }
        }

        T IRepository.GetById<T>(int id)
        {
            return this.context.Set<T>().FirstOrDefault(e => e.Id == id);
        }

        T IRepository.Add<T>(T entity)
        {
            this.context.Set<T>().Add(entity);

            return entity;
        }
        T IRepository.Include<T>(T entity, string includeProperty)
        {
            return this.context.Set<T>()
                               .Where(e => e.Id == entity.Id)
                               .Include(includeProperty)
                               .FirstOrDefault();
        }

        int IRepository.Count<T>()
        {
            return this.context.Set<T>().Count();
        }

        IEnumerable<T> IRepository.GetPagedListIncude<T>(int pageSize, int pageNumber, string includeProperty)
        {
            var query = this.Context.Set<T>().Where(e => true).Include(includeProperty); ;
            return this.Page(query, pageSize, pageNumber);
        }

        private IEnumerable<T> Page<T>(IQueryable<T> entities, int pageSize, int pageNumber) where T : Entity
        {
            if (pageNumber >= 0)
            {
                if (pageSize > 0)
                {
                    if (pageNumber == 0)
                    {
                        pageNumber = 1;
                    }

                    if (pageNumber > 0)
                    {
                        return entities
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
                    }
                }

                return entities.ToList();
            }

            int numberOfUsers = entities.Count();

            int virtualFirstIndex = numberOfUsers - pageSize * Math.Abs(pageNumber);
            int numberOfElementsInPage = Math.Min(pageSize, virtualFirstIndex + pageSize);

            return entities
                    .Skip(virtualFirstIndex)
                    .Take(numberOfElementsInPage)
                    .ToList();
        }

        public void Save()
        {
            this.context.SaveChanges();
        }
    }
}
