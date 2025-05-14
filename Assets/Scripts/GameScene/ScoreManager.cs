using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int CurrentScore { get; private set; } = 0;
    public int HighScore { get; private set; } = 0;
    public int TotalScore { get; private set; } = 0;

    public event Action<int, int> OnScoreReset; //점수 리셋될 점수 감지하기

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
        Debug.Log($"점수 증가, 현재 점수 : {CurrentScore}");
    }

    public void ResetScore()
    {
        TotalScore += CurrentScore;
        CurrentScore = 0;

        OnScoreReset?.Invoke(CurrentScore, TotalScore); //점수 리셋되면 알려주기
    }

    //최고기록 변경 메서드
    public bool HiScoreChange()
    {
        if(HighScore < CurrentScore)
        {
            HighScore = CurrentScore;
            return true;
        }

        else
        {
            return false;
        }
    }


}
