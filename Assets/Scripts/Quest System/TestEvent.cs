using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : QuestEvent<TestEvent, TestListener>
{
    public string message { get; private set; }

    public TestEvent(string messageRelay) : base(true)
    {
        message = messageRelay;
    }
}
