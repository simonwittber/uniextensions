using UnityEngine;
using System.Collections;

namespace DifferentMethods.Extensions.Async
{
    /// <summary>
    /// A coroutine that can be suspended, resumed and aborted.
    /// </summary>
    public class ExtCoroutine : IEnumerable
    {
        bool abort = false;
        bool suspend = false;
        IEnumerator coroutine;

        /// <summary>
        /// Create a coroutine with abort suspend and resume functions.
        /// </summary>
        /// <param name="task">
        /// A <see cref="IEnumerator"/> The task which will be be run as a coroutine.
        /// </param>
        public ExtCoroutine(IEnumerator task)
        {
            coroutine = task;
        }

        /// <summary>
        /// Start executing he coroutine. 
        /// </summary>
        public void Start()
        {

            //UniExtender.Instance.StartCoroutine (GetEnumerator ());
        }

        /// <summary>
        /// Abort execution of the coroutine.
        /// </summary>
        public void Abort()
        {
            abort = true;
        }

        /// <summary>
        /// Temporarily suspend the execution of the coroutine.
        /// </summary>
        public void Suspend()
        {
            suspend = true;
        }

        /// <summary>
        /// Resume a previously suspended coroutine.
        /// </summary>
        public void Resume()
        {
            suspend = false;
        }

        public IEnumerator GetEnumerator()
        {
            while (!abort)
            {
                if (suspend)
                    yield return null;
                else
                {
                    if (coroutine.MoveNext())
                        yield return coroutine.Current;
                    else
                        break;
                }
            }
        }

    }
}