using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace DifferentMethods.Extensions.Async
{
    public class BackgroundTask : YieldInstruction
    {
    };

    public class ForegroundTask : YieldInstruction
    {
    };

    /// <summary>
    /// Magic thread allow a coroutine to go into the background and return to foreground using yield statements.
    /// </summary>
    public class MagicThread : MonoBehaviour
    {

        static MagicThread _instance;

        static MagicThread instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("MagicThreads", typeof(MagicThread)).GetComponent<MagicThread>();
                    _instance.gameObject.hideFlags = HideFlags.HideAndDontSave;
                }
                return _instance;
            }
        }

        public static void Start(IEnumerator task, bool startInBackground = true)
        {
            if (startInBackground)
                instance.backgroundTasks.Add(task);
            else
                instance.foregroundTasks.Add(task);
        }

        List<IEnumerator> foregroundTasks = new List<IEnumerator>();
        List<IEnumerator> backgroundTasks = new List<IEnumerator>();

        IEnumerator Start()
        {
            while (true)
            {
                yield return null;
                if (foregroundTasks.Count > 0)
                {
                    IEnumerator[] newTasks;
                    lock (foregroundTasks)
                    {
                        newTasks = foregroundTasks.ToArray();
                        foregroundTasks.Clear();
                    }
                    for (int j = 0; j < newTasks.Length; j++)
                    {
                        var i = newTasks[j];
                        StartCoroutine(HandleCoroutine(i));
                    }
                }
                if (backgroundTasks.Count > 0)
                {
                    for (int j = 0; j < backgroundTasks.Count; j++)
                    {
                        var i = backgroundTasks[j];
                        System.Threading.ThreadPool.QueueUserWorkItem(state =>
                        {
                            HandleThread(i);
                        });
                    }
                    backgroundTasks.Clear();
                }
            }
        }

        IEnumerator HandleCoroutine(IEnumerator task)
        {
            while (task.MoveNext())
            {
                var t = task.Current;
                if ((t as BackgroundTask) == null)
                    yield return t;
                else
                {
                    backgroundTasks.Add(task);
                    yield break;
                }
            }
        }

        void HandleThread(IEnumerator task)
        {
            try
            {
                while (task.MoveNext())
                {
                    var t = task.Current;
                    if ((t as ForegroundTask) != null)
                    {
                        lock (foregroundTasks)
                        {
                            foregroundTasks.Add(task);
                        }
                        break;
                    }
                }
            }
            catch (ThreadAbortException)
            {
                return;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Exception in MagicThread: " + e.ToString());
            }
        }


    }
}
