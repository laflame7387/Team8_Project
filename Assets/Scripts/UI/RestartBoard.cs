using UnityEngine;
using System.Collections;

public class RestartBoard : MonoBehaviour
{
    public PlayerController player;
    public GameObject restartUI;
    public GameObject scoreBoard;
    public GameObject hpUI;
    public GameObject highScoreUI;

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
        hpUI.SetActive(false);
        restartUI.SetActive(true);

        //highScore 갱신시에만 신기록 UI 표시
        {
            if (ScoreManager.Instance.HiScoreChange() == true)
            {
                highScoreUI.SetActive(true);
            }

            else
            {
                highScoreUI.SetActive(false);
            }
        }
            Time.timeScale = 0f;
    }
}
