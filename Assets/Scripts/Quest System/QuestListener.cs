using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class QuestListener<T, U> : MonoBehaviour where T : QuestListener<T, U> where U : QuestEvent<U, T>
{
    List<Action<U>> functions;

    protected virtual void Awake()
    {
        functions = new List<Action<U>>();
    }

    protected void OnDestroy()
    {
        for (int i = 0; i < functions.Count; ++i)
        {
            QuestObserver<T, U>.Observer.RemoveFunc(functions[i]);
        }

        functions.Clear();
    }

    public void AddFunction(Action<U> function)
    {
        functions.Add(function);
        QuestObserver<T, U>.Observer.AddFunc(function);
    }
}
