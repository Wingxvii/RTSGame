using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droid : SelectableObject
{

    public float maxSpeed = 10.0f;
    private Rigidbody selfRigid;


    protected override void BaseStart()
    {
        selfRigid = this.GetComponent<Rigidbody>();
    }

    //overrided base classes
    protected override void BaseFixedUpdate()
    {
        if (selfRigid.velocity.magnitude > maxSpeed)
        {
            selfRigid.velocity = selfRigid.velocity.normalized * maxSpeed;
        }
    }


    public override void OnDeactivation()
    {
        OnDeath();
    }



    //unique classes
    public void OnDeath() {
        DroidManager.Instance.KillDroid(this);
    }
}
