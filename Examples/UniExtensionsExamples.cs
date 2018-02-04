using UnityEngine;
using System.Collections;

using DifferentMethods.Extensions.Async;
using DifferentMethods.Extensions;

public class UniExtensionsExamples : MonoBehaviour
{



    //How to use Markov Generator
    //--------------------------------------------------------------
    [ContextMenu("Generate Names")]
    void GenerateNames()
    {
        //note, usually a much large sample set of words is used.
        var names = "Boris Vlad Kierov Alexi Yuri Artur Vasily Vladislav Viktor Sergei".Split(' ');
        var mg = new DifferentMethods.Extensions.Procedural.MarkovGenerator(names, 2);
        Debug.Log(mg.NextString());
        Debug.Log(mg.NextString());
        Debug.Log(mg.NextString());
        Debug.Log(mg.NextString());
        Debug.Log(mg.NextString());
    }

    //How to use ExtCoroutine
    //--------------------------------------------------------------
    void ExtCoroutineDemo()
    {
        var task = new ExtCoroutine(ThisIsACoroutine());
        task.Start();
        task.Suspend();
        task.Abort();
    }

    IEnumerator ThisIsACoroutine()
    {
        while (true)
        {
            yield return null;
        }
    }


    //How to use MagicThreads
    //--------------------------------------------------------------
    void MagicThreadDemo()
    {
        MagicThread.Start(ThisIsAMagicThread(), false);
    }

    IEnumerator ThisIsAMagicThread()
    {
        //at this line the coroutine is running in the main unity thread.
        yield return null;
        //the next line will move the coroutine into a background thread.
        yield return new BackgroundTask();

        //at this line the coroutine is running in a background thread.
        yield return null;

        //the next line will move the coroutine back into the main unity thread
        yield return new ForegroundTask();

        //at this line the coroutine is back running in the main unity thread.
        yield return null;
    }


    [ContextMenu("JSON Test")]
    void JsonTester()
    {
        var instance = new JsonTest();
        try
        {
            var json = DifferentMethods.Extensions.Serialization.JsonSerializer.Encode(instance);
            Debug.Log(json);
            Debug.Log(DifferentMethods.Extensions.Serialization.JsonPrettyPrint.Format(json));
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

    }




    public class JsonTest
    {
        public string s = "Xyzzy";
        public char c = 'C';
        public int i = 42;
        public float f = 3.14f;
        public int[] intArray = new int[] { 1, 2, 3, 4, 5 };
        public string[] strArray = new string[] { "Xyzzy", "Plugh" };

    }

}
