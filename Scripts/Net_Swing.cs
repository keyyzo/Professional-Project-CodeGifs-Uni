using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Net_Swing : MonoBehaviour
{
    //player input
    private PlayerControls input;

    //gameobjects
    private GameObject hoop;
    private GameObject trail;

    //boolean values
    public bool readyToUse = false;
    private bool clipPlaying = true;
    private bool actionCompleted = true;

    //audio objects
    public AudioClip netClip;
    public AudioSource netClipSource;

    private void Awake()
    {
        //set input
        input = new PlayerControls();
        
        //perform net action
        input.Player.ActionButton.performed += ctx => PerformAction();

    }

    
    
    //enbale input
    private void OnEnable()
    {
        input.Player.Enable();
    }

    //disable input
    private void OnDisable()
    {
        input.Player.Disable();
    }

    
    //set values
    void Start()
    {
        
        hoop = gameObject.transform.FindChild("net_global_control").FindChild("Net_GEO").FindChild("Net_Top").GetChild(0).gameObject;
        trail = gameObject.transform.FindChild("net_global_control").FindChild("Net_GEO").FindChild("Net_Top").GetChild(0).GetChild(0).gameObject;
        trail.SetActive(false);
        hoop.GetComponent<CapsuleCollider>().enabled = false;
    }


    public void PerformAction()
    {
        //if game isnt paused enable sound and particle effect then perform action
        if(FindObjectOfType<Pause_script>().pauseState != PauseState.Paused && readyToUse)
        {
            if (actionCompleted)
            {
                if (clipPlaying)
                {
                    netClipSource.PlayOneShot(netClip);
                }
                clipPlaying = false;

                GetComponent<Animator>().SetTrigger("Base_Swing");
                actionCompleted = false;
                hoop.GetComponent<CapsuleCollider>().enabled = true;
                trail.SetActive(true);
                //GetComponent<AudioSource>().Play();
            }
        }        
    }

    //if animation is done rest values so player can swing again
    public void AnimationDone()
    {
        clipPlaying = true;
        actionCompleted = true;
        hoop.GetComponent<CapsuleCollider>().enabled = false;
        trail.SetActive(false);
    }

    //if net collides with animal make it dissapear and add 1 to counter
    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Animal")
        {
            Destroy(col.gameObject);
            FindObjectOfType<GameManager>().IncreaseCaught();
        }
    }

    //if net is despawned call item script function
    void NetDespawned()
    {
        FindObjectOfType<Item_Script>().DespawnFinished();
    }

    //if net has spawned call item script function
    void NetSpawned()
    {
        FindObjectOfType<Item_Script>().ToolEquiped();
        readyToUse = true;
    }

   

}
