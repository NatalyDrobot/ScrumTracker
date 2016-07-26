using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using DataLayer.Interfaces;

namespace DataLayer.Repositories
{
    public sealed class Repository<TEntity, TDto> : IRepository<TEntity, TDto> where TEntity : class where TDto : class
    {
        public IEnumerable<TDto> GetList()
        {
            using (var context = new StickersContext())
            {
                return MappingEntityToDto(context.Set<TEntity>());
            }
        }

        public void Add(TDto item)
        {
            using (var context = new StickersContext())
            {
                Add(context, item);
                context.SaveChanges();
            }
        }

        private void Add(StickersContext context, TDto item)
        {
            context.Entry(MappingDtoToEntity(item)).State = EntityState.Added;
        }

        public void Add(IEnumerable<TDto> items)
        {
            using (var context = new StickersContext())
            {
                foreach (var item in items)
                {
                    Add(context, item);
                }
                context.SaveChanges();
            }
        }

        public void Update(TDto item)
        {
            using (var context = new StickersContext())
            {
                context.Entry(MappingDtoToEntity(item)).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Update(IEnumerable<TDto> items)
        {
            using (var context = new StickersContext())
            {
                foreach (var item in items)
                {
                    Update(context, item);
                }
                context.SaveChanges();
            }
        }

        private void Update(StickersContext context, TDto item)
        {
            context.Entry(MappingDtoToEntity(item)).State = EntityState.Modified;
        }


        private void Remove(StickersContext context, TDto item)
        {
            context.Entry(MappingDtoToEntity(item)).State = EntityState.Deleted;
        }

        public void Remove(TDto item)
        {
            using (var context = new StickersContext())
            {
                Remove(context, item);
                context.SaveChanges();
            }
        }

        public void Remove(IEnumerable<TDto> items)
        {
            using (var context = new StickersContext())
            {
                foreach (var item in items)
                {
                    Remove(context, item);
                }
                context.SaveChanges();
            }
        }

        public IEnumerable<TDto> GetWithFilter(Expression<Func<TEntity, bool>> expresion)
        {
            using (var context = new StickersContext())
            {
                var entity = context.Set<TEntity>().Where(expresion).Cast<TEntity>().ToArray<TEntity>();
                return MappingEntityToDto(entity);
            }
        }

        private TEntity MappingDtoToEntity(TDto from)
        {
           return Mapper.Map<TDto, TEntity>(from);
        }
        private IEnumerable<TEntity> MappingDtoToEntity(IEnumerable<TDto> from)
        {
            return Mapper.Map<IEnumerable<TDto>, IEnumerable<TEntity>>(from);
        }

        private TDto MappingEntityToDto(TEntity from)
        {
            return Mapper.Map<TEntity, TDto>(from);
        }
        private IEnumerable<TDto> MappingEntityToDto(IEnumerable<TEntity> from)
        {
            return Mapper.Map<IEnumerable<TEntity>, IEnumerable<TDto>>(from);
        }
    }
}
