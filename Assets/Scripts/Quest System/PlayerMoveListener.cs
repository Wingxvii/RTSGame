using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When the player moves
[RequireComponent(typeof(AchievementScreen))]
public class PlayerMoveListener : QuestListener<PlayerMoveListener, PlayerMoveEvent>
{
    bool Activated = false;

    AchievementScreen screenDisplay;
    protected override void Awake()
    {
        base.Awake();
        screenDisplay = GetComponent<AchievementScreen>();
        AddFunction(MovingPlayer);
    }

    protected void MovingPlayer(PlayerMoveEvent questEvent)
    {
        if (!Activated)
        {
            screenDisplay.AchievementMessage("Run Forrest, RUN!", "You moved the player. Congrats!");
            Activated = true;
        }
    }
}
