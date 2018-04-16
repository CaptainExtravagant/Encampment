using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTile : MonoBehaviour {

	public enum RESOURCE_TYPE
    {
        STONE = 0,
        WOOD,
        FOOD
    }

	public RESOURCE_TYPE resourceType;

    private BaseManager managerReference;

	private int resourceValue;
    private int initialValue;

	private Mesh chosenMesh;
    private MeshFilter meshFilter;
	private MeshCollider colliderReference;

    public int GetResourceType()
    {
        return (int)resourceType;
    }

	void Awake()
	{
		managerReference = FindObjectOfType<BaseManager> ();

		resourceValue = Random.Range (0, 400);
        initialValue = resourceValue;
	}

	private void SetUpMesh()
	{
		colliderReference = gameObject.GetComponent<MeshCollider> ();
		meshFilter = gameObject.GetComponent<MeshFilter> ();
		chosenMesh = meshFilter.mesh;
	}

    public void MineResource(int miningValue)
    {
        resourceValue -= miningValue;

        managerReference.AddResources(miningValue, (int)resourceType);

		if (resourceValue <= 0) {
			Destroy (gameObject);
		}
    }
}
