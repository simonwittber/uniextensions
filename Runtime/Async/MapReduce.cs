using UnityEngine;
using System.Collections;
using System.Threading;

using DifferentMethods.Extensions.Collections;
using System;

namespace DifferentMethods.Extensions.Async
{

    /// <summary>
    /// A simple way to spread execution of functions across multiple threads.
    /// </summary>
    public class MapReduce : IDisposable
    {

        /// <summary>
        /// Add a method to the queue of tasks to be executed.
        /// </summary>
        /// <param name='fn'>
        /// Fn.
        /// </param>
        public void Enqueue(System.Action fn)
        {
            tasks.Write(fn);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DifferentMethods.Extensions.MapReduce"/> class.
        /// </summary>
        /// <param name='poolSize'>
        /// Pool size. the number of threads to use.
        /// </param>
        /// <param name='maxQueueSize'>
        /// Max queue size. the largest possible number of queued tasks.
        /// </param>
        public MapReduce(int poolSize, int maxQueueSize)
        {
            tasks = new RingBuffer<System.Action>(maxQueueSize);
            workers = new Worker[poolSize];
            for (var i = 0; i < poolSize; i++)
            {
                workers[i] = new Worker(tasks);
            }
        }

        public void Dispose()
        {
            for (var i = 0; i < workers.Length; i++)
            {
                workers[i].thread.Abort();
            }
        }

        /// <summary>
        /// Execute all the queued tasks. If wait is true, this method will block until all tasks are finished.
        /// </summary>
        /// <param name='wait'>
        /// If set to <c>true</c> wait.
        /// </param>
        public void Execute(bool wait = false)
        {
            for (int i = 0; i < workers.Length; i++)
            {
                var w = workers[i];
                w.ev.Set();
            }
            if (wait)
            {
                while (!tasks.Empty) ;
            }

        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="DifferentMethods.Extensions.MapReduce"/> is finished executing all the queued tasks.
        /// </summary>
        /// <value>
        /// <c>true</c> if done; otherwise, <c>false</c>.
        /// </value>
        public bool Done
        {
            get
            {
                return tasks.Empty;
            }
        }

        class Worker
        {
            public Thread thread;
            public AutoResetEvent ev;
            RingBuffer<System.Action> tasks;

            public Worker(RingBuffer<System.Action> tasks)
            {
                this.tasks = tasks;
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
                        if (tasks.Empty)
                        {
                            ev.WaitOne();
                            continue;
                        }
                        try
                        {
                            var fn = tasks.Read();
                            fn();
                        }
                        catch (IndexOutOfRangeException)
                        {
                            continue;
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning(ex);
                }
            }

        }

        RingBuffer<System.Action> tasks;
        Worker[] workers;

    }
}
