using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject options;
    public GameObject controls;
    public GameObject mainButtons;

    public Button playBtn;
    public Button optionsBtn;
    public Button controlsBtn;
    public Button exitBtn;

    public Button optionsExitBtn;
    public Button controlsExitBtn;

    void Start()
    {
        options.SetActive(false);
        controls.SetActive(false);

        playBtn.onClick.AddListener(PlayGame);
        optionsBtn.onClick.AddListener(DisplayOptions);
        controlsBtn.onClick.AddListener(DisplayControls);
        exitBtn.onClick.AddListener(ExitGame);

        optionsExitBtn.onClick.AddListener(MainMenu);
        controlsExitBtn.onClick.AddListener(MainMenu);
    }

    void PlayGame()
    {
        //this is where you switch scenes to play game
    }

    void DisplayOptions()
    {
        options.SetActive(true);
        mainButtons.SetActive(false);
    }

    void DisplayControls()
    {
        controls.SetActive(true);
        mainButtons.SetActive(false);
    }

    void ExitGame()
    {
        //exit game
    }

    void MainMenu()
    {
        mainButtons.SetActive(true);

        if (controls)
        {
            controls.SetActive(false);
        }
        else
        {
            options.SetActive(false);
        }
    }
}
