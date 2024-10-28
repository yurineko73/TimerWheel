using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Linq;

#region 第一版（不可用）
// public class TimerTask
// {
//     public Action<object, object> Callback { get; set; }
//     public object Param1 { get; set; }
//     public object Param2 { get; set; }
//     public float Delay { get; set; } // 延迟时间，下一次执行前需要等待的时间
//     public float Interval { get; set; } // 执行间隔
//     public int ExecuteCount { get; set; } // 执行次数 -1表示无限
//     public int CurrentCount { get; private set; }
//     public bool IsCanceled { get; set; }

//     public TimerTask()
//     {
//         CurrentCount = 0;
//         IsCanceled = false;
//     }

//     public void Execute()
//     {
//         if (!IsCanceled)
//         {
//             Callback?.Invoke(Param1, Param2);
//             CurrentCount++;
//             if (CurrentCount >= ExecuteCount && ExecuteCount != -1)
//             {
//                 IsCanceled = true;
//             }
//         }
//     }
// }

// public class TimingWheel
// {
//     private float m_TickInterval;
//     private List<List<TimerTask>> m_WheeList;
//     private float m_CurrentTime;

//     public TimingWheel(float tickInterval, int wheelSize)
//     {
//         m_TickInterval = tickInterval;
//         m_CurrentTime = 0f;
//         m_WheeList = new List<List<TimerTask>>(wheelSize);
//         for (int i = 0; i < wheelSize; i++)
//         {
//             m_WheeList.Add(new List<TimerTask>());
//         }
//     }

//     public TimerTask AddTask(TimerTask task)
//     {
//         int bucketIndex = CalculateBucketIndex(task.Delay);
//         m_WheeList[bucketIndex].Add(task);
//         return task;
//     }

//     public void RemoveTask(TimerTask task)
//     {
//         task.IsCanceled = true;
//     }

//     public TimerTask ModifyTask(TimerTask task, float? delay = null, float? interval = null, int? executeCount = null)
//     {
//         RemoveTask(task);
//         TimerTask newtask = new TimerTask
//         {
//             Callback = task.Callback,
//             Param1 = task.Param1,
//             Param2 = task.Param2,
//             Delay = delay.HasValue? delay.Value : task.Delay,
//             Interval = interval.HasValue? interval.Value : task.Interval,
//             ExecuteCount = executeCount.HasValue? executeCount.Value : task.ExecuteCount
//         };
//         return AddTask(newtask);
//     }

//     private int CalculateBucketIndex(float delay)
//     {
//         return (int)(delay / m_TickInterval) % m_WheeList.Count;
//     }

//     public void Update()
//     {
//         // 更新当前时间
//         m_CurrentTime += Time.deltaTime;

//         // 获取当前时间轮的位置
//         int currentTickIndex = (int)(m_CurrentTime / m_TickInterval) % m_WheeList.Count;

//         // 处理当前时间轮位置的桶中的任务
//         List<TimerTask> currentBucket = m_WheeList[currentTickIndex];
//         for (int i = currentBucket.Count - 1; i >= 0; i--)
//         {
//             TimerTask task = currentBucket[i];
//             if (!task.IsCanceled && m_CurrentTime >= task.Delay)
//             {
//                 task.Execute();
//                 if (!task.IsCanceled && (task.ExecuteCount == -1 || task.CurrentCount < task.ExecuteCount))
//                 {
//                     // 更新下一次执行的时间
//                     task.Delay = 0;
//                     // 计算下一个桶的索引
//                     int nextTickIndex = CalculateBucketIndex(m_CurrentTime + task.Interval);
//                     m_WheeList[nextTickIndex].Add(task);
//                 }
//                 currentBucket.Remove(task);
//             }
//         }
//     }
// }

#endregion

#region 第二版（不可用）
// public class TimerTask
// {
//     public int id;

//     public int delay;    //单位毫秒

//     public int interval; //单位毫秒

//     public int loopTimes;

//     public Action<object, object> Callback { get; set; }
//     public object Param1 { get; set; }
//     public object Param2 { get; set; }

//     public DateTime dateTime;

//     public TimerTask(int id, Action<object, object> callback, object param1, object param2, int delay, int interval, int loopTimes)
//     {
//         this.id = id;
//         this.interval = interval;
//         this.Callback = callback;
//         this.Param1 = param1;
//         this.Param2 = param2;
//         this.loopTimes = loopTimes;
//         this.delay = delay;
//         dateTime = DateTime.Now.AddMilliseconds(interval + delay);
//     }

