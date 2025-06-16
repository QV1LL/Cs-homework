using LowpriceProductsApp.Domain.Entities;
using System;
using System.Collections.Generic;

namespace LowpriceProductsApp.Domain.Repositories;

public interface IRepository<T> where T : IEntity
{
    T? Get(Guid id);
    IEnumerable<T> GetAll();
    T? Find(Predicate<T> predicate);
    void Remove(Guid id);
    void Remove(T entity);
    void Add(T entity);
}
