using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Trap Shot script

// Shoots a trap object into the air, which can be
// controlled with how close or far the trap shoots in front
// of the player

public class TrapShot : MonoBehaviour
{
    private Animator animator;
    private PlayerControls input;
    public GameObject trapObj;
    public Transform shotPoint;
    public float shotForce = 2.0f;
    public float rotSpeed = 1.0f;
    public float rotSpeedX = 0.0f;
    public float rotSpeedY = 0.0f;
    private float aimVal = 1.0f;
    private float aimValForward = 1.0f;
    private float aimValBackward = -1.0f;
    private bool actionCompleted;
    private Vector2 stickAiming;
    private Vector3 aimVec3;
    private bool aimingTrap = false;
    private bool aimingTrapForward = false;
    private bool aimingTrapBackward = false;
    private float aimValue = 5.0f;
    public int maxShotsAvailable = 4;

    public AudioClip trapShotClip;
    public AudioSource trapShotSource;

    private bool clipPlaying = true;

    private GameObject[] laidTraps;

    // Activates the player inputs
    // with the PlayerControls when enabled

    private void Awake()
    {
        input = new PlayerControls();
        
        input.Player.ActionButton.performed += ctx => SpawnTrap();
        input.Player.ForwardTrap.performed += ctx => aimingTrapForward = true;
        input.Player.ForwardTrap.canceled += ctx => aimingTrapForward = false;
        input.Player.BackwardTrap.performed += ctx => aimingTrapBackward = true;
        input.Player.BackwardTrap.canceled += ctx => aimingTrapBackward = false;

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

    // Start is called before the first frame update
    void Start()
    {
        // Setting animation component to variable

        animator = gameObject.GetComponent<Animator>();

        // Sets whether trap can shoot or not 

        actionCompleted = true;
    }

    // Update is called once per frame
    void Update()
    {
        // moves aim of trap forward
        // if forward button is down

        if (aimingTrapForward)
        {
            aimValue += 1.0f * rotSpeed * Time.deltaTime;
            shotForce += 1.0f * rotSpeed * Time.deltaTime;
        }

        // moves aim of trap backward
        // if backward button is down

        if (aimingTrapBackward)
        {
            aimValue -= 1.0f * rotSpeed * Time.deltaTime;
           shotForce -= 1.0f * rotSpeed * Time.deltaTime;
        }
        
        // moves trap aim forward, maxing out at a hard coded value

        if (aimingTrapForward && (aimValue >= 5.0f && aimValue <= 25.0f))
        {

          transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + (1.0f * rotSpeed * Time.deltaTime), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }

        // moves trap aim backward, maxing out at a hard coded value

        if (aimingTrapBackward && (aimValue >= 5.0f && aimValue <= 25.0f))
        {

           transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + (-1.0f * rotSpeed * Time.deltaTime), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }

        // Locks aim distance
        // and shot force values
        // to keep the aiming consistent
        // and not go over the max and min values

        if (aimValue >= 25.0f)
        {
            aimValue = 25.0f;
        }

        if (aimValue <= 5.0f)
        {
            aimValue = 5.0f;
        }

        if (shotForce >= 25.0f)
        {
            shotForce = 25.0f;
        }

        if (shotForce <= 5.0f)
        {
            shotForce = 5.0f;
        }

   
    }

    private void FixedUpdate()
    {
        // Adds any laid traps from the scene to the array
        // helps with controlling whether or not the trap shot
        // can shoot again

        laidTraps = GameObject.FindGameObjectsWithTag("Trap");
    }


    // Carries out the shooting of the trap object

    public void SpawnTrap()
    {
        // Checks whether the game has been paused

        if(FindObjectOfType<Pause_script>().pauseState != PauseState.Paused)
        {
            // checks if the cannon is ready to shoot and if there are less
            // than max shots within the scene already

            if (actionCompleted && laidTraps.Length < maxShotsAvailable)
            {
                // Plays the cannon shot clip

                if (clipPlaying)
                {
                    trapShotSource.PlayOneShot(trapShotClip);
                }
                clipPlaying = false;

                // Disables the cannon for the animation to play

                actionCompleted = false;

                // Spawns a trap object and shoots it forward,
                // following the predicted aim path

                GameObject spawnTrapObj = Instantiate(trapObj, shotPoint.position, shotPoint.rotation);
                spawnTrapObj.GetComponent<Rigidbody>().velocity = shotPoint.transform.up * shotForce;

                // Plays the shoot animation

                animator.SetTrigger("Shoot");
            }
        }        
    }

    // Resets the cannon, allowing for it to shoot a trap again

    void AnimationDone()
    {
        actionCompleted = true;
        clipPlaying = true;
    }
    
    // Plays the cannon despawn animation
    // as the cannon is disabled

    void TrapDespawned()
    {
        FindObjectOfType<Item_Script>().DespawnFinished();
    }

    // Plays the cannon spawn animation
    // as the cannon is enabled

    void TrapSpawned()
    {
        FindObjectOfType<Item_Script>().ToolEquiped();
    }

    // Plays the bag closed animation

    void BagClosed()
    {
        FindObjectOfType<Item_Script>().ToolEquiped();
    }
   
}