//     public void Run()
//     {
//         Callback?.Invoke(Param1, Param2);
//     }

//     public bool CheckLoop()
//     {
//         if (loopTimes < 0)
//         {
//             dateTime = DateTime.Now.AddMilliseconds(interval);
//             return true;
//         }
//         else
//         {
//             loopTimes--;
//             dateTime = DateTime.Now.AddMilliseconds(interval);
//             return loopTimes > 0;
//         }
//     }
// }

// public class TimingWheel
// {
//     private Dictionary<int, List<TimerTask>> m_MonthMap = new Dictionary<int, List<TimerTask>>();
//     private Dictionary<int, List<TimerTask>> m_DayMap = new Dictionary<int, List<TimerTask>>();
//     private Dictionary<int, List<TimerTask>> m_HourMap = new Dictionary<int, List<TimerTask>>();
//     private Dictionary<int, List<TimerTask>> m_MinuteMap = new Dictionary<int, List<TimerTask>>();
//     private Dictionary<int, List<TimerTask>> m_SecondMap = new Dictionary<int, List<TimerTask>>();

//     private Dictionary<int, List<TimerTask>> m_MillisecondMap = new Dictionary<int, List<TimerTask>>();

//     private int m_Id = 0;

//     public TimingWheel()
//     {
//         // Debug.Log(DateTime.Now);
//         for (int i = 0; i < 13; i++)
//         {
//             m_MonthMap.Add(i, new List<TimerTask>());
//         }

//         for (int i = 0; i < 31; i++)
//         {
//             m_DayMap.Add(i, new List<TimerTask>());
//         }

//         for (int i = 0; i < 24; i++)
//         {
//             m_HourMap.Add(i, new List<TimerTask>());
//         }

//         for (int i = 0; i < 60; i++)
//         {
//             m_MinuteMap.Add(i, new List<TimerTask>());
//         }

//         for (int i = 0; i < 60; i++)
//         {
//             m_SecondMap.Add(i, new List<TimerTask>());
//         }

//         //毫秒级粒度为50ms
//         for (int i = 0; i < 20; i++)
//         {
//             m_MillisecondMap.Add(i, new List<TimerTask>());
//         }
//     }

//     public void Update()
//     {
//         DateTime now = DateTime.Now;

//         List<TimerTask> monthList = m_MonthMap[now.Month];
//         List<TimerTask> dayList = m_DayMap[now.Day];
//         List<TimerTask> hourList = m_HourMap[now.Hour];
//         List<TimerTask> minuteList = m_MinuteMap[now.Minute];
//         List<TimerTask> secondList = m_SecondMap[now.Second];

//         int milliSecondDelta = now.Millisecond / 50;
//         List<TimerTask> millisecondList = m_MillisecondMap[milliSecondDelta];

//         for (int i = monthList.Count - 1; i >= 0; i--)
//         {
//             TimerTask task = monthList[i];            
//             //添加到日轮
//             m_DayMap[task.dateTime.Day].Add(task);
//         }
//         monthList.Clear();

//         for (int i = dayList.Count - 1; i >= 0; i--)
//         {
//             TimerTask task = dayList[i];
//             //添加到小时轮
//             m_HourMap[task.dateTime.Hour].Add(task);
//         }
//         dayList.Clear();

//         for (int i = hourList.Count - 1; i >= 0; i--)
//         {
//             TimerTask task = hourList[i];
//             //添加到分轮
//             m_MinuteMap[task.dateTime.Minute].Add(task);
//         }
//         hourList.Clear();

//         for (int i = minuteList.Count - 1; i >= 0; i--)
//         {
//             TimerTask task = minuteList[i];
//             //添加到秒轮
//             m_SecondMap[task.dateTime.Second].Add(task);
//         }
//         minuteList.Clear();

//         for (int i = secondList.Count - 1; i >= 0; i--)
//         {
//             TimerTask task = secondList[i];
//             //添加到毫秒轮
//             m_MillisecondMap[task.dateTime.Millisecond / 50].Add(task);
//         }
//         secondList.Clear();

