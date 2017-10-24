using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionArea : MonoBehaviour {

    public float buildTime;
    private BaseManager baseManager;

    public void CreateBuilding()
    {
        //baseManager.toBeBuilt.Remove(this);
        Destroy(gameObject);
    }

    public void SetBaseManager(BaseManager manager)
    {
        baseManager = manager;
    }

    public void SetBuildTime(float newBuildTime)
    {
        buildTime = newBuildTime;
    }

    public void AddConstructionPoints(float points)
    {
        buildTime -= points;
        if(buildTime <= 0)
        {
            CreateBuilding();
        }
    }

    public float GetBuildTime()
    {
        return buildTime;
    }
}
