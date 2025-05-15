using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] TextMeshProUGUI scoreText;

    void Start()
    {
        scoreManager = ScoreManager.Instance;
    }

    void Update()
    {
        scoreText.text = scoreManager.CurrentScore.ToString();
    }
}
