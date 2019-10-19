using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : SelectableObject
{

    protected override void BaseStart()
    {
        currentHealth = 500;
        maxHealth = 500;

    }

}
