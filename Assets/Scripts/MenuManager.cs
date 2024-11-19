using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button startButton;
    public void Start()
    {
        //create on click listeners for start and end buttons
        startButton.onClick.AddListener(StartGame);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Level");
    }
}
