using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using System.Xml;
using System.Globalization;

public class HighScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI firstScore;
    public TextMeshProUGUI secondScore;
    public TextMeshProUGUI thirdScore;


    // Start is called before the first frame update
    void Start()
    {
        List<int?> scoreList = ScoreManager.Instance.HighScore;
        //.Where(n => n.HasValue)       // LINQ 메서드
        //.Select(n => n.Value)         // int? → int
        //.OrderByDescending(n => n)    // 내림차순 정렬
        //.ToList();

        // 순위별로 TMP에 표시 (기록이 없는 등수는 "아직 기록이 없습니다")
        firstScore.text = scoreList.Count > 0 && scoreList[0].HasValue ? $"{scoreList[0]:N0}점" : "아직 기록이 없습니다";
        secondScore.text = scoreList.Count > 1 && scoreList[1].HasValue ? $"{scoreList[1]:N0}점" : "아직 기록이 없습니다";
        thirdScore.text = scoreList.Count > 2 && scoreList[2].HasValue ? $"{scoreList[2]:N0}점" : "아직 기록이 없습니다";

    }
}
