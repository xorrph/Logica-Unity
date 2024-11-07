using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstPersonCamera : MonoBehaviour
{
    public Transform player; // player object assigned here as a transform so the rotatin is able to change
    public float mouseSens; // mouse sensitivity, this allows the player to customise the speed at which they can look around
    float cameraVertRot = 0f; // camera vertical rotation 

    bool lockCursor = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false; // make the cursor not visible
        Cursor.lockState = CursorLockMode.Locked; // lock the cursor in the centre of the screen
        mouseSens = gameObject.GetComponentInParent<PlayerInfo>().getSens();
        if (mouseSens == 0)
        {
            mouseSens = 2.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Mouse X") * mouseSens; // get the input of the mouses x axis and multiply this by the mouse sensitivty 
        float inputY = Input.GetAxis("Mouse Y") * mouseSens; //  get the input of the mouses y axis and multiply this by the mouse sensitivty 


        cameraVertRot -= inputY;// change the camera's rotation depending on the Y input from the mouse
        cameraVertRot = Mathf.Clamp(cameraVertRot, -90f, 90f); // limit the camera's y rotation between 90 and -90
        transform.localEulerAngles = Vector3.right * cameraVertRot; // apply the Y roation sn


        player.Rotate(Vector3.up * inputX); // change the player's rotation based on X input 

    }
}
