using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTile : MonoBehaviour {

	enum RESOURCE_TYPE
    {
        STONE = 0,
        WOOD,
        FOOD
    }

    private RESOURCE_TYPE resourceType;

    private BaseManager managerReference;

    private int resourceValue;

    public int largeResource = 300;
    public int mediumResource = 200;
    public int smallResource = 100;

    public GameObject largeMesh;
    public GameObject mediumMesh;
    public GameObject smallMesh;

    private MeshFilter meshFilter;

    public int GetResourceType()
    {
        return (int)resourceType;
    }

    public void Init(BaseManager newManager)
    {
        managerReference = newManager;
    }

    public void MineResource(int miningValue)
    {
        resourceValue -= miningValue;

        managerReference.AddResources(miningValue, (int)resourceType);

        UpdateState();
    }

    private void UpdateState()
    {
        if(resourceValue < largeResource)
        {
            DestroyObject(largeMesh);
        }
        else if(resourceValue < mediumResource)
        {
            DestroyObject(mediumMesh);
        }
        else if(resourceValue < smallResource)
        {
            DestroyObject(smallMesh);
        }
        else if(resourceValue <= 0)
        {
            DestroyObject(gameObject);
        }
    }
}
