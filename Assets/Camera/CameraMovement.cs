using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {


    //GLOBALS
    float translationX;
    float translationZ;
	float zoom;
    Transform cameraTarget;
    Vector3 holdingPoint;
    bool holdingMouse;
    bool movementEnabled;
	float movement = 5;

    float holdMouseTimer = 0.1f;
    float timer;

    public bool IsHoldingMouse()
    {
        return holdingMouse;
    }

    public void SetCameraMovement(bool enabled)
    {
        movementEnabled = enabled;
    }

    private void Awake()
    {
        //Set camera target reference and holdingMouse to false as default
        cameraTarget = transform.parent;
        holdingMouse = false;
        movementEnabled = true;
    }
	
	void Update () {

        //Update Input Axis for movement
        translationX = Input.GetAxis("Horizontal");
        translationZ = Input.GetAxis("Vertical");

        if (movementEnabled)
        {
			//Edge Scrolling
			if (!holdingMouse) {
				
				if (Input.mousePosition.x >= Screen.width - 5) {
					cameraTarget.Translate (new Vector3 (0.5f,
						0.0f,
						-0.5f), Space.Self);  
				}

				if (Input.mousePosition.x <= 5) {
					cameraTarget.Translate (new Vector3 (-0.5f,
						0.0f,
						0.5f), Space.Self);  
				}

				if (Input.mousePosition.y >= Screen.height - 5) {
					cameraTarget.Translate (new Vector3 (0.5f,
						0.0f,
						0.5f), Space.Self);  
				}
				if (Input.mousePosition.y <= 5) {
					cameraTarget.Translate (new Vector3 (-0.5f,
						0.0f,
						-0.5f), Space.Self);  
				}
			}

            //Keyboard Movement (WASD)
            if (cameraTarget != null)
            {
                //Move camera based on both X and Z so WASD line up properly to perspective; Space.World makes movement based on global axis, not local
                cameraTarget.Translate(new Vector3(((translationX / 2) + (translationZ / 2)),
                    0.0f,
					((translationZ / 2) - (translationX / 2))), Space.Self);                
            }

			//Mouse Wheel Zoom
			cameraTarget.Translate(new Vector3(Input.GetAxisRaw("Mouse ScrollWheel") * movement, -Input.GetAxisRaw("Mouse ScrollWheel") * movement, Input.GetAxisRaw("Mouse ScrollWheel") * movement), Space.Self);

			if (Input.GetKey(KeyCode.Q)) {
				cameraTarget.Rotate (new Vector3 (0, 5.0f, 0), Space.World);
			}

			if (Input.GetKey(KeyCode.E)) {
				cameraTarget.Rotate (new Vector3 (0, -5.0f, 0), Space.World);
			}

            //=====================
            //Click and Drag Movement with Right Mouse Button
            //=====================
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                holdMouseTimer += Time.deltaTime;

            }

            if (Input.GetKey(KeyCode.Mouse1))
            {
                timer += Time.deltaTime;

                if (!holdingMouse && timer >= holdMouseTimer)
                {
                    //Find the position on screen where the mouse was clicked and set holdingMouse to true
                    holdingPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                    holdingMouse = true;

                }

                if (holdingMouse)
                {
                    CameraDragMovement();
                }
            }

            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                holdingMouse = false;

                timer = 0;
            }

            //=====================
            //Tap and Drag Movement for Mobile
            //=====================
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (!holdingMouse)
                {
                    //Find the position on screen where the touch was registered and set holdingMouse to true
                    holdingPoint = Camera.main.ScreenToViewportPoint(Input.GetTouch(0).position);
                    holdingMouse = true;
                }
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
				CameraMobileMovement();
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                holdingMouse = false;
            }
        }

	}

    //Function for moving the camera with mouse and touch on mobile
    void CameraDragMovement()
    {
        //Set offset to the current mouse position - the position when the mouse was clicked
        Vector3 mouseOffset = Camera.main.ScreenToViewportPoint(Input.mousePosition) - holdingPoint;

        //Move camera, use x values normally, empty y value, use y value from mouseOffset so z moves properly; use same method as WASD movement so the mouse movements lines up with camera movement correctly
		cameraTarget.Translate(new Vector3(
			(((mouseOffset.x) / 2) + ((mouseOffset.y) / 2)),
			0.0f,
			(((mouseOffset.y) / 2) - (mouseOffset.x) / 2)),
			Space.Self);
    }

	void CameraMobileMovement()
	{
		//Set offset to the current mouse position - the position when the mouse was clicked
		Vector3 mouseOffset = Camera.main.ScreenToViewportPoint(Input.mousePosition) - holdingPoint;

		//Move camera, use x values normally, empty y value, use y value from mouseOffset so z moves properly; use same method as WASD movement so the mouse movements lines up with camera movement correctly
		cameraTarget.Translate(new Vector3(
			(((-mouseOffset.x) / 2) + ((-mouseOffset.y) / 2)),
			0.0f,
			(((-mouseOffset.y) / 2) - ((-mouseOffset.x) / 2))),
			Space.Self);
	}
}
