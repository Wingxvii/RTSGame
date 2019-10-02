using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPattern : MonoBehaviour
{
    #region SingletonCode
    private static CommandPattern _instance;
    public static CommandPattern Instance { get { return _instance; } }
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
    #endregion

    public GameObject prefabObject;
    public BuildingEnum prefabType;

    public GameObject redFab;
    public GameObject blueFab;
    public GameObject greenFab;
    public GameObject yellowFab;


    //stack of undo and redo commands
    private Stack<ICommand> _Undocommands = new Stack<ICommand>();
    private Stack<ICommand> _Redocommands = new Stack<ICommand>();

    #region UndoRedo
    public void undo()
    {
        if (_Undocommands.Count != 0)
        {
            _Redocommands.Push(_Undocommands.Pop());
            _Redocommands.Peek().UnExecuteAction();
        }
    }
    public void redo()
    {
        if (_Redocommands.Count != 0)
        {
            _Undocommands.Push(_Redocommands.Pop());
            _Undocommands.Peek().ExecuteAction();
        }
    }
    #endregion

    public void OnPrefabSelect(int prefab) {
        SelectionManager.Instance.OnPrefabCreation();

        switch (prefab) {
            case 1:
                prefabObject = (GameObject)Instantiate(redFab);
                prefabType = BuildingEnum.RedBuilding;
                break;
            case 2:
                prefabObject = (GameObject)Instantiate(greenFab);
                prefabType = BuildingEnum.GreenBuilding;
                break;
            case 3:
                prefabObject = (GameObject)Instantiate(blueFab);
                prefabType = BuildingEnum.BlueBuilding;
                break;
            case 4:
                prefabObject = (GameObject)Instantiate(yellowFab);
                prefabType = BuildingEnum.YellowBuilding;
                break;

        }
        //define the variable changes required for the prefab
        prefabObject.layer = 2;
        prefabObject.GetComponent<BoxCollider>().enabled = false;
        //prefabObject.GetComponent<SelectableObject>().enabled = false;
        prefabObject.SetActive(true);
    }


    // Start is called before the first frame update
    void Start()
    {
        prefabObject = (GameObject)Instantiate(redFab);
        prefabObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (prefabObject != null && prefabObject.activeSelf) {
            prefabObject.GetComponent<Transform>().position = SelectionManager.Instance.mousePosition;
        }

        if (Input.GetKey(KeyCode.P)) {
            Debug.Break();
        }
    }

    public float Round(float num, float multiple)
    {
        int result = Mathf.RoundToInt(num / multiple);

        return result * multiple;
    }

    public void OnPlace(GameObject placeObject) {
        _Undocommands.Push(new AddCommand(placeObject));
    }

    public void OnDelete() {
        foreach (GameObject obj in SelectionManager.Instance.SelectedObjects) {
            _Undocommands.Push(new DeleteCommand(obj));
        }
    }

}
