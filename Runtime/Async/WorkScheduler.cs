using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DifferentMethods.Extensions.Async
{
    public class WorkScheduler : MonoBehaviour
    {
        Dictionary<string, List<IEnumerator>> queues = new Dictionary<string, List<IEnumerator>>();

        public void ScheduleWorkItem(string name, IEnumerator task)
        {
            var exists = queues.ContainsKey(name);
            if (!exists)
            {
                queues[name] = new List<IEnumerator>();
                StartCoroutine(ProcessQueue(name));
            }
            queues[name].Add(task);
        }

        IEnumerator ProcessQueue(string name)
        {
            var tasks = queues[name];
            while (true)
            {
                yield return null;
                if (tasks.Count > 0)
                {
                    var task = tasks.Pop(0);
                    while (task.MoveNext())
                        yield return task.Current;
                }
                if (tasks.Count == 0)
                {
                    queues.Remove(name);
                    yield break;
                }
            }
        }
    }

}