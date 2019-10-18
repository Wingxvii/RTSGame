using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MouseEvent
{
    Nothing = 0,
    Selection = 1,
    PrefabBuild = 2,
}

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

    public Vector2 boxStart;
    public Vector2 boxEnd;

    public GameObject player;
    public bool boxActive = false;
    public Texture selectionBox;

    //mouse selection
    public MouseEvent currentEvent = MouseEvent.Nothing;

    //list of all objects
    public List<SelectableObject> AllObjects;
    //list of all selected objects
    public List<SelectableObject> SelectedObjects;
    //primary selected object
    public SelectableObject PrimarySelectable;

    public Vector3 mousePosition;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        SelectedObjects = new List<SelectableObject>();
        AllObjects.Add(player.GetComponent<SelectableObject>());
    }

    // Update is called once per frame
    void Update()
    {
        //handle selection box first
        HandleSelectionBox();

        //handle mouse click events last
        HandleMouseClickEvents();

    }

    public void OnPrefabCreation()
    {
        ClearSelection();
        currentEvent = MouseEvent.PrefabBuild;
    }

    public void ClearSelection()
    {
        if (!SelectedObjects.Count.Equals(0))
        {
            foreach (SelectableObject obj in SelectedObjects)
            {
                obj.OnDeselect();
            }
            SelectedObjects.Clear();
        }
        currentEvent = MouseEvent.Nothing;
        boxStart = Vector2.zero;
        boxEnd = Vector2.zero;
        boxActive = false;

        SwitchPrimarySelected();
    }

    //here is the factory
    public GameObject UseFactoryPattern(Vector3 pos, EntityType type) {
        switch (type)
        {
            case EntityType.Turret:
                return GameObject.Instantiate(RTSManager.Instance.turretPrefab, pos, Quaternion.identity);
            case EntityType.Barracks:
                return GameObject.Instantiate(RTSManager.Instance.barracksPrefab, pos, Quaternion.identity);
            case EntityType.Wall:
                return GameObject.Instantiate(RTSManager.Instance.wallPrefab, pos, Quaternion.identity);
            default:
                return new GameObject();

        }

    }

    private void HandleSelectionBox()
    {
        //handle box init behaviour
        if (Input.GetMouseButtonDown(0) && boxActive == false && currentEvent != MouseEvent.PrefabBuild)
        {
            boxStart = Input.mousePosition;
            boxActive = true;
        }
        //handle box drag updates
        else if (Input.GetMouseButton(0) && boxActive)
        {
            if (Mathf.Abs(boxStart.x - Input.mousePosition.x) > 15 || Mathf.Abs(boxStart.y - Input.mousePosition.y) > 15)
            {
                boxEnd = Input.mousePosition;
            }
            else
            {
                boxEnd = Vector2.zero;
            }
        }
        //handle box release
        if (Input.GetMouseButtonUp(0) && boxActive)
        {
            Vector3 worldSelection1;

            Ray rayCast = Camera.main.ScreenPointToRay(boxStart);
            RaycastHit castHit;
            if (Physics.Raycast(rayCast, out castHit, 500))
            {
                worldSelection1 = castHit.point;

                foreach (SelectableObject obj in AllObjects)
                {
                    if (obj.GetComponent<Transform>().position.x >= Mathf.Min(worldSelection1.x, mousePosition.x) &&
                        obj.GetComponent<Transform>().position.x <= Mathf.Max(worldSelection1.x, mousePosition.x) &&
                        obj.GetComponent<Transform>().position.z >= Mathf.Min(worldSelection1.z, mousePosition.z) &&
                        obj.GetComponent<Transform>().position.z <= Mathf.Max(worldSelection1.z, mousePosition.z) && !SelectedObjects.Contains(obj))
                    {
                        Debug.Log(obj.name);
                        SelectedObjects.Add(obj);
                        currentEvent = MouseEvent.Selection;
                        obj.GetComponent<SelectableObject>().OnSelect();

                    }
                }
                SwitchPrimarySelected();
            }

            boxStart = Vector2.zero;
            boxEnd = Vector2.zero;
            boxActive = false;
        }
    }

    private void HandleMouseClickEvents() {

        //update mouse position on screen
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 500) && hit.transform.gameObject.tag != "SelectableObject")
        {
            mousePosition = hit.point;
            //mousePosition = new Vector3(hit.point.x, 2, hit.point.z);
        }

        //check if anything needs to be done
        if (currentEvent == MouseEvent.PrefabBuild && Input.GetMouseButtonDown(0))
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                Object.Destroy(RTSManager.Instance.prefabObject);
                ClearSelection();
            }

            if (ResourceManager.Instance.Purchase(RTSManager.Instance.prefabObject.GetComponent<SelectableObject>().type))
            {

                RTSManager.Instance.OnPlace(UseFactoryPattern(mousePosition, RTSManager.Instance.prefabType));
            }
            else
            {
                Debug.Log("NOT ENOUGH CREDITS");

            }
        }

        else
        {

            //selection checking
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log(hit.transform.gameObject.name);

                if (hit.collider == null)
                {
                    foreach (SelectableObject obj in SelectedObjects)
                    {
                        obj.OnDeselect();
                    }
                    SelectedObjects.Clear();
                }
                else if (hit.transform.gameObject.tag == "SelectableObject")
                {
                    //deselect everything else if left control is not holded down
                    if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
                    {
                        ClearSelection();
                    }
                    SelectedObjects.Add(hit.transform.gameObject.GetComponent<SelectableObject>());
                    SwitchPrimarySelected(hit.transform.gameObject.GetComponent<SelectableObject>());

                    currentEvent = MouseEvent.Selection;
                    hit.transform.gameObject.GetComponent<SelectableObject>().OnSelect();
                }
                //deselect on ground selection, with selection exceptions
                else if (hit.transform.gameObject.tag == "Ground" && !((currentEvent == MouseEvent.PrefabBuild || (currentEvent == MouseEvent.Selection && boxActive)) && Input.GetKey(KeyCode.LeftShift)))
                {
                    foreach (SelectableObject obj in SelectedObjects)
                    {
                        obj.OnDeselect();
                    }
                    currentEvent = MouseEvent.Nothing;
                    SelectedObjects.Clear();
                }
            }
            //destroy preset on shift hold up
            else if (currentEvent == MouseEvent.PrefabBuild && Input.GetKeyUp(KeyCode.LeftShift))
            {
                Object.Destroy(RTSManager.Instance.prefabObject);
                ClearSelection();
            }
        }
    }

    public void SwitchPrimarySelected(SelectableObject primary = null) {
        if (primary == null && SelectedObjects.Count > 0) {
            PrimarySelectable = SelectedObjects[SelectedObjects.Count - 1];
        }
        else
        {
            PrimarySelectable = primary;
        }

    }

    private void OnGUI()
    {
        //used to draw selection box

        if (boxStart != Vector2.zero && boxEnd != Vector2.zero) {
            GUI.DrawTexture(new Rect(boxStart.x, Screen.height - boxStart.y, boxEnd.x - boxStart.x, -1 * ((Screen.height - boxStart.y) - (Screen.height - boxEnd.y))), selectionBox);
        }
    }

}
