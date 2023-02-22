using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Charge Script

// Charge object that shoots out the magnetiser
// and locks on to the closest piece of smaller scrap
// in order to begin the creation of a scrap boulder

// CURRENTLY SCRIPT IS OUT OF USE

public class Charge : MonoBehaviour
{
    public CharacterController controller;
    public float chargeSpeed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        // Moves the charge forward based on which direction 
        // the magnetiser is facing

        controller.Move(transform.forward * chargeSpeed * Time.deltaTime);
    }

    // Checks to make sure the charge hits only the smaller scrap or the ground
    // if not then the charge gets destroyed

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag != "Scrap" && hit.gameObject.tag != "ground")
        {
            
            Destroy(gameObject);
        }

    }

}
