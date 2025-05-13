using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void GoMain()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void GoStart()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void GoGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
