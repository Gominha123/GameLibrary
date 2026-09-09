using System;

public class GameRepository<T>
{
    private List<T> items = new List<T>();

    public void Add(T item)
    {
        // adicionar o jogo
        items.Add(item);
    }

    public List<T> GetAll()
    {
        // devolver os jogos
        return items;
    }

    public void Remove(T item)
    {
        // remover o jogo
        items.Remove(item);
    }
}
