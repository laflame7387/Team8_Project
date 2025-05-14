using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

interface IButton
{
    void OnClicked();
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private int three = 3;

    [SerializeField] private TextMeshProUGUI animationText;
    [SerializeField] private GameObject dimBackground;
    //[SerializeField] private GameObject closeButton;

    private Transform prevButtonPosition;
    
    //UIManager 싱글톤화
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    //UIManager 프로퍼티화
    //public static UIManager instance
    //{
    //    get
    //    {
    //        if(instance == null)
    //        {
    //            return null;
    //        }
    //        return instance;
    //    }
    //}

    void Start()
    {
        //if (closeButton != null)
        //{
        //    prevButtonPosition = closeButton.transform.parent;
        //}

        //게임씬 시작시 3초 기다리기
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            dimBackground.SetActive(true);
            animationText.gameObject.SetActive(true);
            StartCoroutine(ThreeSeconds());
        }

        if (SceneManager.GetActiveScene().name == "UIScene")
        {
            dimBackground.SetActive(true);
            animationText.gameObject.SetActive(true);
            StartCoroutine(ThreeSeconds());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //UI를 누르면 dim 활성화, UI 켜짐, 닫기버튼 이동(켜진 UI로)
    //닫기를 누르면 dim 비활성화, UI 꺼짐, 닫기버튼 이동(이전 자리로)
    //필요한 변수 : 닫기버튼의 현재 위치(존재하면), dim 이미지,
    //              열고자 하는 UI, 
    //

    public void OnClicked()
    {
        dimBackground.SetActive(true);
    }

    IEnumerator ThreeSeconds()
    {
        animationText.text = "3";
        yield return new WaitForSeconds(1.0f);
        animationText.text = "2";
        yield return new WaitForSeconds(1.0f);
        animationText.text = "1";
        yield return new WaitForSeconds(1.0f);
        dimBackground.SetActive(false);
        animationText.gameObject.SetActive(false);
    }
}
