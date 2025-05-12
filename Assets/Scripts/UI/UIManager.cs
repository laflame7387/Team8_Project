using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IButton
{
    void OnClicked();
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instactiate { get; private set; }

    [SerializeField] private GameObject closeButton;
    [SerializeField] private GameObject dimBackground;


    private Transform prevButtonPosition;

    // Start is called before the first frame update
    void Start()
    {


        if (closeButton != null)
        {
            prevButtonPosition = closeButton.transform.parent;
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

}
