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
        Time.timeScale = 1.0f;
    }
    public void GoStart()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void GoGame()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1.0f;
        ScoreManager.Instance.ResetScore();
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
