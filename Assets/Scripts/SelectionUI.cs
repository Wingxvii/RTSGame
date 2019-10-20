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

    public List<Button> pages;
    public int currPage = 1;
    public int itemsPerPage;

    //these are the UI Parameters
    public float XStart = -215;
    public float XOffset = 99;
    public int numOfCols = 14;

    public float YStart = -400;
    public float YOffset = -55;
    public int numOfRows = 2;

    //prefabs
    public Sprite ButtonDroid;
    public Sprite ButtonBarracks;
    public Sprite ButtonWall;
    public Sprite ButtonTurret;
    public GameObject ButtonPrefab;

    public GameObject UIParent;

    public List<GameObject> buttonPool;

    // Start is called before the first frame update
    void Start()
    {
        //fill positions in slot
        for (int counter2 = 0; counter2 < numOfRows; counter2++)
        {
            for (int counter = 0; counter < numOfCols; counter++)
            {
                Vector3 pos = new Vector3(XStart + (counter * XOffset), YStart + (counter2 * YOffset), 0.0f);
                buttonPool.Add(GameObject.Instantiate(ButtonPrefab, pos + UIParent.transform.position, Quaternion.identity));
            }
        }

        foreach (GameObject button in buttonPool)
        {
            button.gameObject.SetActive(false);
            button.transform.parent = UIParent.transform;
        }


        itemsPerPage = buttonPool.Count;

        foreach (Button button in pages)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void ProcessUI(bool resetPage) {
        if (resetPage) { currPage = 1; }

        foreach (GameObject button in buttonPool)
        {
            button.gameObject.SetActive(false);
        }

        int objCounter = 0;
        foreach (SelectableObject obj in SelectionManager.Instance.SelectedObjects) {
            objCounter++;
            if (objCounter < itemsPerPage) {
                buttonPool[objCounter - 1].SetActive(true);

                switch (obj.type) {
                    case EntityType.Barracks:
                        buttonPool[objCounter - 1].GetComponent<Image>().sprite = ButtonBarracks;
                        break;
                    case EntityType.Droid:
                        buttonPool[objCounter - 1].GetComponent<Image>().sprite = ButtonDroid;
                        break;
                    case EntityType.Wall:
                        buttonPool[objCounter - 1].GetComponent<Image>().sprite = ButtonWall;
                        break;
                    case EntityType.Turret:
                        buttonPool[objCounter - 1].GetComponent<Image>().sprite = ButtonTurret;
                        break;
                }


                buttonPool[objCounter - 1].GetComponent<SelectionButton>().OnCreate(obj);
            }
        }
    }

    public void OnPageSwitch( int page) {
        currPage = page;
        ProcessUI(false);
    }

    public void OnElementSelected(SelectionButton button) {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl))
        {
            SelectionManager.Instance.DeselectItem(button.parentObject);
        }
        else {
            SelectionManager.Instance.OnFocusSelected(button.parentObject);
        }
    }

}



