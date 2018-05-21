using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DifferentMethods.Extensions.Async;

public static class MonoBehaviourExtensionMethods
{
    /// <summary>
    /// Get a component from a game object, and add it if it is missing.
    /// </summary>
    /// <param name="g">
    /// A <see cref="GameObject"/>
    /// </param>
    /// <returns>
    /// A <see cref="T"/>
    /// </returns>
    public static T DefaultComponent<T>(this MonoBehaviour behaviour) where T : Component
    {
        T c = behaviour.GetComponent<T>();
        if (c == null)
            c = behaviour.gameObject.AddComponent<T>();
        return c;
    }

    /// <summary>
    /// Schedules a coroutine to run to be after all tasks in the same queue have run.
    /// </summary>
    /// <param name='behaviour'>
    /// Behaviour.
    /// </param>
    /// <param name='queueName'>
    /// Queue name.
    /// </param>
    /// <param name='task'>
    /// Task.
    /// </param>
    public static void QueueWorkItem(this MonoBehaviour behaviour, string queueName, IEnumerator task)
    {
        var schedule = behaviour.DefaultComponent<WorkScheduler>();
        schedule.ScheduleWorkItem(queueName, task);
    }

    /// <summary>
    /// Schedules a coroutine to run to be after all tasks in the default queue have run.
    /// </summary>
    /// <param name='behaviour'>
    /// Behaviour.
    /// </param>
    /// <param name='task'>
    /// Task.
    /// </param>
    public static void QueueWorkItem(this MonoBehaviour behaviour, IEnumerator task)
    {
        var schedule = behaviour.DefaultComponent<WorkScheduler>();
        schedule.ScheduleWorkItem("_default", task);
    }


    /// <summary>
    /// Schedules a function to be run after delay seconds.
    /// </summary>
    /// <param name='behaviour'>
    /// Behaviour.
    /// </param>
    /// <param name='delay'>
    /// Delay in seconds.
    /// </param>
    /// <param name='fn'>
    /// The function to run.
    /// </param>
    public static void ExecuteLater(this MonoBehaviour behaviour, float delay, System.Action fn)
    {
        behaviour.StartCoroutine(_ExecuteLater(delay, fn));
    }

    static IEnumerator _ExecuteLater(float delay, System.Action fn)
    {
        yield return new WaitForSeconds(delay);
        fn();
    }

    /// <summary>
    /// Run a number of coroutines in sequence.
    /// </summary>
    /// <param name="g">
    /// A <see cref="GameObject"/>
    /// </param>
    /// <param name="tasks">
    /// A <see cref="IEnumerator[]"/>
    /// </param>
    /// <returns>
    /// A <see cref="Coroutine"/>
    /// </returns>
    public static Coroutine RunCoroutines(this MonoBehaviour behaviour, params IEnumerator[] tasks)
    {
        return behaviour.StartCoroutine(_RunCoroutines(tasks));
    }

    /// <summary>
    /// Start a coroutine, then run a method when it is finished.
    /// </summary>
    /// <param name="task">
    /// A <see cref="IEnumerator"/> The task to run as a couroutine.
    /// </param>
    /// <param name="whenFinished">
    /// A <see cref="System.Action"/> The action to take when finished.
    /// </param>
    /// <returns>
    /// A <see cref="Coroutine"/>
    /// </returns>
    public static Coroutine StartCoroutine(this MonoBehaviour behaviour, IEnumerator task, System.Action whenFinished)
    {
        return behaviour.StartCoroutine(_StartCoroutine(task, whenFinished));
    }

    /// <summary>
    /// Run a coroutine, but if it takes too long, abort and call a method.
    /// </summary>
    /// <returns>
    /// A <see cref="Coroutine"/>
    /// </returns>
    public static Coroutine StartCoroutine(this MonoBehaviour behaviour, IEnumerator task, float T, System.Action onTimeOut)
    {
        return behaviour.StartCoroutine(_StartCoroutine(task, T, onTimeOut));
    }

    static IEnumerator _RunCoroutines(IEnumerator[] tasks)
    {
        for (int j = 0, tasksLength = tasks.Length; j < tasksLength; j++)
        {
            var i = tasks[j];
            yield return UniExtender.Instance.StartCoroutine(i);
        }
    }

    static IEnumerator _StartCoroutine(IEnumerator task, System.Action whenFinished)
    {
        yield return UniExtender.Instance.StartCoroutine(task);
        if (whenFinished != null)
            whenFinished();
    }

    static IEnumerator _StartCoroutine(IEnumerator task, float T, System.Action onTimeOut)
    {
        var start = Time.timeSinceLevelLoad;
        while (task.MoveNext())
        {
            yield return task.Current;
            if (Time.timeSinceLevelLoad - start > T)
            {
                onTimeOut();
                yield break;
            }
        }
    }

}
