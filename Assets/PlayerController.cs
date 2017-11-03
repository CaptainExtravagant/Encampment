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
			//Debug.Log ("Mouse Click");
			selectedObject = FindObjectUnderMouse ();

			if (selectedObject != null) {
				Debug.Log ("Object Selected");

				if (selectedObject.GetComponent<BaseVillager> ()) {
					Debug.Log ("Villager Found");

					villagerReference = selectedObject.GetComponent<BaseVillager> ();

					villagerReference.SetSelected (true);
				}
				else if (selectedObject.GetComponent<ResourceTile> ()) {
					Debug.Log ("Resources Found");
					resourceReference = selectedObject.GetComponent<ResourceTile> ();

					if (villagerReference != null)
					{
						Debug.Log ("Setting Work Area (Controller)");
						villagerReference.SelectWorkArea (resourceReference.gameObject);
						villagerReference.SetSelected (false);
						villagerReference = null;
					}

					resourceReference = null;
				} else if (selectedObject.GetComponent<BaseBuilding> ()) {

					Debug.Log ("Building Found");

					buildingReference = selectedObject.GetComponent<BaseBuilding> ();
					//Assign villagers to buildings, open building menu

					if (villagerReference != null) {
						villagerReference.SetSelected (false);
						villagerReference = null;
					}
				}
			}
			else
			{
				Debug.Log ("Object is null");
			}
           
        }
    }

    GameObject FindObjectUnderMouse()
    {
		GameObject foundObject;

		cursorPosition = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (cursorPosition, out hit, Mathf.Infinity)) {
			foundObject = hit.collider.gameObject;
		} else {
			foundObject = null;
		}

		return foundObject;
    }
}
