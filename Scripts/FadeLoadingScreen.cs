using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Fade Loading Screen Script

// Checks to see if the player character has fully
// spawned in and set up before carrying out the screen transition.
// Hides the objects spawning in the scene as well

public class FadeLoadingScreen : MonoBehaviour
{
    public Image img;
    float timer = 3.0f;
    private bool done = false;
    private GameObject playerSpawn;

    // Start is called before the first frame update
    void Start()
    {
        // Finds the player object and sets
        // the gameobject to the player object

        playerSpawn = GameObject.Find("Spawn Point");
        
    }

    // Update is called once per frame
    void Update()
    {

        // Checks if the CharacterSpawn has finished loading within the scene

        if (playerSpawn.GetComponent<CharacterSpawn>().isAddComponentsFinished())
        {
            // Starts the timer before carrying out the
            // transition on the image

            timer -= 1.0f * Time.deltaTime;

            if (timer <= 0.0f && !done)
            {
                // Image transition, reducing the fill
                // amount with the effect chosen from in-engine

                img.fillAmount -= 1.0f * Time.deltaTime;

                if (img.fillAmount == 0)
                {
                    done = true;
                }
            }
        }

        
        
    }
}
