using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum EntityType
{
    None,
    Wall,
    Barracks,
    Droid,
    Player,
    Turret,
}

public class SelectableObject : MonoBehaviour
{
    public Behaviour halo;
    public int level = 1;
    public int id;
    public EntityType type;
    public bool destructable = false;
    public GameObject self;
    public static int idtracker { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        halo = (Behaviour)this.GetComponent("Halo");
        halo.enabled = false;
        id = ++idtracker;
        self = this.gameObject;

        //call base function
        BaseStart();
    }

    public void OnSelect()
    {
        Debug.Log("Selected");
        if (halo != null) { halo.enabled = true; }
    }
    public void OnDeselect()
    {
        Debug.Log("Deselected");
        halo.enabled = false;
    }

    private void Update()
    {
        //call base function
        BaseUpdate();
    }

    private void FixedUpdate()
    {
        //call base function
        BaseFixedUpdate();
    }
    public void OnDestroy()
    {
        //call base function
        BaseOnDestory();
    }

    public void ResetValues()
    {
        this.gameObject.transform.position = Vector3.zero;
        this.gameObject.transform.rotation = Quaternion.identity;

        BaseResetValues();
    }

    //base class overrides
    protected virtual void BaseStart() { }
    protected virtual void BaseUpdate() { }
    protected virtual void BaseFixedUpdate() { }
    protected virtual void BaseOnDestory() { }
    protected virtual void BaseResetValues() { }


}
