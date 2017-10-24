using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : MonoBehaviour {

    private bool placedInWorld;
    private bool isBuilt;

    protected BaseManager baseManager;

    RaycastHit hit;
    Ray cursorPosition;

    public float buildTime;

    public BaseBuilding(string path)
    {

    }

    void LoadBuilding(string path)
    {

    }

    private void Update()
    {
        if(!placedInWorld)
        {
            BindToMouse();
        }
    }

    void BindToMouse()
    {
        //Get the current mouse position in the world
        Vector3 currentPosition = transform.position;

        cursorPosition = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Set the new position to the next rounded value, making the building snap to the grid
        if (Physics.Raycast(cursorPosition, out hit, Mathf.Infinity))
        {
            transform.position = new Vector3(Mathf.Round(hit.point.x), 0.0f, Mathf.Round(hit.point.z));
        }
    }

    public GameObject PlaceInWorld(BaseManager managerReference)
    {
        GameObject constructionReference;

        placedInWorld = true;
        constructionReference = (GameObject)Instantiate(Resources.Load("Buildings/ConstructionActor"), transform.position, Quaternion.identity);

        constructionReference.GetComponent<ConstructionArea>().SetBuildTime(buildTime);
        constructionReference.GetComponent<ConstructionArea>().SetBaseManager(managerReference);

        GameObject.Destroy(gameObject);
        return constructionReference;
    }

    public bool IsPlaced()
    {
        return placedInWorld;
    }

    public bool IsBuilt()
    {
        return isBuilt;
    }

}