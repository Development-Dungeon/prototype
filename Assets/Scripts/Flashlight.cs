using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashlight; // Reference to the flashlight
    public Camera playerCamera; // Reference to the player's camera

    private bool isOn = false; // Flashlight state

    void Start()
    {
        // Ensure the flashlight is initially off
        flashlight.enabled = isOn;
    }

    void Update()
    {
        // Toggle flashlight on/off with "F" key
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;
            flashlight.enabled = isOn;
        }

        // Make flashlight point to the center of the screen
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        flashlight.transform.position = playerCamera.transform.position;
        flashlight.transform.rotation = Quaternion.LookRotation(ray.direction);
    }
}

