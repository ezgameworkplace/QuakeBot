using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<System.Action> executionQueue = new Queue<System.Action>();

    public void Update()
    {
        while (executionQueue.Count > 0)
        {
            executionQueue.Dequeue().Invoke();
        }
    }

    public static void Enqueue(System.Action action)
    {
        executionQueue.Enqueue(action);
    }
}
