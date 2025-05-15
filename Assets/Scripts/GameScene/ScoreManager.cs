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

    public event Action<int, int> OnScoreReset; //���� ���µ� ���� �����ϱ�

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
        Debug.Log($"���� ����, ���� ���� : {CurrentScore}");
    }

    public void SetScore(int CurrentScore)
    {
        Debug.Log("SetScore ����");
        // 1. null ���� �� ���� ���ھ� �߰�
        List<int> nonNullScores = HighScore
            .Where(s => s.HasValue)
            .Select(s => s.Value)
            .ToList();

        nonNullScores.Add(CurrentScore);

        // 2. �������� ����
        nonNullScores = nonNullScores
            .OrderByDescending(s => s)
            .Take(3) // ���� 3���� ����
            .ToList();

        // 3. ���� HighScore�� ������Ʈ (3�ڸ� ����)
        HighScore.Clear();
        foreach (var s in nonNullScores)
            HighScore.Add(s);
        while (HighScore.Count < 3)
            HighScore.Add(null); // �ڸ� ä���
    }

    public void ResetScore()
    {
        TotalScore += CurrentScore;
        CurrentScore = 0;

        OnScoreReset?.Invoke(CurrentScore, TotalScore); //���� ���µǸ� �˷��ֱ�
    }
}
