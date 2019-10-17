using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    #region SingletonCode
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }
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

    public GameObject UIBuilding;
    public GameObject UIBarracks;
    public GameObject UIDroid;

    // Start is called before the first frame update
    void Start()
    {
        UIBarracks.SetActive(false);
        UIDroid.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //use observer for this
        if (SelectionManager.Instance.SelectedObjects.Count > 0)
        {
            switch (SelectionManager.Instance.SelectedObjects[SelectionManager.Instance.SelectedObjects.Count - 1].GetComponent<SelectableObject>().type)
            {
                case EntityType.Barracks:
                    EnableUI(UIBarracks);
                    break;
                case EntityType.Droid:
                    EnableUI(UIDroid);
                    break;
            }
        }
        else {
            EnableUI(UIBuilding);
        }
    }

    void EnableUI(GameObject enabledUI) {
        UIBuilding.SetActive(false);
        UIBarracks.SetActive(false);
        UIDroid.SetActive(false);

        enabledUI.SetActive(true);
    }

}
