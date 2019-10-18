using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barracks : SelectableObject
{
    public Slider buildProcess;
    public Queue<float> buildTimes;
    public float currentBuildTime = 0;

    public Canvas canvas;

    //inherited function realizations
    protected override void BaseStart()
    {
        canvas = GetComponentInChildren<Canvas>();
        canvas.transform.LookAt(canvas.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);

        buildProcess = GetComponentInChildren<Slider>();
        buildProcess.gameObject.SetActive(false);

        buildTimes = new Queue<float>();
    }
    protected override void BaseUpdate()
    {

        //add to queue
        if (currentBuildTime <= 0 && buildTimes.Count > 0)
        {
            buildProcess.gameObject.SetActive(true);
            currentBuildTime += buildTimes.Dequeue();
        }
        //tick queue
        else if (currentBuildTime > 0)
        {
            buildProcess.value = currentBuildTime / 5.0f;
            currentBuildTime -= Time.deltaTime;
            if (currentBuildTime < 0)
            {
                DroidManager.Instance.QueueFinished(this.transform, EntityType.Droid);
            }
        }
        //queue ended
        else if (currentBuildTime <= 0) {
            buildProcess.gameObject.SetActive(false);
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
