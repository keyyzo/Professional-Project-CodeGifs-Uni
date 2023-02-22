using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Spawned Trap script

// A temporary trap that lays on the ground,
// changing the speed, accelleration and velocity values
// of any animal that touches the trap

public class SpawnedTrap : MonoBehaviour
{
    public GameObject trappedAnimalBehaviour;
    public GameObject trappedAnimal;

    private float currentTime = 0.0f;
    public float aliveTimer = 15.0f;
    public float timeAnimalIsTrapped = 5.0f;
    private float tempAnimalMoveSpeed;
    private float tempAnimalTurnSpeed;
    private float tempAnimalAccellSpeed;
    private Vector3 tempAnimalVelocity;

    private bool isTrapAlive = false;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the base trap timer
        // to a value that is changeable in-engine

        currentTime = aliveTimer;
    }

    // Update is called once per frame
    void Update()
    {
        // If trap doesnt have an animal stuck in it
        // continue the base timer

        if (!isTrapAlive)
        {
            currentTime -= 1.0f * Time.deltaTime;
        }

        // If an animal does end up stuck in the trap

        else
        {
           // Stops reducing time from the base trap timer
           // Begins reducing time from a trapped timer,
           // controlled in engine to potentially accomodate for
           // animal behaviour

            timeAnimalIsTrapped -= 1.0f * Time.deltaTime;
        }

        // if base timer runs out of time
        // destroy the trap

        if (currentTime <= 0.0f)
        {
            Destroy(gameObject);
        }

        // If the trapped timer runs out
        // release the trapped animal and set their
        // approriate values back to their original setting
        // then destroy the trap

        if (timeAnimalIsTrapped <= 0.0f)
        {
            trappedAnimal.GetComponent<NavMeshAgent>().speed = tempAnimalMoveSpeed;
            trappedAnimal.GetComponent<NavMeshAgent>().acceleration = tempAnimalAccellSpeed;
            trappedAnimal.GetComponent<NavMeshAgent>().velocity = tempAnimalVelocity;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Checks if an animal has collided with the trap
        // and the trap is not already active

        if (collision.gameObject.tag == "Animal" && !isTrapAlive)
        {
            // Received the animal behaviour object - OBSELETE CODE
            trappedAnimalBehaviour = collision.gameObject.transform.parent.GetChild(1).gameObject;

            // Saved the collided object for ease of access
            trappedAnimal = collision.gameObject;

            // Set position of trapped animal to centre of trap and slightly raised above to avoid glitches
            trappedAnimal.gameObject.transform.position = new Vector3(transform.position.x, trappedAnimal.gameObject.transform.position.y + 1.0f,transform.position.z);

            // temporarily saved the speed, acceleration and velocity values for resetting later
            tempAnimalMoveSpeed = trappedAnimal.GetComponent<NavMeshAgent>().speed;
            tempAnimalAccellSpeed = trappedAnimal.GetComponent<NavMeshAgent>().acceleration;
            tempAnimalVelocity = trappedAnimal.GetComponent<NavMeshAgent>().velocity;

            // set the same values to zero, imitating a trapped animal
            trappedAnimal.GetComponent<NavMeshAgent>().speed = 0.0f;
            trappedAnimal.GetComponent<NavMeshAgent>().acceleration = 0.0f;
            trappedAnimal.GetComponent<NavMeshAgent>().velocity = Vector3.zero;

            // set the trap active
            isTrapAlive = true;
            


        }
    }
}
