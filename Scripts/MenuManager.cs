using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//enum for menu
public enum MenuState {NewGame,LoadGame,Options,Quit}

public class MenuManager : MonoBehaviour
{
    //input
    MenuControls input;
    //menu state 
    public MenuState menuState;
    //UI text
    public Text NewGame;
    public Text LoadGame;
    public Text Options;
    public Text Quit;
    //text coolours
    Color32 selectedColour;
    Color32 deselectedColour;
    //check if clip is playing
    private bool clipPlaying = true;
    //audio objects
    public AudioClip[] menuClips = new AudioClip[2];
    public AudioSource menuClipsSource;

    private void Awake()
    {
        //set input
        input = new MenuControls();

        //input for moving the option up
        input.Newactionmap.Moveup.performed += ctx => SelectUp();
        //input for moving option down
        input.Newactionmap.MoveDown.performed += ctx => SelectDown();
        //input for selecting option
        input.Newactionmap.Select.performed += ctx => Select();
        //input not used yet
        input.Newactionmap.Back.performed += ctx => Back();
    }

    //enable input
    private void OnEnable()
    {
        input.Newactionmap.Enable();
    }

    //disable input
    private void OnDisable()
    {
        input.Newactionmap.Disable();
    }

    //set values
    void Start()
    {
        selectedColour = new Color32(0x90,0xE4,0xF1,0xFF);
        deselectedColour = new Color32(0xFB,0xEB,0xC7,0xFF);
        menuState = MenuState.NewGame;
        Time.timeScale = 1f;
    }
    
    void Update()
    {
        //changes text colour based on state
        switch (menuState)
        {
            case MenuState.NewGame:
                NewGame.color = selectedColour;
                LoadGame.color = deselectedColour;
                Options.color = deselectedColour;
                Quit.color = deselectedColour;
                break;
            case MenuState.LoadGame:
                NewGame.color = deselectedColour;
                LoadGame.color = selectedColour;
                Options.color = deselectedColour;
                Quit.color = deselectedColour;
                break;
            case MenuState.Options:
                NewGame.color = deselectedColour;
                LoadGame.color = deselectedColour;
                Options.color = selectedColour;
                Quit.color = deselectedColour;
                break;
            case MenuState.Quit:
                NewGame.color = deselectedColour;
                LoadGame.color = deselectedColour;
                Options.color = deselectedColour;
                Quit.color = selectedColour;
                break;
        }
    }


    void SelectUp()
    {
        //stop clip
        menuClipsSource.Stop();
        clipPlaying = true;
        if (clipPlaying)
        {
            //play sound clip
            //menuClipsSource.PlayOneShot(menuClips[0]);
            menuClipsSource.clip = menuClips[1];
            menuClipsSource.PlayScheduled(0.9);
        }
        clipPlaying = false;
        //switch state based on previous state
        switch (menuState)
        {
            case MenuState.NewGame:
                menuState = MenuState.Quit;
                break;
            case MenuState.LoadGame:
                break;
            case MenuState.Options:
                break;
            case MenuState.Quit:
                menuState = MenuState.Options;
                break;
        }
    }

    void SelectDown()
    {
        //stop clip
        menuClipsSource.Stop();
        clipPlaying = true;
        if (clipPlaying)
        {
            //play menu sound
            //menuClipsSource.PlayOneShot(menuClips[1]);
            menuClipsSource.clip = menuClips[1];
            menuClipsSource.PlayScheduled(0.9);
        }
        clipPlaying = false;
        //switch state based on previous state
        switch (menuState)
        {
            case MenuState.NewGame:
                menuState = MenuState.LoadGame;
                break;
            case MenuState.LoadGame:
                menuState = MenuState.Options;
                break;
            case MenuState.Options:
                menuState = MenuState.Quit;
                break;
            case MenuState.Quit:
                menuState = MenuState.NewGame;
                break;
        }
    }

    void Select()
    {
        //stop clip
        menuClipsSource.Stop();
        clipPlaying = true;
        if (clipPlaying)
        {
            //play menu sound
            //menuClipsSource.PlayOneShot(menuClips[1]);
            menuClipsSource.clip = menuClips[0];
            menuClipsSource.PlayScheduled(0.9);
        }
        clipPlaying = false;

        //call action based on state
        switch (menuState)
        {
            case MenuState.NewGame:
                SceneManager.LoadScene("Loading Scene", 0);
                break;
            case MenuState.LoadGame:
                menuState = MenuState.NewGame;
                break;
            case MenuState.Options:
                menuState = MenuState.LoadGame;
                break;
            case MenuState.Quit:
                Application.Quit();
                break;
        }

    }

    //function not used
    void Back()
    {

    }

    

}
