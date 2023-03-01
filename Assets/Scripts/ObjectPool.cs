using System.Collections.Generic;

public class ObjectPool<T>
{
    private readonly Queue<T> pool;
    private readonly System.Func<T> objectGenerator;

    public ObjectPool(System.Func<T> objectGenerator, int initialPoolSize = 0)
    {
        this.objectGenerator = objectGenerator;
        pool = new Queue<T>(initialPoolSize);

        for (int i = 0; i < initialPoolSize; i++)
        {
            pool.Enqueue(objectGenerator());
        }
    }

    public T GetNextObject()
    {
        if (pool.Count == 0)
        {
            pool.Enqueue(objectGenerator());
        }

        return pool.Dequeue();
    }

    public void ReturnObjectToPool(T obj)
    {
        pool.Enqueue(obj);
    }
}