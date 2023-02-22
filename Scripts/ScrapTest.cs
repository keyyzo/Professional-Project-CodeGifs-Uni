using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Scrap Test script

// controls the state of the scrap, whether it is magnetised, 
// being added to a trash boulder or making it avoid the animals
// within the scene

public class ScrapTest : MonoBehaviour
{

    public bool isMagnetized;
    public float magnetSpeed = 10.0f;
    public float magnetDistance = 10.0f;
    private int scrapCount;

    public GameObject boulder;
    public float currentTime = 0.0f;
    public float timeMagnetized = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Set the scrap count to zero
        // and all scrap unmagnetised on spawn

        scrapCount = 0;
        isMagnetized = false;

        // Set the timer to a predetermined
        // value set in-engine
        currentTime = timeMagnetized;

        // Saves all the animals in the scene to the array
        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");

        // Ignores the collisions between all scrap within the scene
        // and all animals within the scene
        foreach (GameObject animal in animals)
        {
            Physics.IgnoreCollision(animal.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

    // Update is called once per frame
    void Update()
    {

        // checks if scrap has been magnetised

        if (isMagnetized)
        {
            print(isMagnetized);

            // starts the timer on how long the scrap
            // is magnetised for

            currentTime -= 1.0f * Time.deltaTime;

            // Plays the magnetised sound effect

            GetComponent<AudioSource>().Play();

            // Checks for other surrounding scrap within a set sized radius

            Collider[] scrapColliders = Physics.OverlapSphere(gameObject.transform.position, magnetDistance);

            for (int i = 0; i < scrapColliders.Length; i++)
            {
                // if the scrap is close enough,
                // draw the scrap towards the magnetised scrap

                if (scrapColliders[i].tag == "Scrap")
                {
                    Transform scrap = scrapColliders[i].transform;
                    scrap.position = Vector3.MoveTowards(scrap.position, gameObject.transform.position, magnetSpeed * Time.deltaTime);
                }
            }

            // if the scrap attracts 4 or more
            // other pieces of scrap, spawn
            // the trash boulder and destroy the magnetised scrap

            if (scrapCount >= 4)
            {
                SpawnBoulder();
                Destroy(gameObject);
            }
            
        }

        // if the magnetised scrap timer runs out
        // stop the sound effect
        // and unmagnetise the scrap, setting it up
        // to be magnetised again if shot at

        if (currentTime <= 0.0f)
        {
            isMagnetized = false;
            GetComponent<AudioSource>().Stop();
            currentTime = timeMagnetized;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        // If colliding with other scrap while magnetised,
        // destroy the colliding scrap and adding to the scrap count

        if (collision.gameObject.tag == "Scrap" && isMagnetized)
        {
            scrapCount++;
            Destroy(collision.gameObject);
        }

        // If colliding with the trash boulder,
        // destroy the scrap

        if (collision.gameObject.tag == "Boulder" && collision.gameObject.GetComponent<Rigidbody>().mass >= gameObject.GetComponent<Rigidbody>().mass)
        {
            Destroy(gameObject);
        }

        // If colliding with any of the animals,
        // the collisions are ignored so the animal can walk through the scrap

        if (collision.gameObject.tag == "Animal")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }

    }

    // if the charge hits the scrap
    // magnetise the scrap and destroy the charge

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Charge")
        {
            isMagnetized = true;
            Destroy(other.gameObject);
        }
    }

    // spawns the trash boulder at the magnetised scrap position

    public void SpawnBoulder()
    {
        Instantiate(boulder, transform.position, transform.rotation);
    }

    // function to destroy the scrap

    public void Die()
    {
        Destroy(gameObject);
    }

    // function to check whether the scrap is magnetised or not

    public bool isScrapMagnetized()
    {
        return isMagnetized;
    }
}
