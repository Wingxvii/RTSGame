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

    public GameObject redFab;
    public GameObject blueFab;
    public GameObject greenFab;
    public GameObject yellowFab;


    //stack of undo and redo commands
    private Stack<ICommand> _Undocommands = new Stack<ICommand>();
    private Stack<ICommand> _Redocommands = new Stack<ICommand>();

    #region UndoRedo
    void undo()
    {
        if (_Undocommands.Count != 0)
        {
            _Redocommands.Push(_Undocommands.Pop());
            _Redocommands.Peek().UnExecuteAction();
        }
    }
    void redo()
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
                break;
            case 2:
                prefabObject = (GameObject)Instantiate(greenFab);
                break;
            case 3:
                prefabObject = (GameObject)Instantiate(blueFab);
                break;
            case 4:
                prefabObject = (GameObject)Instantiate(yellowFab);
                break;

        }
        prefabObject.GetComponent<Renderer>().material.color = new Color(prefabObject.GetComponent<Renderer>().material.color.r, prefabObject.GetComponent<Renderer>().material.color.g, prefabObject.GetComponent<Renderer>().material.color.b,0.5f);
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
        if (prefabObject.activeSelf) {
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

}
