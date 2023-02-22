using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Trap Object script

// Spawns a placed trap on the ground
// once the shot trap has landed

public class TrapObject : MonoBehaviour
{

    public GameObject spawnPlacedTrap;

    // Checks for the ground tag
    // and spawns the placed trap
    // at the point of collision with the placed trap
    // local rotation
    // Destroys the shot trap object

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            Destroy(gameObject);
            Instantiate(spawnPlacedTrap, transform.position, spawnPlacedTrap.transform.localRotation);
        }
    }

    
}
