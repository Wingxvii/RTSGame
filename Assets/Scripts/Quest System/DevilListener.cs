using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilListener : QuestListener<DevilListener, DevilEvent>
{
    bool Activated = false;

    AchievementScreen screenDisplay;
    protected override void Awake()
    {
        base.Awake();
        screenDisplay = GetComponent<AchievementScreen>();
        AddFunction(SIXkey);
    }

    protected void SIXkey(DevilEvent questEvent)
    {
        if (!Activated)
        {
            screenDisplay.AchievementMessage("EEEEEVIIIIIILLLLL!!!!", "You pressed 6 three times - hopefully by accident");
            Activated = true;
        }
    }
}

