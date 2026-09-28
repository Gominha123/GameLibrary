using System;

public interface IGameReader<T>
{
    List<T> GetAll();
}
