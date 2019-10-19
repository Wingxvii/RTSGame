using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurretState
{
    Idle,
    IdleShooting,
    PositionalShooting,
    TargetedShooting,
    Recoil,
    Reloading,
}


public class Turret : SelectableObject
{
    public TurretState state = TurretState.Idle;
    public float visionRange = 30.0f;
    public float maxRange = 50.0f;
    private Player attackPoint;
    private Vector3 attackTarget;

    public ParticleSystem muzzle;

    //stats
    public float reloadRate = 5.0f;
    public float recoilRate = 0.5f;
    public int attackAmno = 10;
    public int attackDamage = 5;

    public float accuracy = 0.85f;

    public float reloadTimer = 0.0f;
    public int currentAmno = 10;

    protected override void BaseStart()
    {
        muzzle = GetComponentInChildren<ParticleSystem>();
        muzzle.Pause();
        currentHealth = 500;
        maxHealth = 500;

    }
    protected override void BaseFixedUpdate()
    {
        float dist;

        switch (state)
        {
            case TurretState.Idle:
                if (Vector3.Distance(DroidManager.Instance.playerTarget.transform.position, this.transform.position) < visionRange)
                {
                    state = TurretState.IdleShooting;
                    attackPoint = DroidManager.Instance.playerTarget;
                }
                break;
            case TurretState.IdleShooting:
                //look at
                this.transform.LookAt(DroidManager.Instance.playerTarget.transform.position);
                dist = Vector3.Distance(DroidManager.Instance.playerTarget.transform.position, this.transform.position);

                if (dist < maxRange)
                {

                    if (currentAmno > 0)
                    {
                        muzzle.Play();
                        if (accuracy - (dist / 100.0f) > Random.Range(0.0f, 1.0f))
                        {
                            DroidManager.Instance.playerTarget.OnDamage(attackDamage);
                        }
                        currentAmno--;
                        state = TurretState.Recoil;
                        reloadTimer += recoilRate;

                    }
                    else
                    {
                        reloadTimer += reloadRate;
                        state = TurretState.Reloading;
                    }
                }
                else{
                        state = TurretState.Idle;
                }
                break;
            case TurretState.TargetedShooting:
                //look at
                this.transform.LookAt(DroidManager.Instance.playerTarget.transform.position);
                dist = Vector3.Distance(DroidManager.Instance.playerTarget.transform.position, this.transform.position);

                if (dist < maxRange)
                {

                    if (currentAmno > 0)
                    {
                        muzzle.Play();

                        if (accuracy - (dist / 100.0f) > Random.Range(0.0f, 1.0f))
                        {
                            DroidManager.Instance.playerTarget.OnDamage(attackDamage);
                        }
                        currentAmno--;
                        state = TurretState.Recoil;
                        reloadTimer += recoilRate;

                    }
                    else
                    {
                        reloadTimer += reloadRate;
                        state = TurretState.Reloading;
                    }

                }
                else
                {
                        state = TurretState.Idle;
                }

                break;
            case TurretState.Recoil:
                //look at
                this.transform.LookAt(DroidManager.Instance.playerTarget.transform.position);

                if (reloadTimer <= 0.0f)
                {
                    state = TurretState.IdleShooting;
                }
                break;

            case TurretState.Reloading:
                if (reloadTimer <= 0.0f)
                {
                    currentAmno = 10;
                    state = TurretState.Idle;
                }
                break;
        }
    }

    protected override void BaseUpdate()
    {
        if (reloadTimer >= 0.0f)
        {
            reloadTimer -= Time.deltaTime;
        }
    }
    public void IssueAttack(Player attackee)
    {
        Debug.Log("attack issueds");
        if (state != TurretState.Reloading)
        {
            state = TurretState.TargetedShooting;
        }
        attackPoint = attackee;
    }
    public void Reload() {
        reloadTimer += reloadRate;
        state = TurretState.Reloading;
    }
    public override void OnDeath()
    {
        Debug.Log("Dead Turret");
        OnDeactivation();
        SelectionManager.Instance.AllObjects.Remove(this);
        Object.Destroy(this);
    }


}
