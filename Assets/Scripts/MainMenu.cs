using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string multiplayer;
    public string singlePlayer;
    public string startMenu;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartMenu()
    {
        SceneManager.LoadScene(startMenu);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(multiplayer);
    }

    public void SinglePlayer()
    {
        SceneManager.LoadScene(singlePlayer);
    }

}