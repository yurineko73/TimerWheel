using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

#region 第一版（不可用）
// public class TimingWheelManager : MonoBehaviour
// {
//     private static TimingWheel s_TimingWheel = null;

//     public static void Initialize(float tickInterval, int wheelSize)
//     {
//         s_TimingWheel = new TimingWheel(tickInterval, wheelSize);
//     }


//     // private void Awake()
//     // {
//     //     _timingWheel = new TimingWheel(1.0f, 60); // 1秒间隔，60个桶
//     // }

//     // private void Start()
//     // {
//     //     var task = AddTimer(
//     //         (param1, param2) =>
//     //         {
//     //             Debug.Log("执行定时器");
//     //         },
//     //         null,
//     //         null,
//     //         1,
//     //         1,
//     //         1
//     //     );

//     //     task = ModifyTimer(
//     //         task,
//     //         3,
//     //         5,
//     //         10
//     //     );
//     //     RemoveTimer(task);
//     // }

//     private void Update()
//     {
//         if (s_TimingWheel == null) return;
//         s_TimingWheel.Update();
//     }

//     // 其他方法，例如添加、删除、修改定时器等
//     public static TimerTask AddTimer(Action<object, object> callback, object param1, object param2, float delay, float interval, int executeCount)
//     {
//         TimerTask task = new TimerTask
//         {
//             Callback = callback,
//             Param1 = param1,
//             Param2 = param2,
//             Delay = delay,
//             Interval = interval,
//             ExecuteCount = executeCount
//         };
//         return s_TimingWheel.AddTask(task);
//     }

//     public static void RemoveTimer(TimerTask task)
//     {
//         s_TimingWheel.RemoveTask(task);
//     }

//     public static TimerTask ModifyTimer(TimerTask task, float? delay = null, float? interval = null, int? executeCount = null)
//     {
//         return s_TimingWheel.ModifyTask(task, delay, interval, executeCount);
//     }
// }

#endregion


public class TimingWheelManager : MonoBehaviour
{
    private static TimingWheel s_TimingWheel = null;

    // public static void Initialize()
    // {
    //     s_TimingWheel = new TimingWheel();
    //     s_TimingWheel.Initialize();
    // }
    void Awake()
    {
        s_TimingWheel = new TimingWheel();
        s_TimingWheel.Initialize();
    }
   
    void Start()
    {
        // UnityEngine.Debug.Log("TimingWheelManager Start");
        // RemoveTimer(taskId);
        
    }
    void Update()
    {
        if (s_TimingWheel == null) 
        {
            return;
        }
        s_TimingWheel.Update();
    }
    public static int AddTimer(Action<object, object> callback, float delay, float interval = 0f, int times = 1, object userData1 = null, object userData2 = null)
    {
        var taskId = s_TimingWheel.AddTimer(callback, delay, interval, times, userData1, userData2);
        return taskId;
    }

    public static void RemoveTimer(int taskId)
    {
        s_TimingWheel.RemoveTimer(taskId);
    }

    public static int ModifyTimer(int id, float? delay, float? interval, int times = 0, Action<object, object> callback = null, object userData1 = null, object userData2 = null)
    {
        return s_TimingWheel.ModifyTimer(id, delay, interval, times, callback, userData1, userData2);
    }

#region 测试代码
    private int taskId = 0;
    public void OnButtonAdd100Task()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < 1000000; i++)
        {
            taskId = AddTimer
            (
                (param1, param2) =>
                {

                },
                UnityEngine.Random.Range(0, 100000000),
                0,
                1,
                null,
                null
            );
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log("[TimingWheelManager.OnButtonAdd100Task] " + $"Added 100,000 timers in {stopwatch.ElapsedMilliseconds} ms");
    }
    // 测试执行10万个定时器的性能
    public void OnButtonTestExecuteTimers()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < 100000; i++)
        {
            taskId = AddTimer
            (
                (param1, param2) =>
                {
                    
                },
                1,
                0,
                1,
                null,
                null
            );
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log("[TimingWheelManager.OnButtonTestExecuteTimers] " + $"Added 100,00 timers in {stopwatch.ElapsedMilliseconds} ms");
    }
    public void OnButtonAddTask()
    {
        taskId = AddTimer
        (
            (param1, param2) =>
            {
                UnityEngine.Debug.Log("[TimingWheelManager.OnButtonAddTask]执行定时器 id = " + taskId);
            },
            10,
            1.0f,
            5,
            null
        );
    }
    public void OnButtonAddRemoveTask()
    {
        AddTimer
        (
            (param1, param2) =>
            {
                
            },
            1,
            0,
            1,
            null,
            null
        );
        AddTimer
        (
            (param1, param2) =>
            {
                RemoveTimer(4);
                RemoveTimer(5);
                RemoveTimer(6);
                RemoveTimer(7);
            },
            1
        );
        for (int i = 0; i < 10; i++)
        {
            AddTimer
            (
                (param1, param2) =>
                {
                    
                },
                1,
                0,
                1,
                null,
                null
            );
        }        
    }

    public void OnButtonRemoveTask()
    {
        RemoveTimer(taskId);
    }

    public void OnButtonModifyTask()
    {
        taskId = ModifyTimer
        (
            taskId,
            5,
            2,
            2,
            (param1, param2) =>
            {
                UnityEngine.Debug.Log("[TimingWheelManager.OnButtonModifyTask]执行定时器ModifyTimer id = " + taskId);
            },
            null
        );
    }
#endregion
}

