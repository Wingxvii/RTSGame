using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceConstants {
    public const int SUPPLY_MAX = 100;
    public const int COST_BARRACKS = 500;
    public const int COST_DROIDS = 200;
    public const int COST_TURRERT = 400;

    public const int SUPPLY_PER_BARRACKS = 20;

}

public class ResourceManager : MonoBehaviour
{
    #region SingletonCode
    private static ResourceManager _instance;
    public static ResourceManager Instance { get { return _instance; } }
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

    public int credits = 0;
    public int totalSupply = 0;
    public int supplyCurrent = 0;

    public int numBarracksActive = 0;

    // Start is called before the first frame update
    void Start()
    {
        credits = 1000;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        credits += 1;
    }

    public bool Purchase(EntityType type) {
        switch (type) {
            case EntityType.Barracks:
                if (credits >= ResourceConstants.COST_BARRACKS)
                {
                    credits -= ResourceConstants.COST_BARRACKS;
                    return true;
                }
                return false;
                break;
            case EntityType.Droid:
                if (credits >= ResourceConstants.COST_DROIDS)
                {
                    credits -= ResourceConstants.COST_DROIDS;
                    return true;
                }
                return false;
                break;
            case EntityType.Turret:
                if (credits >= ResourceConstants.COST_TURRERT)
                {
                    credits -= ResourceConstants.COST_TURRERT;
                    return true;
                }
                return false;
                break;
            default:
                Debug.Log("PURCHACE ERROR");
                return false;
        }
    }
    public void UpdateSupply() {
        totalSupply = numBarracksActive * ResourceConstants.SUPPLY_PER_BARRACKS;
    }
}
