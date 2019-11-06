using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestListener : QuestListener<TestListener, TestEvent>
{
    protected override void Awake()
    {
        base.Awake();
        AddFunction(UseEvent);
    }

    protected void UseEvent(TestEvent questEvent)
    {
        Debug.Log(questEvent.message + ", " + name);
    }
}
