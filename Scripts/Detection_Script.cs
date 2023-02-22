using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Detection_Script : MonoBehaviour
{
    //game objects
    public GameObject animalTriggered;
    public GameObject animalBehaviour;
    
    //game object arrays
    public GameObject[] animalTriggeredArray = new GameObject[12];
    public GameObject[] animalBehaviourArray = new GameObject[12];

    //float values
    public float animalSpeed;
    public float timer;
    public float triggerTime;
    
    //float arrays
    public float[] animalSpeedArray = new float[12];
    public float[] timerArray = new float[12];

    //boolean array
    bool[] triggerStart = new bool[12];


    //initialise starting values and set arrays to null
    void Start()
    {
        
        triggerTime = 15.0f;
        for(int i = 0; i < 12; i++)
        {
            timerArray[i] = 0;
            animalSpeedArray[i] = 0;
            animalBehaviourArray[i] = null;
            triggerStart[i] = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
            //loop through all the arrays
            for(int i = 0; i < 12; i++)
            {
                //if the index boolean array is true
                if (triggerStart[i])
                {
                    // if the index timer array is greater than 0 and behaviour isnt null
                    if (timerArray[i] > 0.0f && animalBehaviourArray[i] != null)
                    {
                        //count down timer
                        timerArray[i] -= 1.0f * Time.deltaTime;
                    }

                    //if timer is 0 or less and behaviour isnt null set speed bback to normal and set arrays back to origional values
                    if (timerArray[i] <= 0 && animalBehaviourArray[i] != null)
                    {
                        //animalBehaviourArray[i].GetComponent<RandomMovementNavmesh>().moveSpeed = animalSpeedArray[i] / 2;
                        animalBehaviourArray[i].GetComponent<NavMeshAgent>().speed = animalSpeedArray[i];
                        timerArray[i] = 0;
                        animalBehaviourArray[i] = null;
                        animalSpeedArray[i] = 0;
                        triggerStart[i] = false;
                    }

                    //if array is null set values back to 0
                    if (animalBehaviourArray[i] == null)
                    {
                        animalSpeedArray[i] = 0;
                        timerArray[i] = 0;
                    }
                }
            }      
    }

    //trigger check
    private void OnTriggerEnter(Collider animalcollider)
    {
        //if trigger is animal
        if(animalcollider.tag == "Animal")
        {
            //obtain animal
            animalTriggered = animalcollider.transform.parent.gameObject;


            for(int i = 0; i < 12; i++)
            {
                //check what animal was obtained and obtain its navmesh agent, set animal speed and timer
                if (animalTriggered.name == animalTriggeredArray[i].name && triggerStart[i] == false)
                {

                //    animalTriggeredArray[i] = animalTriggered;
                    //animalBehaviour = animalTriggered.transform.GetChild(1).gameObject;
                    animalBehaviour = animalTriggered.transform.GetChild(0).gameObject;
                    animalBehaviourArray[i] = animalBehaviour;
                    //animalSpeed = animalBehaviour.GetComponent<RandomMovementNavmesh>().moveSpeed;
                    animalSpeed = animalBehaviour.GetComponent<NavMeshAgent>().speed;
                    animalSpeedArray[i] = animalSpeed;
                    //animalBehaviour.GetComponent<RandomMovementNavmesh>().moveSpeed = animalSpeed * 2;
                    animalBehaviour.GetComponent<NavMeshAgent>().speed = animalSpeed * 2;
                    triggerStart[i] = true;
                    timerArray[i] = triggerTime;
                }
            }

            //code not used
            //animalTriggeredList.Add(animalTriggered);
            //animalBehaviour = animalTriggered.transform.GetChild(1).gameObject;
            //animalBehaviourList.Add(animalBehaviour);
            //animalSpeed = animalBehaviour.GetComponent<RandomMovementNavmesh>().moveSpeed;
            //animalSpeedList.Add(animalSpeed);
            //animalBehaviour.GetComponent<RandomMovementNavmesh>().moveSpeed = animalSpeed * 2;
            //triggerStart = true;
        }
        
    }
}
