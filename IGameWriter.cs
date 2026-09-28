using System;

public interface IGameWriter<T>
{
    void Add(T item);
    void AddMultiple(List<T> items);
    void Remove(T item);
}
