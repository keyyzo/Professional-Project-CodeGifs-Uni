using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject tutorialMusic;
    public GameObject levelMusic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == tutorialMusic && !tutorialMusic.GetComponent<AudioSource>().isPlaying)
        {
            levelMusic.GetComponent<AudioSource>().Stop();
            tutorialMusic.GetComponent<AudioSource>().Play();
        }

        if (collision.gameObject == levelMusic && !levelMusic.GetComponent<AudioSource>().isPlaying)
        {
            tutorialMusic.GetComponent<AudioSource>().Stop();
            levelMusic.GetComponent<AudioSource>().Play();
        }
    }
}
