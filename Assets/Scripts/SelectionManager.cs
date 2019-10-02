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
    private static SelectionManager _instance;
    public static SelectionManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    //single pattern ends here
    #endregion  

    //mouse selection
    public MouseEvent currentEvent = MouseEvent.Nothing;

    //list of all selected objects
    public List<GameObject> SelectedObjects;

    public Vector3 mousePosition;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        SelectedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100) && hit.transform.gameObject.tag != "SelectableObject")
        {
            //mousePosition = hit.point;
            mousePosition = new Vector3(hit.point.x, 2, hit.point.z);
        }
        //selection checking
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
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

    public void OnPrefabCreation()
    {
        currentEvent = MouseEvent.PrefabBuild;
        if (!SelectedObjects.Count.Equals(0))
        {
            foreach (GameObject obj in SelectedObjects)
            {
                obj.GetComponent<SelectableObject>().OnDeselect();
            }
            SelectedObjects.Clear();
        }
    }
}
