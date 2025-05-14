using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

interface IButton
{
    void OnClicked();
}

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    public bool isWaiting = false;
    private int three = 3;

    [SerializeField] private TextMeshProUGUI animationText;
    [SerializeField] private GameObject dimBackground;
    //[SerializeField] private GameObject closeButton;

    private Transform prevButtonPosition;
    
    //UIManager 싱글톤화
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    //UIManager 프로퍼티화
    public static UIManager Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    if (closeButton != null)
    //    {
    //        prevButtonPosition = closeButton.transform.parent;
    //    }
    //}

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

    //3초간 기다리게 하는 메서드
    public void WaitThreeSeconds()
    {
        if (isWaiting == true) 
        {
            dimBackground.SetActive(true);
            StartCoroutine(ThreeSeconds());
            isWaiting = false;
        }

        else
        {
            return;
        }
    }

    IEnumerator ThreeSeconds()
    {
        animationText.text = three.ToString();
        yield return new WaitForSeconds(1.0f);
        animationText.text = (three - 1).ToString();
        yield return new WaitForSeconds(1.0f);
        animationText.text = (three - 2).ToString();
        yield return new WaitForSeconds(1.0f);
        dimBackground.SetActive(false);
    }
}
