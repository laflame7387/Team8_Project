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
    
    //UIManager �̱���ȭ
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

    //UIManager ������Ƽȭ
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

    //UI�� ������ dim Ȱ��ȭ, UI ����, �ݱ��ư �̵�(���� UI��)
    //�ݱ⸦ ������ dim ��Ȱ��ȭ, UI ����, �ݱ��ư �̵�(���� �ڸ���)
    //�ʿ��� ���� : �ݱ��ư�� ���� ��ġ(�����ϸ�), dim �̹���,
    //              ������ �ϴ� UI, 
    //

    public void OnClicked()
    {
        dimBackground.SetActive(true);
    }

    //3�ʰ� ��ٸ��� �ϴ� �޼���
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
