﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MouseEvent
{
    Nothing = 0,
    Selection = 1,
    PrefabBuild = 2,
    UnitMove = 3,
    UnitAttack = 4,
    Rally = 5,
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

    //selected types
    public bool selectedTypeFlag = false;

    //raycasting
    public Vector3 mousePosition;
    private Ray ray;
    private RaycastHit hit;
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

        //raycast the mouse
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 500))
        {
            mousePosition = hit.point;
        }

        //handle left mouse click events
        HandleLeftMouseClicks();

        //handle right mouse click events
        HandleRightMouseClicks();

        //handle key press
        HandleKeys();
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
                pos = new Vector3(pos.x, 2.0f, pos.z);
                return GameObject.Instantiate(RTSManager.Instance.turretPrefab, pos, Quaternion.identity);
            case EntityType.Barracks:
                pos = new Vector3(pos.x, 1.0f, pos.z);
                return GameObject.Instantiate(RTSManager.Instance.barracksPrefab, pos, Quaternion.identity);
            case EntityType.Wall:
                pos = new Vector3(pos.x, 2.0f, pos.z);
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
                        //Debug.Log(obj.name);
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

    private void HandleKeys()
    {

        //destroy preset on shift hold up
        if (Input.GetKeyUp(KeyCode.LeftShift) && currentEvent == MouseEvent.PrefabBuild)
        {
            Object.Destroy(RTSManager.Instance.prefabObject);
            ClearSelection();
        }


        //handles primary selectable cycling
        if (Input.GetKeyDown(KeyCode.Tab) && PrimarySelectable != null && currentEvent == MouseEvent.Selection) {
            //allows the program to do a full search starting from the flag
            do
            {
                foreach (SelectableObject obj in SelectedObjects)
                {
                    //this checks for the first item in the next selectable type, then switches the primary to it
                    if (PrimarySelectable == obj)
                    {
                        //checks if this went full circle
                        if (selectedTypeFlag)
                        {
                            selectedTypeFlag = false;
                            break;
                        }
                        //else, set flag to true
                        selectedTypeFlag = true;
                    }
                    //if a different type is found, and flag is active
                    if (obj.type != PrimarySelectable.type && selectedTypeFlag)
                    {
                        selectedTypeFlag = false;
                        PrimarySelectable = obj;
                    }
                }
            } while (selectedTypeFlag);                 //my first practical use of do-while =D
        }

    }

    private void HandleLeftMouseClicks() {

        //check if anything events need to be handled
        if (Input.GetMouseButtonDown(0)) {

            if (currentEvent == MouseEvent.PrefabBuild)
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
            else if (currentEvent == MouseEvent.UnitMove) {
                //send the mouse location of all objects with the same type as the primary type
                foreach (SelectableObject obj in SelectedObjects)
                {
                    if (obj.type == PrimarySelectable.type)
                    {
                        obj.IssueLocation(mousePosition);
                    }
                }

                AnimationManager.Instance.PlayMove(mousePosition);
                Object.Destroy(RTSManager.Instance.prefabObject);
                currentEvent = MouseEvent.Selection;

            }
            else if (currentEvent == MouseEvent.UnitAttack)
            {
                if (hit.transform.gameObject.tag == "SelectableObject" && hit.transform.GetComponent<SelectableObject>().type == EntityType.Player)
                {
                    switch (PrimarySelectable.type)
                    {
                        //droids will attack tether to enemy
                        case EntityType.Droid:
                            foreach (SelectableObject obj in SelectedObjects)
                            {
                                if (obj.type == EntityType.Droid)
                                {
                                    Droid temp = (Droid)obj;
                                    temp.IssueAttack(hit.transform.gameObject.GetComponent<Player>());
                                }
                            }
                            break;
                        //turrets will aim attack enemy
                        case EntityType.Turret:
                            foreach (SelectableObject obj in SelectedObjects)
                            {
                                if (obj.type == EntityType.Turret)
                                {
                                    Turret temp = (Turret)obj;
                                    temp.IssueAttack(hit.transform.gameObject.GetComponent<Player>());
                                }
                            }
                            break;

                        default:
                            break;
                    }
                    AnimationManager.Instance.PlayAttack(mousePosition);
                    Object.Destroy(RTSManager.Instance.prefabObject);
                    currentEvent = MouseEvent.Selection;
                }
                else
                {
                    switch (PrimarySelectable.type)
                    {
                        //droids will attack tether to enemy
                        case EntityType.Droid:
                            foreach (SelectableObject obj in SelectedObjects)
                            {
                                if (obj.type == EntityType.Droid)
                                {
                                    Droid temp = (Droid)obj;
                                    temp.IssueAttack(mousePosition);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    AnimationManager.Instance.PlayAttack(mousePosition);
                    Object.Destroy(RTSManager.Instance.prefabObject);
                    currentEvent = MouseEvent.Selection;
                }

            }
            else if (currentEvent == MouseEvent.Rally)
            {
                //send the mouse location of all objects with the same type as the primary type
                foreach (SelectableObject obj in SelectedObjects)
                {
                    if (obj.type == PrimarySelectable.type)
                    {
                        obj.IssueLocation(mousePosition);
                    }
                }

                Object.Destroy(RTSManager.Instance.prefabObject);
                currentEvent = MouseEvent.Selection;

            }
            else
            {

                //selection checking
                if (!EventSystem.current.IsPointerOverGameObject())
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
            }
        }


    }

    private void HandleRightMouseClicks()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (currentEvent == MouseEvent.PrefabBuild || currentEvent == MouseEvent.UnitMove || currentEvent == MouseEvent.UnitAttack || currentEvent == MouseEvent.Rally)
            {
                Object.Destroy(RTSManager.Instance.prefabObject);
                currentEvent = MouseEvent.Selection;
            }

            if (currentEvent == MouseEvent.Selection) {

                //check if enemy selected
                if (hit.transform.gameObject.tag == "SelectableObject" && hit.transform.GetComponent<SelectableObject>().type == EntityType.Player)
                {
                    switch (PrimarySelectable.type)
                    {
                        //droids will attack tether to enemy
                        case EntityType.Droid:
                            foreach (SelectableObject obj in SelectedObjects)
                            {
                                if (obj.type == EntityType.Droid)
                                {
                                    Droid temp = (Droid)obj;
                                    temp.IssueAttack(hit.transform.gameObject.GetComponent<Player>());
                                }
                            }
                            break;
                        //turrets will aim attack enemy
                        case EntityType.Turret:
                            foreach (SelectableObject obj in SelectedObjects)
                            {
                                if (obj.type == EntityType.Turret)
                                {
                                    Turret temp = (Turret)obj;
                                    temp.IssueAttack(hit.transform.gameObject.GetComponent<Player>());
                                }
                            }
                            break;

                        default:
                            break;
                    }
                    AnimationManager.Instance.PlayAttack(mousePosition);
                }
                else
                {

                    switch (PrimarySelectable.type)
                    {
                        case EntityType.Droid:
                            AnimationManager.Instance.PlayMove(mousePosition);
                            break;
                        default:
                            break;
                    }

                    //send the mouse location of all objects with the same type as the primary type
                    foreach (SelectableObject obj in SelectedObjects)
                    {
                        if (obj.type == PrimarySelectable.type)
                        {
                            obj.IssueLocation(mousePosition);
                        }
                    }
                }
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
