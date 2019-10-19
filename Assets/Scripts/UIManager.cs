﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;

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
    public GameObject UITurret;
    public GameObject UIWall;
    public GameObject UIPlayer;

    // Start is called before the first frame update
    void Start()
    {
        UIBarracks.SetActive(false);
        UIDroid.SetActive(false);
        UITurret.SetActive(false);
        UIWall.SetActive(false);
        UIPlayer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //use observer for this
        if (SelectionManager.Instance.SelectedObjects.Count > 0)
        {
            switch (SelectionManager.Instance.PrimarySelectable.type)
            {
                case EntityType.Barracks:
                    EnableUI(UIBarracks);
                    break;
                case EntityType.Droid:
                    EnableUI(UIDroid);
                    break;
                case EntityType.Turret:
                    EnableUI(UITurret);
                    break;
                case EntityType.Wall:
                    EnableUI(UIWall);
                    GetStats(SelectionManager.Instance.PrimarySelectable);
                    break;
                case EntityType.Player:
                    EnableUI(UIPlayer);
                    GetStats(SelectionManager.Instance.PrimarySelectable);
                    break;

                default:
                    EnableUI();
                    break;
            }
        }
        else {
            EnableUI(UIBuilding);
        }
    }

    void EnableUI(GameObject enabledUI = null) {

        if (!enabledUI.activeSelf)
        {
            UIBuilding.SetActive(false);
            UIBarracks.SetActive(false);
            UIDroid.SetActive(false);
            UITurret.SetActive(false);
            UIWall.SetActive(false);
            UIPlayer.SetActive(false);

            if (enabledUI != null)
            {
                enabledUI.SetActive(true);
            }
        }
    }

    void GetStats(SelectableObject obj) {
        switch (obj.type) {
            case EntityType.Player:
                UIPlayer.transform.Find("Health").GetComponent<UnityEngine.UI.Text>().text = obj.currentHealth.ToString() + "/" + obj.maxHealth.ToString();
                break;
            case EntityType.Wall:
                UIWall.transform.Find("Health").GetComponent<UnityEngine.UI.Text>().text = obj.currentHealth.ToString() + "/" + obj.maxHealth.ToString();
                break;

            default:
                break;
        }
    }
}
