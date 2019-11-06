using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestEvent<T, U> where T : QuestEvent<T, U> where U : QuestListener<U, T>
{
    public static int EventsActive { get; private set; } = 0;
    public bool eventActive { get; private set; } = false;

    protected QuestEvent(bool activated)
    {
        eventActive = activated;
        if (eventActive) { ++EventsActive; }
    }
    ~QuestEvent()
    {
        if (eventActive) { --EventsActive; }
    }

    public static void FireEvent(T questEvent)
    {
        //Debug.Log("FIRING! " + questEvent.GetType());
        if (questEvent.eventActive)
            QuestObserver<U, T>.Observer.FireEvent(questEvent);
    }
}