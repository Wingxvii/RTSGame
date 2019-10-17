using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject UIDefault;
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
        
    }
}