//         for (int i = millisecondList.Count - 1; i >= 0; i--)
//         {
//             TimerTask task = millisecondList[i];
//             if (task != null && DateTime.Now >= task.dateTime)
//             {
//                 millisecondList.Remove(task);

//                 task.Run();

//                 if (task.CheckLoop())
//                 {
//                     AddTask(task);
//                 }
//             }
//         }
//     }
//     public TimerTask SetInterval(Action<object, object> callback, object param1, object param2, int delay, int interval, int loopTimes)
//     {
//         TimerTask task = new TimerTask(GetId(), callback, param1, param2, delay, interval, loopTimes);
//         AddTask(task);
//         return task;
//     }
//     public TimerTask ModifyTask(TimerTask task, Action<object, object>? callback, object? param1, object? param2, int? delay,
//         int? interval, int? loopTimes)
//     {
//         ClearInterval(task.id);
//         TimerTask newtask = new TimerTask
//         (
//             task.id,
//             callback ?? task.Callback,
//             param1 ?? task.Param1,
//             param2 ?? task.Param2,
//             delay ?? task.delay,
//             interval ?? task.interval,
//             loopTimes ?? task.loopTimes
//         );
//         AddTask(newtask);
//         return newtask;
//     }

//     public void ClearInterval(int id)
//     {
//         RemoveTask(m_MonthMap, id);
//         RemoveTask(m_DayMap, id);
//         RemoveTask(m_HourMap, id);
//         RemoveTask(m_MinuteMap, id);
//         RemoveTask(m_SecondMap, id);
//         RemoveTask(m_MillisecondMap, id);
//     }

//     private void AddTask(TimerTask task)
//     {
//         m_MonthMap[task.dateTime.Month].Add(task);
//     }

//     private bool RemoveTask(Dictionary<int, List<TimerTask>> wheel, int id)
//     {
//         foreach (var item in wheel)
//         {
//             List<TimerTask> tasks = item.Value;
//             foreach (var task in tasks)
//             {
//                 if (task.id == id)
//                 {
//                     tasks.Remove(task);
//                     return true;
//                 }
//             }
//         }

//         return false;
//     }

//     private int GetId()
//     {
//         return m_Id++;
//     }
// }
#endregion

/// <summary>
// 1. 不必要的变量优化 11
// 2. 日志格式   11
// 3.添加定时器回调借口参数定义 11
// 4.用对象池优化添加和删除 11
// 5.m_TmpLinkList 优化 11
// 6.在定时器内实时添加另一个定时器的情况对upadate进行优化 用m_CurTargetTick进行处理了11
// 7. 定时器参数的检查，间隔小于10ms的检查 11
// 8. 统一清理 不做，因为如果在update中删除任务，会有影响 11
// 9.  m_AllTimerMap字典的优化 RemoveTimer 字典判断删除优化为一起 11
// 10. 定时器的移除，考虑立即和延后的情况 立即删除会消耗很大，是否需要做？11
// 11. 定时器移除做reset操作，对应lua方面  11
// 12. 定时器移除在upadate中移除当前任务的下一个任务或几个任务的情况测试 11
// 13.CalcCurTick方法使用情况优化  11
// 14.定时器的初始化不要对外暴露接口Initialize 11
// 15.总结前两个：
// 	第一版的问题是只用了一个轮，每次检查都要检查一个索引下所有的元素，包括不需要触发的；用了list记录，添加是o1或on，删除是on
// 	第二版的问题是 1.基于系统时间，如果修改时间可能会导致时间轮出问题；2.使用list保持任务对象，添加删除的效率低；
// 		3.从大轮往小轮遍历，且元素添加在大轮中，多做了很多高阶向低阶迁移元素的操作。
/// </summary>
public class TimerTask
{
    private Action<object, object> m_Callback;    // 单个定时器任务的回调方法
    public Action<object, object> Callback { get { return m_Callback; } }
    private object m_UserData1;            // 用户自定义回调数据1
    public object UserData1 { get { return m_UserData1; } }
    private object m_UserData2;            // 用户自定义回调数据2
    public object UserData2 { get { return m_UserData2; } }

