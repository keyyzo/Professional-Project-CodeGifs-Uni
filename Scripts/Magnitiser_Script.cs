using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Magnetiser Script

// Finds and aims to the closest scrap to the magnetiser
// then shoots out a charge to create a trash boulder

public class Magnitiser_Script : MonoBehaviour
{
    private PlayerControls input;
    public float chargeSpawnReach = 5.0f;

    public Transform target;
    List<GameObject> scrapsList = new List<GameObject>();
    public GameObject closestScrap;
    public GameObject boulder;
    public float distance;
    public float maxDistance = 200.0f;
    private bool ifTargetIsAvailable = false;
    private bool shootCharge = false;
    private bool boulderSet = false;
    Animator animator;
    public int chargeLimit = 1;
    private int chargesUsed = 0;

    public AudioClip magnetClip;
    public AudioSource magnetClipSource;

    private bool clipPlaying = true;

    // Activates the player inputs
    // with the PlayerControls when enabled

    private void Awake()
    {
        input = new PlayerControls();
        input.Player.ActionButton.started += ctx => shootCharge = ctx.ReadValueAsButton();

    }

    // Enables and disables input from player

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }

    void Start()
    {
        // Setting animation component to variable

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // Clearing the list every frame to avoid errors

        scrapsList.Clear();

        // Finds all smaller scrap within the scene and saves to an array

        GameObject[] scrapInScene = GameObject.FindGameObjectsWithTag("Scrap");

        // adds each scrap within the array to the list
        foreach (GameObject scrap in scrapInScene)
        {
            scrapsList.Add(scrap);
        }

        // calls function to find and aim to 
        // closest scrap

        ClosestScrap();

        // Check if shooting the magnetiser
        if (shootCharge )
        {
            // if there is scrap close by and the magnetiser has charges left,
            // shoots the charge and increases charge use

            if (scrapsList.Count > 0 && chargesUsed < chargeLimit)
            {
                SpawnCharge();
                chargesUsed++;
                clipPlaying = true;
            }

            // resets shot to be able to shoot again
            else
            {
                shootCharge = false;
                print("charge limit reached");
            }
        }


        // if all charges used and boulder created
        // point magnetiser towards the trash boulder
        if(chargesUsed == chargeLimit)
        {
            if(!boulderSet)
            {
                boulder = FindObjectOfType<BoulderTest>().gameObject;
            }
            gameObject.transform.LookAt(boulder.transform.position);
            transform.Rotate(0, 180, 0);
        }


    }


    // Shoots the charge from the magnetiser
    public void SpawnCharge()
    {
        // checks if game is paused

        if(FindObjectOfType<Pause_script>().pauseState != PauseState.Paused)
        {

            // plays the sound effect
            if (clipPlaying)
            {
                magnetClipSource.PlayOneShot(magnetClip);
            }
            clipPlaying = false;

            // points magnetiser to closest scrap
            transform.LookAt(closestScrap.transform);

            // sets closest scrap as magnetised
            closestScrap.GetComponent<ScrapTest>().isMagnetized = true;

            // plays the shoot animation
            animator.SetTrigger("Magnet_Shoot");

            // resets shoot capabilites
            shootCharge = false;
        }        
    }

    // finds all scrap within the scene
    // and points towards the closest scrap avaiable

    public void ClosestScrap()
    {
        // sets range to be used for finding closest scrap

        float range = maxDistance;



        foreach (GameObject scrapObject in scrapsList)
        {
            // if the scrap object doesnt exist,
            // remove from list and destroy the object

            if (scrapObject == null)
            {
                scrapsList.Remove(scrapObject);
                scrapObject.GetComponent<ScrapTest>().Die();
            }

            // determine the closest scrap by finding the distance between
            // each scrap and the player, resetting the distance as the player moves
            // to acommodate moving around the map

            else
            {
               float dist = Vector3.Distance(scrapObject.transform.position, transform.position);
            
               if (dist < range && !scrapObject.GetComponent<ScrapTest>().isScrapMagnetized())
               {
                
                     range = dist;
                     closestScrap = scrapObject;

               }     
            }

      
        }

    }

    // Plays the magnetiser despawn animation
    void MagDespawned()
    {
        transform.Rotate(Vector3.forward);
        FindObjectOfType<Item_Script>().DespawnFinished();
    }
    
    // Plays the magnetiser spawn animation
    void MagSpawned()
    {
        FindObjectOfType<Item_Script>().ToolEquiped();
    }

}
