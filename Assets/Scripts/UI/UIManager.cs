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

    //UI를 누르면 dim 활성화, UI 켜짐, 닫기버튼 이동(켜진 UI로)
    //닫기를 누르면 dim 비활성화, UI 꺼짐, 닫기버튼 이동(이전 자리로)
    //필요한 변수 : 닫기버튼의 현재 위치(존재하면), dim 이미지,
    //              열고자 하는 UI, 
    //

    public void OnClicked()
    {
        dimBackground.SetActive(true);
        
    }

}