    // 定时器唯一ID
    public int ID { get; private set; }
    // 定时器的到期tick
    public long Expire { get; private set; }
    // 当前所处槽位下标
    public int SoltIndex { get; set; }
    public bool IsValid { get; set; }
    private long m_Interval;                   // 执行间隔延迟tick
    public long Interval { get { return m_Interval; } }
    private int m_Times;                    // 执行次数
    public int Times { get { return m_Times; } }
    private long m_StartTick;                // 执行开始tick
    public long StartTick { get { return m_StartTick; } }
    public long DelayTick { get ; set; }

    /*
        param nowTick：定时器当前tick
        param delayTick：延迟tick
        param times：执行次数，-1则是循环执行，0默认为执行1次，>1则该定时器执行n次
        param callback：执行回调
        param userData：用户自定义数据数组
    */
    public void Init(int id, Action<object, object> callback, long nowTick, long delayTick = 0, 
        long intervalTick = 0, int times = 0, object userData1 = null, object userData2 = null)
    {
        ID = id;
        Expire = nowTick + delayTick;
        DelayTick = delayTick;
        m_StartTick = nowTick;
        m_Interval = intervalTick;
        m_Times = times;

        m_Callback = callback;
        m_UserData1 = userData1;
        m_UserData2 = userData2;

        IsValid = true;
    }

    // 更新到下一次过期tick
    public void UpdateNextExpire(long nowTick)
    {
        Expire = nowTick + m_Interval;
    }

    // 定时器过时后，执行定时器任务
    public bool Invoke()
    {
        m_Callback?.Invoke(m_UserData1, m_UserData2);
        if (m_Times > 0) 
        {
            m_Times--;
        }
        return CheckInvokeTimes();
    }

    private bool CheckInvokeTimes()
    {
        return m_Times == -1 || m_Times > 0;
    }

    public void Reset()
    {
        SoltIndex = -1;
        Expire = 0;
        m_Callback = null;
        m_UserData1 = null;
        m_UserData2 = null;
        DelayTick = 0;
        m_StartTick = 0;
        m_Times = 1;
        m_Interval = 0;
        IsValid = false;
    }
}

public class TimingWheel
{
    private const int k_TV0Bits = 8;                     // tv0：0-255,256个槽位
    private const int k_TVNBits = 6;                     // tv1-tv4层：0-63,64个槽位
    private const int k_TV0Size = 256;        // tv0，槽位个数
    private const int k_TVNSize = 64;        // t1-t4层，槽位个数

    // 位操作掩码，用于计算指定expireTime属于哪个槽位下标
    private const int k_TV0Mask = 255;
    private const int k_TVNMask = 63;
    
    private const int k_TVLevel = 5;                    // 层级轮层级个数

    private const int k_MillisecondPerTick = 10;                 // 10ms执行一次tick
    private const float k_SecondPerTick = k_MillisecondPerTick / 1000f;

    // 用一整个数组存储所有时间槽位，计算时再在把内部划分成5层（连续内存）
    private List<LinkedList<TimerTask>> m_TimeSoltsList = new List<LinkedList<TimerTask>>(k_TV0Size + (k_TVLevel - 1) * k_TVNSize);
    private Dictionary<int, TimerTask> m_AllTimerMap = new Dictionary<int, TimerTask>(1000000);
    // 定时器任务对象池
    private ObjectPool m_TaskInstancePool = new ObjectPool();

    private int m_IDSeed = 0;                           // 用于生成自增随机ID
    
    // 定时器初始化时刻tick
    private long m_StartTick = 0;
    // 当前走到的tick，此前的定时器全部执行完毕
    private long m_CurrentTick;
    // 当前需要执行到的tick，根据系统时间计算一次Update需要走到的tick
    private long m_CurrentTargetTick;

    // 计算当前应该走到的tick位置
    private long CalculateCurTick()
    {
        return (long)(Time.unscaledTime / k_SecondPerTick) - m_StartTick;
    }

    // 毫秒转为tick
    private long Millisecond2Tick(long millisecond)
    {
        return millisecond / k_MillisecondPerTick;
    }
    // tick转为毫秒
    private long Tick2Millisecond(long tick)
    {
        return tick * k_MillisecondPerTick;
    }

    /*
        对高阶轮某个槽位，对低阶轮进行的展开操作

        param tvOffset：相对于展开目标轮的偏移。如：t1轮则偏移为t0轮的长度，256
        param soltIdx：当前需要展开的槽位下标
    */
    private int TimerCascade(int tvOffset, int soltIndex)
    {
        var index = tvOffset + soltIndex;
        var tasks = m_TimeSoltsList[index];
        if (tasks.Count > 0)
        {
            while(tasks.Count > 0)
            {
                var task = tasks.First;
                tasks.Remove(task);
                InternalAddTimer(task.Value);
            }
        }

        return soltIndex;
    }

