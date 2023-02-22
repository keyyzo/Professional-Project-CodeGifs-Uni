using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    //player input
    PlayerControls input;
    //character controller
    private CharacterController controller;
    //camera transform 
    public Transform cam;
    //game objects
    public GameObject boulder;
    public GameObject dirtTrail;
    public GameObject tutorialMusic;
    public GameObject levelMusic;
    //vector 2 values
    private Vector2 playerMomentumXZ;
    private Vector2 stickMovement;
    //vector 3 values
    private Vector3 playerVelocity;
    //float values
    public float playerSpeed = 6f;
    public float sneakSpeed = 2f;
    public float jumpSpeed = 10f;
    private float directionY = 0;
    public float playerMass = 10.0f;
    public float boulderSpeed = 2.0f;
    public float boulderSpeedDivider = 50.0f;
    private float turnSmoothTime = 0.1f;
    private float smoothTurnVelocity;
    private float initialSpeed;
    private float gravityValue = 20f;
    //boolean values
    private bool groundedPlayer;
    private bool jumpPressed = false;
    private bool sneakActive = false;
    private bool clipPlaying = true;
    //audio objects
    public AudioClip walkingClip;
    public AudioSource walkingClipSource;


    //awake function used for input
    private void Awake()
    {
        //set input
        input = new PlayerControls();

        //if stick or WASD is used set value to stick movement
        input.Player.Movement.performed += ctx => stickMovement = ctx.ReadValue<Vector2>();

        input.Player.Jump.performed += ctx => jumpPressed = ctx.ReadValueAsButton();

        input.Player.Sneak.performed += ctx => sneakActive = ctx.ReadValueAsButton();
        input.Player.Sneak.canceled += ctx => sneakActive = false;
        
    }

    //initalise values
    private void Start()
    {
        initialSpeed = playerSpeed;
        walkingClipSource.clip = walkingClip;
        walkingClipSource.loop = true;
    }

    //enable input
    private void OnEnable()
    {
        input.Player.Enable();
    }

    //disable input
    private void OnDisable()
    {
        input.Player.Disable();
    }

    //update function
    void Update()
    {
        //if player controller doesnt exist yet
        if(controller == null)
        {
            //if character controller on object as a component
            if (gameObject.GetComponent<CharacterController>())
            {
                //set controller to character controller
                controller = gameObject.GetComponent<CharacterController>();
            }
            else
            {
                return;
            }
        }
        
        //set player velocities
        playerVelocity = new Vector3(stickMovement.x, 0.0f, stickMovement.y).normalized;

        //if player is moving
        if (stickMovement != Vector2.zero)
        {
            //if sound not playing 
            if (!walkingClipSource.isPlaying)
            {
                //play sound
                walkingClipSource.Play();
            }
            //if player not in the air
            if (playerVelocity.y == 0)
            {

                //turn the player in the moving direction based on camera forward
                float targetAngle = Mathf.Atan2(playerVelocity.x, playerVelocity.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                playerVelocity = moveDir;
            }
            else
            {
                //stop walking sound and do final player rotation
                walkingClipSource.Stop();
                float targetAngle = Mathf.Atan2(playerVelocity.x, playerVelocity.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                moveDir.y = playerVelocity.y;
                playerVelocity = moveDir;
            }

        }
        else
        {
            //stop walking sound set velocity to 0
            walkingClipSource.Stop();
            playerVelocity = Vector3.zero;
        }


        //set groundedplayer to check that if theyre of the ground
        groundedPlayer = controller.isGrounded;
        
        //if the player is in the air
        if (!groundedPlayer || directionY > 0.0f)
        {
            //apply gravity
            directionY -= gravityValue * Time.deltaTime;
            //st
            walkingClipSource.Stop();

        }
        //set player velocity to gravity
        playerVelocity.y = directionY;

        //if player is on ground
        if (groundedPlayer)
        {
            //activate particle effect
            dirtTrail.SetActive(true);
            //allow player to jump
            if (jumpPressed)
            {
                Jump();
            }
            
        }
        else
        {
            //dectivate particle effect
            dirtTrail.SetActive(false);
        }


        //old values no longer used
        //--------------------------------------------------
        //playerVelocity.x = playerVelocity.x * playerSpeed;
        //playerVelocity.z = playerVelocity.z * playerSpeed;

        //playerMomentumXZ.x = playerVelocity.x * playerMass;
        //playerMomentumXZ.y = playerVelocity.z * playerMass;
        //---------------------------------------------------

        //if player is sneaking
        if (sneakActive)
        {
            //slow player
            playerSpeed = sneakSpeed;
            //lower sound volume
            walkingClipSource.volume = 0.3f;
            //disable detection sphere
            transform.GetChild(1).GetComponent<SphereCollider>().enabled = false;
        }

        else
        {
            //set speed back to normal 
            playerSpeed = initialSpeed;
            // set sound volume
            walkingClipSource.volume = 0.5f;
            //enable detection sphere
            transform.GetChild(1).GetComponent<SphereCollider>().enabled = true;
        }

        Vector3 playerMoveVec = new Vector3(playerVelocity.x * playerSpeed,  playerVelocity.y, playerVelocity.z * playerSpeed);
        //controller.Move(playerVelocity * Time.deltaTime);
        controller.Move(playerMoveVec * Time.deltaTime);

    }


    void Jump()
    {
        //set players speed upwards
        directionY = jumpSpeed;
        //disable particle effect
        dirtTrail.SetActive(false);
    }

    //collision checks
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //if player touches ground
        if (hit.gameObject.tag == "ground" && jumpPressed == true)
        {
            //set jump back to false
            jumpPressed = false;
        }

        //if player hit boulder
        if (hit.gameObject.tag == "Boulder")
        {
            //set player to boulder speed
            playerSpeed = boulderSpeed;

            //set boulder x and z velocities
            float boulderVelocityX = playerMomentumXZ.x / hit.gameObject.GetComponent<Rigidbody>().mass;
            float boulderVelocityZ = playerMomentumXZ.y / hit.gameObject.GetComponent<Rigidbody>().mass;

            //if boulder mass is less or the same as player mass
            if (hit.gameObject.GetComponent<Rigidbody>().mass <= (playerSpeed * playerMass) / boulderSpeed)
            {
                //set boulder velocity
                Vector3 dir = (gameObject.transform.position - hit.gameObject.transform.position).normalized;

               // hit.gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * boulderSpeed;
                hit.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(playerVelocity.x,0.0f,playerVelocity.z) * boulderSpeed; //new Vector3(boulderVelocityX, 0.0f, boulderVelocityZ) * 
                
            }

            else
            {
                //starm making the boulder slower
                hit.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(playerVelocity.x, 0.0f, playerVelocity.z) * (boulderSpeed / boulderSpeedDivider);
            }
        }
    }

    //trigger check
    private void OnTriggerEnter(Collider collision)
    {
        //if player hits tutorial music trigger
        if (collision.gameObject == tutorialMusic && !tutorialMusic.GetComponent<AudioSource>().isPlaying)
        {
            //play tutorial music
            levelMusic.GetComponent<AudioSource>().Stop();
            tutorialMusic.GetComponent<AudioSource>().Play();
        }

        //if player hits level trigger
        if (collision.gameObject == levelMusic && !levelMusic.GetComponent<AudioSource>().isPlaying)
        {
            //play level music
            tutorialMusic.GetComponent<AudioSource>().Stop();
            levelMusic.GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if player leaves boulder reset speed
        if (other.gameObject.tag == "Boulder")
        {
            playerSpeed = initialSpeed;
        }
    }


}
