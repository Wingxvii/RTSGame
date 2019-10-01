﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    void ExecuteAction();
    void UnExecuteAction();
    void Cleanup();
}
public enum BuildingActions
{
    Upgrade,
    Delete,
}

class UpgradeCommand : ICommand
{
    private GameObject buildingElement;
    bool done = false;

    public UpgradeCommand(GameObject building)
    {
        buildingElement = building;
        ExecuteAction();
    }

    public void ExecuteAction()
    {
        buildingElement.GetComponent<SelectableObject>().level++;
        Transform objTransf = buildingElement.GetComponent<Transform>();

        //make it bigger
        objTransf.localScale = new Vector3(objTransf.localScale.x * 1.1f, objTransf.localScale.y * 1.1f, objTransf.localScale.z * 1.1f);
        //make sure its anchored on the ground
        objTransf.position = new Vector3(objTransf.position.x, 0 + objTransf.localScale.y * 0.5f, objTransf.position.z);
        done = true;
    }
    public void UnExecuteAction()
    {
        buildingElement.GetComponent<SelectableObject>().level--;
        Transform objTransf = buildingElement.GetComponent<Transform>();

        //make it smaller
        objTransf.localScale = new Vector3(objTransf.localScale.x * 0.9f, objTransf.localScale.y * 0.9f, objTransf.localScale.z * 0.9f);
        //make sure its anchored on the ground
        objTransf.position = new Vector3(objTransf.position.x, 0 + objTransf.localScale.y * 0.5f, objTransf.position.z);
        done = false;
    }
    public void Cleanup() {
        //nothing needs to be done
    }
}

class DeleteCommand : ICommand
{
    private GameObject buildingElement;
    bool done = false;

    public DeleteCommand(GameObject building)
    {
        buildingElement = building;
        ExecuteAction();
    }

    public void ExecuteAction()
    {
        buildingElement.SetActive(false);
        done = true;
    }
    public void UnExecuteAction()
    {
        buildingElement.SetActive(true);
        done = false;
    }
    public void Cleanup()
    {
        if (!done) {
            Object.Destroy(buildingElement);
        }
    }
}