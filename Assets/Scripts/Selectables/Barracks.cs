using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barracks : SelectableObject
{
    public Slider buildProcess;
    public Queue<float> buildTimes;
    public float currentBuildTime;

    //inherited function realizations
    protected override void BaseStart()
    {
        buildProcess = GetComponentInChildren<Slider>();
        buildProcess.gameObject.SetActive(false);

        buildTimes = new Queue<float>();
    }
    protected override void BaseUpdate()
    {
        if (currentBuildTime < 0 && buildTimes.Count > 0)
        {
            DroidManager.Instance.QueueFinished(this, EntityType.Droid);
            currentBuildTime += buildTimes.Dequeue();
        }
        if (currentBuildTime > 0)
        {
            currentBuildTime -= Time.deltaTime;
        }
    }


    public override void OnActivation()
    {
        ResourceManager.Instance.numBarracksActive++;
        ResourceManager.Instance.UpdateSupply();
    }
    public override void OnDeactivation()
    {
        ResourceManager.Instance.numBarracksActive--;
        ResourceManager.Instance.UpdateSupply();
    }

    //child-sepific functions
    public void OnTrainRequest() {
        if (ResourceManager.Instance.Purchase(EntityType.Droid))
        {
            buildTimes.Enqueue(DroidManager.Instance.RequestQueue(EntityType.Droid));
        }
    }

}
