using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int CurrentScore { get; private set; } = 0;
    public List<int?> HighScore { get; private set; } = new List<int?> { null, null, null };
    
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

    public void SetScore(int CurrentScore)
    {
        Debug.Log("SetScore 적용");
        // 1. null 제거 후 현재 스코어 추가
        List<int> nonNullScores = HighScore
            .Where(s => s.HasValue)
            .Select(s => s.Value)
            .ToList();

        nonNullScores.Add(CurrentScore);

        // 2. 내림차순 정렬
        nonNullScores = nonNullScores
            .OrderByDescending(s => s)
            .Take(3) // 상위 3개만 유지
            .ToList();

        // 3. 기존 HighScore를 업데이트 (3자리 유지)
        HighScore.Clear();
        foreach (var s in nonNullScores)
            HighScore.Add(s);
        while (HighScore.Count < 3)
            HighScore.Add(null); // 자리 채우기
    }

    public void ResetScore()
    {
        TotalScore += CurrentScore;
        CurrentScore = 0;

        OnScoreReset?.Invoke(CurrentScore, TotalScore); //점수 리셋되면 알려주기
    }
}
