using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniStorm.CharacterController
{
    public class Pause : MonoBehaviour
    {
        bool Paused = false;

        private void Start()
        {
            UpdateCusor(Paused);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Paused = !Paused;
                UpdateCusor(Paused);
                

            }
        }

        private void UpdateCusor(bool pause)
        {
			if (Paused)
			{
			    Cursor.lockState = CursorLockMode.None;
			    Cursor.visible = true;
                //GetComponent<UniStormMouseLook>().enabled = false;
                FirstPersonController.cameraCanMove = false;
            }
			else
			{
			    Cursor.lockState = CursorLockMode.Locked;
			    Cursor.visible = true;
                //GetComponent<UniStormMouseLook>().enabled = true;
                FirstPersonController.cameraCanMove = true;
            }
            
        }
    }
}