    /*
        计算过期时间当前处于的下标

        param expireTime：当前需要计算下标的过期时间
        param n：该轮所处于的层级
    */
    private int CalculateSoltIndex(long expireTime, int n)
    {
        return (int)(expireTime >> (k_TV0Bits + (n * k_TVNBits)) & k_TVNMask);
    }

    // 计算某个过期时间在第一个轮中当前处于的下标
    private int CalculateTV0Index(long expireTime)
    {
        return (int)(expireTime & k_TV0Mask);
    }

    // 新增一个定时器任务到时间轮中，外部新增定时器时调用该方法
    private void InternalAddTimer(TimerTask timer)
    {
        var expireTime = timer.Expire;            // 定时器超时时间差
        var span = expireTime - m_CurrentTargetTick;      // 此处需要用目标tick算，避免间隔帧内执行异常
        var soltIndex = -1;
        if (span < 0)
        {
            // 如果时间轮间隔太短（小于10ms），可以下一帧执行 防止时间轮间隔太短导致update死循环
            soltIndex = CalculateTV0Index(m_CurrentTargetTick);
        }
        else if (span < k_TV0Size)
        {
            soltIndex = CalculateTV0Index(expireTime);
        }
        else if (span < 1 << (k_TV0Bits + k_TVNBits))
        {
            soltIndex = k_TV0Size + CalculateSoltIndex(expireTime, 0);
        }
        else if (span < 1 << (k_TV0Bits + 2 * k_TVNBits))
        {
            soltIndex = k_TV0Size + k_TVNSize + CalculateSoltIndex(expireTime, 1);
        }
        else if (span < 1 << (k_TV0Bits + 3 * k_TVNBits))
        {
            soltIndex = k_TV0Size + 2 * k_TVNSize + CalculateSoltIndex(expireTime, 2);
        }
        else
        {
            // 定时器过期时间超过该时间轮的最大限制，就直接放到最后
            if (span > 0xffffffffL)
            {
                expireTime = m_CurrentTargetTick + 0xffffffffL;
            }

            soltIndex = k_TV0Size + 3 * k_TVNSize + CalculateSoltIndex(expireTime, 3);
        }

        timer.SoltIndex = soltIndex;
        m_TimeSoltsList[soltIndex].AddLast(timer);
    }

    #region 外部调度接口

    // 初始化定时器
    public void Initialize()
    {
        int count = k_TV0Size + (k_TVLevel - 1) * k_TVNSize;
        for (int i = 0; i < count; i++)
        {
            m_TimeSoltsList.Add(new LinkedList<TimerTask>());
        }
        m_StartTick = m_CurrentTargetTick = m_CurrentTick = CalculateCurTick();
    }

    // 更新时间轮进度，由外部推动
    public void Update()
    {
        m_CurrentTargetTick = CalculateCurTick();
        while (m_CurrentTick < m_CurrentTargetTick)
        {
            var index = CalculateTV0Index(m_CurrentTick);
            if (index == 0
                && TimerCascade(k_TV0Size, CalculateSoltIndex(m_CurrentTick, 0)) == 0
                && TimerCascade(k_TV0Size + k_TVNSize, CalculateSoltIndex(m_CurrentTick, 1)) == 0
                && TimerCascade(k_TV0Size + 2 * k_TVNSize, CalculateSoltIndex(m_CurrentTick, 2)) == 0)
            {
                TimerCascade(k_TV0Size + 3 * k_TVNSize, CalculateSoltIndex(m_CurrentTick, 3));
            }
            
            var invokeTasks = m_TimeSoltsList[index];
            if (invokeTasks.Count > 0)
            {
                while(invokeTasks.Count > 0)
                {
                    var task = invokeTasks.First;
                    invokeTasks.Remove(task);    
                    // 过期的定时器不再执行
                    if (!task.Value.IsValid)
                    {
                        continue;
                    }                

                    bool nextStatus = task.Value.Invoke();        // 执行最低颗粒度轮的过期槽位中的所有定时器任务
                    if (nextStatus)
                    {
                        task.Value.UpdateNextExpire(m_CurrentTargetTick);
                        InternalAddTimer(task.Value);
                    }
                    else
                    {
                        // 清除过期task对象
                        RemoveTimer(task.Value.ID);                        
                    }
                }
            }

            m_CurrentTick++;
        }
    }
    #endregion 

