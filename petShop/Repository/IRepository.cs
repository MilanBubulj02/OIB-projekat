using System.Collections.Generic;

public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    void Add(T item);
}