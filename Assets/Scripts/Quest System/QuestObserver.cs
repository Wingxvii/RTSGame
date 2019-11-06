using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class QuestBase
{
    static List<QuestBase> questSingletons;
    protected static List<QuestBase> QuestSingletons
    {
        get { if (questSingletons == null) { questSingletons = new List<QuestBase>(); } return questSingletons; }
    }

    public static void Terminate()
    {
        for (int i = 0; i < questSingletons.Count; ++i)
            QuestSingletons[i].DestroySelf();
        QuestSingletons.Clear();
    }

    protected abstract void DestroySelf();
}

public class QuestObserver<T, U> : QuestBase where T : QuestListener<T, U> where U : QuestEvent<U, T>
{
    static QuestObserver<T, U> observer;
    public static QuestObserver<T, U> Observer
    {
        get { if (observer == null) { observer = new QuestObserver<T, U>(); } return observer; }
    }

    private QuestObserver()
    {
        Debug.Log("ADDED: " + typeof(T));
        QuestSingletons.Add(this);
        questActions = new List<Action<U>>();
    }

    List<Action<U>> questActions;

    public void AddFunc(Action<U> function)
    {
        questActions.Add(function);
    }

    public void RemoveFunc(Action<U> function)
    {
        questActions.Remove(function);
    }

    public void FireEvent(U questEvent)
    {
        for (int i = 0; i < questActions.Count; ++i)
        {
            questActions[i](questEvent);
        }
    }

    protected override void DestroySelf()
    {
        questActions.Clear();
    }
}