    #region 外部业务接口

    /*
        新增一个定时器

        param interval：时间间隔，单位：秒
        param times：执行次数，-1视为执行无限次
        param callback：任务回调
        param userData：回调用户自定义参数
    */
    public int AddTimer(Action<object, object> callback, float delay, float interval, int times = 1, object userData1 = null, object userData2 = null)
    {
        return AddMillisecondTimer(callback, m_CurrentTargetTick, (long)(delay * 1000), (long)(interval * 1000), times, userData1, userData2);
    }

    /*
        新增一个定时器

        param msInterval：时间间隔，单位：毫秒
        param times：执行次数，-1视为执行无限次
        param callback：任务回调
        param userData：回调用户自定义参数
    */
    public int AddMillisecondTimer(Action<object, object> callback, long nowTick, long delay, long interval, int times = 1, object userData1 = null, object userData2 = null)
    {
        if (interval < 0)
        {
            Debug.LogError("定时器过期时间跨度不能为负数 [TimingWheel.AddMillisecondTimer] interval: " + interval);
            return -1;
        }            

        if (times != -1 && times <= 0)
        {
            Debug.LogError("定时器执行次数参数异常，可选-1或者大于0的整形 [TimingWheel.AddMillisecondTimer] times: " + times);
            return -1;
        }     

        if (callback == null)
        {
            Debug.LogError("定时器回调不能为空 [TimingWheel.AddMillisecondTimer] callback: " + callback);
            return -1;
        }       

        var timerTask =  m_TaskInstancePool.Get();
        timerTask.Init(++m_IDSeed, callback, nowTick, Millisecond2Tick(delay), Millisecond2Tick(interval), times, userData1, userData2);
        InternalAddTimer(timerTask);

        m_AllTimerMap[timerTask.ID] = timerTask;

        // Debug.Log("新增定时器, ID：" + timerTask.ID + "，delay：" + delay/1000 + "，interval：" + interval/1000 + "，times：" + times);
        return timerTask.ID;
    }

    /*
        删除某个定时器
    */
    public bool RemoveTimer(int taskId)
    {
        TimerTask timerTask;
        if (!m_AllTimerMap.TryGetValue(taskId, out timerTask))
        {
            // Debug.LogError("定时器对象不存在");
            return false;
        }  
        // 不在当前帧，直接删除（因为这里删除消耗很大）
        if (timerTask.Expire >= m_CurrentTargetTick) 
        {
            m_TimeSoltsList[timerTask.SoltIndex].Remove(timerTask);
        }
        
        timerTask.Reset();
        m_TaskInstancePool.Return(timerTask);
        // Debug.Log("删除定时器, ID：" + taskId);
        return m_AllTimerMap.Remove(taskId);
        // 可以用对象池优化
    }

    /*
        修改定时器的属性

        param times：修改执行次数
        param callback：修改任务回调
        param userData：修改回调用户自定义参数
    */
    public int ModifyTimer(int id, float? delay, float? interval, int times = 0, Action<object, object> callback = null, object userData1 = null, object userData2 = null)
    {
        TimerTask task;
        if (!m_AllTimerMap.TryGetValue(id, out task) || !task.IsValid)
        {
            Debug.LogError("定时器对象不存在或已经失效 [TimingWheel.ModifyTimer] id: " + id);
            return -1;
        }           

        long intervalTick = interval.HasValue ? (long)(interval * 1000) : Tick2Millisecond(task.Interval);
        long delayTick = delay.HasValue ? (long)(delay * 1000) : Tick2Millisecond(task.DelayTick);
        times = times == 0 ? task.Times : times;
        // Debug.Log("修改定时器, ID：" + id + "，delay：" + delay + "，interval：" + interval + "，times：" + times);
        int taskId = AddMillisecondTimer(callback ?? task.Callback, task.StartTick, delayTick, intervalTick, times, userData1 ?? task.UserData1, userData2 ?? task.UserData2);
        RemoveTimer(id);
        return taskId;
    }

    #endregion
}

