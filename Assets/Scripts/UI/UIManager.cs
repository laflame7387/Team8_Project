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
    
    //UIManager �̱���ȭ
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

    //UIManager ������Ƽȭ
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

        //���Ӿ� ���۽� 3�� ��ٸ���
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

    //UI�� ������ dim Ȱ��ȭ, UI ����, �ݱ��ư �̵�(���� UI��)
    //�ݱ⸦ ������ dim ��Ȱ��ȭ, UI ����, �ݱ��ư �̵�(���� �ڸ���)
    //�ʿ��� ���� : �ݱ��ư�� ���� ��ġ(�����ϸ�), dim �̹���,
    //              ������ �ϴ� UI, 
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
