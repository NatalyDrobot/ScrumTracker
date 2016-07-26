using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataLayer.Interfaces
{
    public interface IRepository<TEntity, TDto> 
        where TEntity : class 
        where TDto : class
    {
        void Add(TDto obj);
        void Update(IEnumerable<TDto> items);
        void Remove(TDto obj);
        void Remove(IEnumerable<TDto> objs);
        void Update(TDto obj);
        IEnumerable<TDto> GetList();
        IEnumerable<TDto> GetWithFilter(Expression<Func<TEntity, bool>> expresion);
    }
}

