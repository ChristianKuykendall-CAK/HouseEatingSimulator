using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    void Awake()
    {
        Button playButton = GetComponent("PlayButton") as Button;
        Button optionsButton = GetComponent("OptionsButton") as Button;
        Button exitButton = GetComponent("ExitButton") as Button;

        GameObject optionsMenu = GameObject.Find("Options");
        Button backButton = GetComponent("BackButton") as Button;
    }

}
