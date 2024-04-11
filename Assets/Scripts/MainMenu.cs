using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Sample Scene 1_Harry");
    }

    public void ControlsButton()
    {
        SceneManager.LoadScene("Controls Screen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
