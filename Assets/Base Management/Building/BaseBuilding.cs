using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : MonoBehaviour {

    bool placedInWorld;
    bool built;

    RaycastHit hit;
    Ray cursorPosition;

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
        //transform.position = new Vector3(cursorPosition.x, 0.0f, cursorPosition.z);
    }

    public GameObject PlaceInWorld()
    {
        GameObject constructionReference;

        placedInWorld = true;
        constructionReference = (GameObject)Instantiate(Resources.Load("Buildings/ConstructionActor"), transform.position, Quaternion.identity);
        GameObject.Destroy(gameObject);
        return constructionReference;
    }

    public bool IsPlaced()
    {
        return placedInWorld;
    }

}
