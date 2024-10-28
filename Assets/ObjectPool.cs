/*
    简单对象池
*/

using System.Collections.Generic;

public class ObjectPool
{
    private const int k_MaxCnt = 1000000;
    private readonly Stack<TimerTask> m_Objects;

    public ObjectPool()
    {
        m_Objects = new Stack<TimerTask>(1000000);
    }

    public TimerTask Get()
    {
        if (m_Objects.Count > 0)
        {
            return m_Objects.Pop();
        }

        return new TimerTask();
    }

    public void Return(TimerTask obj)
    {
        if (m_Objects.Count < k_MaxCnt)
            m_Objects.Push(obj);
    }
}
