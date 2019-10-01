using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MouseEvent {
    Nothing = 0,
    Selection = 1,
    PrefabBuild = 2,
}


//SelectionManager.Instance.doSomething();

//void SaveToFile(Vector4(objType, x,y,z));
//Vector4(objType, x,y,z) LoadFromFile();

//SaveManager.Instacne.SaveToFile(fileData);



public class SelectionManager : MonoBehaviour
{
    #region SingletonCode
    //singleton pattern begins here
    private static readonly SelectionManager instance = new SelectionManager();
    static SelectionManager() { }
    private SelectionManager() { }
    public static SelectionManager Instance { get { return instance; } }
    //single pattern ends here
    #endregion  

    //mouse selection
    public MouseEvent currentEvent = MouseEvent.Nothing;

    //list of all selected objects
    public List<GameObject> SelectedObjects;


    // Update is called once per frame
    void Update()
    {
        //selection checking
        if (Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100)&& !EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log(hit.transform.gameObject.name);

                if (hit.transform.gameObject.tag == "SelectableObject")
                {
                    //deselect everything else if left control is not holded down
                    if (!Input.GetKeyDown(KeyCode.LeftControl)) { 
                        foreach (GameObject obj in SelectedObjects)
                        {
                            obj.GetComponent<SelectableObject>().OnDeselect();
                        }
                        SelectedObjects.Clear();
                    }

                    SelectedObjects.Add(hit.transform.gameObject);
                    currentEvent = MouseEvent.Selection;
                    hit.transform.gameObject.GetComponent<SelectableObject>().OnSelect();
                }
                //deselect on ground hit
                else if (hit.transform.gameObject.tag == "Ground") {
                    foreach (GameObject obj in SelectedObjects)
                    {
                        obj.GetComponent<SelectableObject>().OnDeselect();
                    }
                    currentEvent = MouseEvent.Nothing;
                    SelectedObjects.Clear();
                }
            }
        }

    }

    public void OnPrefabCreation() {
        currentEvent = MouseEvent.PrefabBuild;
        foreach (GameObject obj in SelectedObjects)
        {
            obj.GetComponent<SelectableObject>().OnDeselect();
        }
        SelectedObjects.Clear();
    }
}
