using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Boulder Test script

// Controls the values at which the mass of the boulder,
// the scrap finding range and the scale of the boulder 
// is affected when rolling around collecting scrap

public class BoulderTest : MonoBehaviour
{
    Rigidbody boulderRigidBody;
    CapsuleCollider boulderCapsuleCollider;
    float scaleVal;
    Vector3 scaleChange;
    [SerializeField] float scaleDivider = 50f;
    [SerializeField] float massDivider = 10.0f;

    public float magnetSpeed = 10.0f;
    public float magnetDistance = 10.0f;
    public float boulderStartForce;
    public float effectDivider = 1000;


    // Start is called before the first frame update
    void Start()
    {
        // Set components to their specific variables

        boulderCapsuleCollider = GetComponent<CapsuleCollider>();
        boulderRigidBody = GetComponent<Rigidbody>();

        // Set starting mass
        boulderRigidBody.mass = 10.0f;
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Creates an overlap sphere that will detect each piece of scrap within a radius

        Collider[] scrapColliders = Physics.OverlapSphere(gameObject.transform.position, magnetDistance);

        for (int i = 0; i < scrapColliders.Length; i++)
        {
            // If the scrap has less mass than the boulder, big or small, will get drawn towards the boulder
            // until eventually it becomes part of the boulder
            // Scrap drawn speed decided by in-engine variables

            if ((scrapColliders[i].tag == "Scrap" || scrapColliders[i].tag == "BigScrap") && scrapColliders[i].gameObject.GetComponent<Rigidbody>().mass <= boulderRigidBody.mass)
            {
                Transform scrap = scrapColliders[i].transform;
                scrap.position = Vector3.MoveTowards(scrap.position, gameObject.transform.position, magnetSpeed * Time.deltaTime);
            }
        }

        // Allows the game manager to receive the current
        // mass of the boulder to set the game completion goals

        FindObjectOfType<GameManager>().ObtainMass(boulderRigidBody.mass);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Checks if the collided scrap has less mass than the boulder, big or small

        if ((collision.gameObject.tag == "Scrap" || collision.gameObject.tag == "BigScrap") && collision.gameObject.GetComponent<Rigidbody>().mass <= boulderRigidBody.mass)
        {
            // Adds the scrap mass to the boulder mass, with a divider
            // for balancing the mechanic

            boulderRigidBody.mass += (collision.gameObject.GetComponent<Rigidbody>().mass / massDivider);

            // obtains a scale value by setting the scrap mass
            // as each scale component
            scaleVal = collision.gameObject.GetComponent<Rigidbody>().mass;
            scaleChange = new Vector3(scaleVal, scaleVal, scaleVal);

            // scales the boulder up based on the scale value from the scrap,
            // with a divider for balancing
            transform.localScale += (scaleChange / scaleDivider);

            // Increases the scrap detection radius based on the 
            // obtained scale value with the scale divider
            magnetDistance += (scaleVal / scaleDivider);

            // scales up the boulder particles with a hard coded value
            transform.GetChild(0).gameObject.transform.localScale += new Vector3(0.002f,0.002f,0.002f);

            Debug.Log("Boulder has grew in size and mass!");
        }
    }
}
