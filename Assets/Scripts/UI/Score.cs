using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] TextMeshProUGUI scoreText;

    //[SerializeField] private int nowScore;
    

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();

        if (scoreManager != null )
        {
            Debug.Log("ScoreManager 찾기 성공");
        }
        else
        {
            scoreManager = FindAnyObjectByType<ScoreManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = scoreManager.CurrentScore.ToString();
    }
}
