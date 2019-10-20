using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionButton : MonoBehaviour
{
    public EntityType prefabType;
    public SelectableObject parentObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCreate(SelectableObject parentObj) {
        parentObject = parentObj;
    }

    public void OnClick()
    {
        SelectionUI.Instance.OnElementSelected(this);
    }

}
