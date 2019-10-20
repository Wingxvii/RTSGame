using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectionUI : MonoBehaviour
{
    #region SingletonCode
    private static SelectionUI _instance;
    public static SelectionUI Instance { get { return _instance; } }
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


    //UI selection buttons
    public List<float> XPos;
    public List<float> YPos;

    public List<Button> pages;
    public int currPage = 1;
    public int itemsPerPage;

    //these are the UI Parameters
    public float XStart = -216;
    public float XOffset = 55;
    public int numOfCols = 14;

    public float YStart = -396;
    public float YOffset = -55;
    public int numOfRows = 2;

    //prefabs
    public GameObject ButtonDroid;
    public GameObject ButtonBarracks;
    public GameObject ButtonWall;
    public GameObject ButtonTurret;

    public List<SelectionButton> buttons;


    // Start is called before the first frame update
    void Start()
    {
        //fill positions in slot
        for (int counter = 0; counter < numOfCols; counter++)
        {
            XPos.Add(XStart + (counter * XOffset));
        }
        for (int counter = 0; counter < numOfRows; counter++)
        {
            YPos.Add(YStart + (counter * YOffset));
        }

        itemsPerPage = numOfRows * numOfCols;
        
        foreach (Button button in pages)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void ProcessUI(bool resetPage) {
        if (resetPage) { currPage = 1; }
        foreach (SelectableObject obj in SelectionManager.Instance.SelectedObjects) {

        }
    }

    public void OnPageSwitch( int page) {
        currPage = page;
        ProcessUI(false);
    }

    public void OnElementSelected(SelectionButton button) {

    }

}



