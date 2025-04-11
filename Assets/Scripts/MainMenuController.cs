using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject options;
    public GameObject controls;

    public Button playBtn;
    public Button optionsBtn;
    public Button controlsBtn;
    public Button exitBtn;

    void Start()
    {
        options.SetActive(false);
        controls.SetActive(false);
    }

    void Update()
    {
        
    }


}
