using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droid : SelectableObject
{




    public override void OnDeactivation()
    {
        OnDeath();
    }
    public void OnDeath() {
        DroidManager.Instance.KillDroid(this);
    }
}
