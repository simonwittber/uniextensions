using UnityEngine;
using System.Collections;
using System.Threading;

using DifferentMethods.Extensions.Collections;
using System;
using System.Collections.Generic;

namespace DifferentMethods.Extensions.Async
{


    public class ThreadPool : IDisposable
    {
        public void Enqueue(System.Action fn)
        {
            lock (this)
            {
                Worker w;
                if (freeWorkers.Count == 0)
                {
                    w = new Worker(this);
                }
                else
                {
                    w = freeWorkers[0];
                    freeWorkers.RemoveAt(0);
                }
                w.task = fn;
                w.ev.Set();
                busyWorkers.Add(w);
            }
        }

        public ThreadPool()
        {
            freeWorkers = new List<Worker>();
            busyWorkers = new List<Worker>();
        }

        public void Dispose()
        {
            lock (this)
            {
                for (var i = 0; i < freeWorkers.Count; i++)
                {
                    freeWorkers[i].thread.Abort();
                }
                for (var i = 0; i < busyWorkers.Count; i++)
                {
                    busyWorkers[i].thread.Abort();
                }
            }
        }


        class Worker
        {
            public Thread thread;
            public AutoResetEvent ev;
            public System.Action task;
            ThreadPool pool;

            public Worker(ThreadPool pool)
            {
                this.pool = pool;
                ev = new AutoResetEvent(true);
                thread = new Thread(ProcessTasks);
                thread.Start();
            }

            void ProcessTasks()
            {
                try
                {
                    while (true)
                    {
                        if (task == null)
                        {
                            ev.WaitOne();
                            continue;
                        }
                        task();
                        task = null;
                        lock (pool)
                        {
                            pool.busyWorkers.Remove(this);
                            pool.freeWorkers.Add(this);
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex);
                }
            }

        }


        List<Worker> freeWorkers, busyWorkers;

    }
}