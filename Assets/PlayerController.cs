using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Camera cameraReference;
    CameraMovement cameraMovement;
    Ray cursorPosition;
    RaycastHit hit;

    GameObject selectedObject;

    BaseVillager villagerReference;
    ResourceTile resourceReference;
    BaseBuilding buildingReference;

    private void Awake()
    {
        cameraReference = Camera.main;
        cameraMovement = cameraReference.GetComponent<CameraMovement>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(selectedObject = FindObjectUnderMouse())
            {
                if(villagerReference = selectedObject.GetComponent<BaseVillager>())
                {
                    Debug.Log("Villager Found");
                    villagerReference.SetSelected(true);
                }
                else if(resourceReference = selectedObject.GetComponent<ResourceTile>())
                {
                    Debug.Log("Resources Found");
                    if(villagerReference != null)
                    {
                        villagerReference.SelectWorkArea(resourceReference.gameObject);
                        villagerReference.SetSelected(false);
                        villagerReference = null;
                    }

                    resourceReference = null;
                }
                else if(buildingReference = selectedObject.GetComponent<BaseBuilding>())
                {

                    Debug.Log("Building Found");
                    //Assign villagers to buildings, open building menu

                    if (villagerReference != null)
                    {
                        villagerReference.SetSelected(false);
                        villagerReference = null;
                    }
                }
            }

            selectedObject = null;
        }
    }

    GameObject FindObjectUnderMouse()
    {
        if (Physics.Raycast(cursorPosition, out hit, Mathf.Infinity))
        {
            return hit.collider.gameObject;
        }

        return null;
    }
}
