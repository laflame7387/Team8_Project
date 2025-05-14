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
        if (ScoreManager.Instance == null)
        {
            Debug.LogError("ScoreManager.Instance null");
            return;
        }

        List<int?> scoreList = ScoreManager.Instance.HighScore;
        //.Where(n => n.HasValue)       // LINQ �޼���
        //.Select(n => n.Value)         // int? �� int
        //.OrderByDescending(n => n)    // �������� ����
        //.ToList();

        Debug.Log("���� ���̽��ھ� ����Ʈ: " + string.Join(", ", scoreList.Select(s => s?.ToString() ?? "null")));

        // �������� TMP�� ǥ�� (�������� �ʴ� ����� "����")
        firstScore.text = scoreList.Count > 0 && scoreList[0].HasValue ? $"{scoreList[0]:N0}��" : "���� ����� �����ϴ�";
        secondScore.text = scoreList.Count > 1 && scoreList[1].HasValue ? $"{scoreList[1]:N0}��" : "���� ����� �����ϴ�";
        thirdScore.text = scoreList.Count > 2 && scoreList[2].HasValue ? $"{scoreList[2]:N0}��" : "���� ����� �����ϴ�";

    }
}
