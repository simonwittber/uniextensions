using System.Collections.Generic;
using System.Threading;

namespace DifferentMethods.Extensions.Async
{
    public class Parallel
    {
        public static int NumberOfParallelTasks;

        static Parallel()
        {
            NumberOfParallelTasks = System.Environment.ProcessorCount;
        }

        public static void ForEach<T>(IEnumerable<T> enumerable, System.Action<T> action)
        {
            var syncRoot = new object();

            if (enumerable == null)
                return;

            var enumerator = enumerable.GetEnumerator();

            InvokeAsync<T> del = InvokeAction;

            var seedItemArray = new T[NumberOfParallelTasks];
            var resultList = new List<System.IAsyncResult>(NumberOfParallelTasks);

            for (int i = 0; i < NumberOfParallelTasks; i++)
            {
                bool moveNext;

                lock (syncRoot)
                {
                    moveNext = enumerator.MoveNext();
                    seedItemArray[i] = enumerator.Current;
                }

                if (moveNext)
                {
                    var iAsyncResult = del.BeginInvoke
                    (enumerator, action, seedItemArray[i], syncRoot, i, null, null);
                    resultList.Add(iAsyncResult);
                }
            }

            for (int i = 0, resultListCount = resultList.Count; i < resultListCount; i++)
            {
                var iAsyncResult = resultList[i];
                del.EndInvoke(iAsyncResult);
                iAsyncResult.AsyncWaitHandle.Close();
            }
        }

        delegate void InvokeAsync<T>(IEnumerator<T> enumerator,
        System.Action<T> achtion, T item, object syncRoot, int i);

        static void InvokeAction<T>(IEnumerator<T> enumerator, System.Action<T> action, T item, object syncRoot, int i)
        {
            if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name =
                string.Format("Parallel.ForEach Worker Thread No:{0}", i);

            bool moveNext = true;

            while (moveNext)
            {
                action.Invoke(item);

                lock (syncRoot)
                {
                    moveNext = enumerator.MoveNext();
                    item = enumerator.Current;
                }
            }
        }
    }
}