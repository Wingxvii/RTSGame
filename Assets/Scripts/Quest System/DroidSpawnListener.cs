using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidSpawnListener : QuestListener<DroidSpawnListener, DroidSpawnEvent>
{
    bool Activated = false;

    AchievementScreen screenDisplay;
    protected override void Awake()
    {
        base.Awake();
        screenDisplay = GetComponent<AchievementScreen>();
        AddFunction(PPkey);
    }

    protected void PPkey(DroidSpawnEvent questEvent)
    {
        if (!Activated)
        {
            screenDisplay.AchievementMessage("What should we call him?", "You spawned a droid. Let's call him Greg.");
            Activated = true;
        }
    }
}
