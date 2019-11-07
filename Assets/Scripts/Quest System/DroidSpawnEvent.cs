using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidSpawnEvent : QuestEvent<DroidSpawnEvent, DroidSpawnListener>
{
    public DroidSpawnEvent() : base(true)
    {

    }
}