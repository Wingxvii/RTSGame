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

    public MouseEvent currentEvent = MouseEvent.Nothing;

    public List<GameObject> SelectedObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //selection checking
        #region selectionCode
        if (Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100)&& !EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log(hit.transform.gameObject.name);

                
                if (hit.transform.gameObject.tag == "SelectableObject")
                {
                    currentEvent = MouseEvent.Selection;
                    SelectedObjects.Add(hit.transform.gameObject);
                    hit.transform.gameObject.GetComponent<SelectableObject>().OnSelect();
                }
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
        #endregion


    }
}
