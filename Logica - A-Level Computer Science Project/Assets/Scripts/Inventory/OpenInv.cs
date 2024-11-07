using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OpenInv : MonoBehaviour
{
    public GameObject Inventory;
    public GameObject Player;
    public GameObject playerCam;
    public GameObject playerCursor;
    public GameObject manager;
    public bool interfaceOn = false;

    // Start is called before the first frame update
    void Start()
    {
        Inventory.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        playerMovement move = Player.GetComponent<playerMovement>();
        firstPersonCamera cam = playerCam.GetComponent<firstPersonCamera>();
        Image pCursor = playerCursor.GetComponent<Image>();
        sandbox pSandbox = manager.GetComponent<sandbox>();

        if (Input.GetKeyUp("q") && interfaceOn == true)

        {
            interfaceOn = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            move.enabled = true;
            cam.enabled = true;
            pCursor.enabled = true;
            pSandbox.enabled = true;

        }

       else if (Input.GetKeyUp("q") && interfaceOn == false)

        {
            interfaceOn = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            move.enabled = false;
            cam.enabled = false;
            pCursor.enabled = false;
            pSandbox.enabled = false;

        }
        Inventory.SetActive(interfaceOn);
    }
}
