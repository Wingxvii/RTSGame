using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveEvent : QuestEvent<PlayerMoveEvent, PlayerMoveListener>
{
    public PlayerMoveEvent() : base(true)
    {

    }
}
