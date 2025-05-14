using UnityEngine;
using System.Collections;

public class RestartBoard : MonoBehaviour
{
    public PlayerController player;
    public GameObject restartUI;
    public GameObject scoreBoard;

    private bool isProcessingDeath = false;

    private void Awake()
    {
        restartUI.SetActive(false);
    }

    void Update()
    {
        //if (!isProcessingDeath)
        //{
        //    Debug.Log("CheckDeathCondition: " + player.CheckDeathCondition());
        //}

        if (!isProcessingDeath && player.CheckDeathCondition())
        {
            StartCoroutine(HandleDeathDelay());
        }
    }

    IEnumerator HandleDeathDelay()
    {
        isProcessingDeath = true;
        yield return new WaitForSeconds(1f);
        scoreBoard.SetActive(false);
        restartUI.SetActive(true);

        Time.timeScale = 0f;
    }
}
