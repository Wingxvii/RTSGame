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

    //list of all objects
    public List<GameObject> AllObjects;

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
        #region place prefab
        if (currentEvent == MouseEvent.PrefabBuild && Input.GetMouseButtonDown(0)) {
            Object.Destroy(CommandPattern.Instance.prefabObject);
            AllObjects.Add(UseFactoryPattern(mousePosition, CommandPattern.Instance.prefabType));           //LOOK HERE
            CommandPattern.Instance.OnPlace(AllObjects[AllObjects.Count - 1]);
        }

        #endregion

        #region update mouse
        //update mouse position on screen
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100) && hit.transform.gameObject.tag != "SelectableObject")
        {
            mousePosition = hit.point;
            //mousePosition = new Vector3(hit.point.x, 2, hit.point.z);
        }

        #endregion

        #region selection
        //selection checking
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log(hit.transform.gameObject.name);

            if (hit.collider == null) {
                foreach (GameObject obj in SelectedObjects)
                {
                    obj.GetComponent<SelectableObject>().OnDeselect();
                }
                currentEvent = MouseEvent.Nothing;
                SelectedObjects.Clear();
            }
            else if (hit.collider != null && hit.transform.gameObject.tag == "SelectableObject")
            {

                //deselect everything else if left control is not holded down
                if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
                {
                    ClearSelection();
                }
                SelectedObjects.Add(hit.transform.gameObject);
                currentEvent = MouseEvent.Selection;
                hit.transform.gameObject.GetComponent<SelectableObject>().OnSelect();
            }
            //deselect on ground hit
            else if (hit.collider != null && hit.transform.gameObject.tag == "Ground")
            {
                foreach (GameObject obj in SelectedObjects)
                {
                    obj.GetComponent<SelectableObject>().OnDeselect();
                }
                currentEvent = MouseEvent.Nothing;
                SelectedObjects.Clear();
            }
        }
        #endregion
    }

    public void OnPrefabCreation()
    {
        ClearSelection();
        currentEvent = MouseEvent.PrefabBuild;
    }

    public void ClearSelection() {
        if (!SelectedObjects.Count.Equals(0))
        {
            foreach (GameObject obj in SelectedObjects)
            {
                obj.GetComponent<SelectableObject>().OnDeselect();
            }
            SelectedObjects.Clear();
        }
        currentEvent = MouseEvent.Nothing;

    }

    //here is the factory
    public GameObject UseFactoryPattern(Vector3 pos, BuildingEnum type) {

        GameObject returnObj;
        Building temp;
        switch (type) {

            case BuildingEnum.BlueBuilding:
                temp = new BlueBuilding(pos, out returnObj);
                return returnObj;
            case BuildingEnum.RedBuilding:
                temp = new RedBuilding(pos, out returnObj);
                return returnObj;
            case BuildingEnum.GreenBuilding:
                temp = new GreenBuilding(pos, out returnObj);
                return returnObj;
            case BuildingEnum.YellowBuilding:
                temp = new YellowBuilding(pos, out returnObj);
                return returnObj;
            default:
                return new GameObject();

        }
        
    }
}
