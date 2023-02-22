using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Loading Scene script

// Showcases an image on the screen as the 
// main level scene loads in the background

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private Image progressBar;

    // Start is called before the first frame update
    void Start()
    {
        // Calls the function as a Coroutine

        StartCoroutine(LoadAsyncOperation());
    }

    // Loads the level scene in the background, showcasing 
    // and updating the progress of the loading on an image using
    // the fill amount

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(1);

        while (gameLevel.progress < 1)
        {
            progressBar.fillAmount = gameLevel.progress;
            yield return new WaitForSeconds(2);
        }

        
    }
}
