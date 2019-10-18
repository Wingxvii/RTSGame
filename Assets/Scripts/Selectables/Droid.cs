using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droid : SelectableObject
{

    public float maxSpeed = 5.0f;
    public float minSpeed = 2.0f;
    private Rigidbody selfRigid;

    private Vector3 journeyPoint;
    private bool journey = false;
    public float journeyAccuracy = 5.0f;

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
        /*
        if (selfRigid.velocity.magnitude < minSpeed)
        {
            selfRigid.velocity = Vector3.zero;
        }
        */
        if (journey) {

            if (Vector3.Distance(this.transform.position, journeyPoint) < journeyAccuracy)
            {
                journey = false;
            }
            else {
                MoveTo(new Vector2(journeyPoint.x, journeyPoint.z));
            }
        }
    }

    public override void OnDeactivation()
    {
        OnDeath();
    }

    public override void IssueLocation(Vector3 location)
    {
        Debug.Log("Issued");
        journey = true;
        journeyPoint = location;
    }


    //unique classes
    public void OnDeath()
    {
        DroidManager.Instance.KillDroid(this);
    }

    public void MoveTo(Vector2 pos) {

        Vector2 dir = new Vector2(pos.x - this.transform.position.x, pos.y - this.transform.position.z).normalized;
        selfRigid.velocity = new Vector3(dir.x, 0, dir.y) * maxSpeed;
    }

}
