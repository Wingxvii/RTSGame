using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this uses the factory method
public enum BuildingEnum
{
    RedBuilding,
    BlueBuilding,
    GreenBuilding,
    YellowBuilding,
    None,
}

public abstract class Building
{
    //creates and sets an id for each building
    public Building()
    {
        id = ++idtracker;
    }
    public GameObject self;
    public static int idtracker { get; private set; }
    public int id;
}

public class RedBuilding : Building
{
    public RedBuilding(Vector3 pos) : base()
    {
        self = GameObject.Instantiate((GameObject)Resources.Load("prefabs/Red", typeof(GameObject)));
        self.GetComponent<SelectableObject>().id = this.id;
    }
}

public class BlueBuilding : Building
{
    public BlueBuilding(Vector3 pos) : base()
    {
        self = GameObject.Instantiate((GameObject)Resources.Load("prefabs/Blue", typeof(GameObject)));
        self.GetComponent<SelectableObject>().id = this.id;
    }
}

public class GreenBuilding : Building
{
    public GreenBuilding(Vector3 pos) : base()
    {
        self = GameObject.Instantiate((GameObject)Resources.Load("prefabs/Green", typeof(GameObject)));
        self.GetComponent<SelectableObject>().id = this.id;
    }
}

public class YellowBuilding : Building
{
    public YellowBuilding(Vector3 pos) : base()
    {
        self = GameObject.Instantiate((GameObject)Resources.Load("prefabs/Yellow", typeof(GameObject)));
        self.GetComponent<SelectableObject>().id = this.id;
    }
}