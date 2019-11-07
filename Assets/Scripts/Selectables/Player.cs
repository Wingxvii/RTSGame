using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SelectableObject
{
    public Transform playerTransform;
    public Rigidbody playerBody;

    static public Vector3 pos = new Vector3(0, 0, 0);
    public float moveSpeed = 1;
    public float maxSpeed = 20.0f;

    // Start is called before the first frame update
    protected override void BaseStart()
    {
        currentHealth = 200;
        maxHealth = 200;

        destructable = false;

        playerTransform = this.GetComponent<Transform>();
        playerBody = this.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    protected override void BaseUpdate()
    {
        //Fire Event on moving the player
        if (Input.GetKey(KeyCode.D))
        {
            PlayerMoveEvent.FireEvent(new PlayerMoveEvent());
            playerBody.velocity += new Vector3(1 * moveSpeed, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            PlayerMoveEvent.FireEvent(new PlayerMoveEvent());
            playerBody.velocity += new Vector3(1 * -moveSpeed, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            PlayerMoveEvent.FireEvent(new PlayerMoveEvent());
            playerBody.velocity += new Vector3(0, 0, 1 * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            PlayerMoveEvent.FireEvent(new PlayerMoveEvent());
            playerBody.velocity += new Vector3(0, 0, 1 * -moveSpeed);
        }
    }

    protected override void BaseFixedUpdate()
    {
        if (playerBody.velocity.magnitude > maxSpeed)
        {
            playerBody.velocity = playerBody.velocity.normalized * maxSpeed;
        }
    }

    public override void OnDeath()
    {
        Debug.Log("Player's Dead");
        if (ResourceConstants.UNKILLABLEPLAYER)
        {
            this.currentHealth = 200;
        }
        else
        {
            base.OnDeath();
        }
    }
}
