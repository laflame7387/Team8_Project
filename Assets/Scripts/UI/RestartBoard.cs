using UnityEngine;
using System.Collections;

public class RestartBoard : MonoBehaviour
{
    public PlayerController player;
    public GameObject restartUI;

    private bool isProcessingDeath = false;

    private void Awake()
    {
        restartUI.SetActive(false);
    }

    void Update()
    {
        if (!isProcessingDeath)
        {
            Debug.Log("CheckDeathCondition: " + player.CheckDeathCondition());
        }

        if (!isProcessingDeath && player.CheckDeathCondition())
        {
            StartCoroutine(HandleDeathDelay());
        }
    }

    IEnumerator HandleDeathDelay()
    {
        isProcessingDeath = true;
        yield return new WaitForSeconds(1f);
        restartUI.SetActive(true);

        Time.timeScale = 0f;
    }
}
