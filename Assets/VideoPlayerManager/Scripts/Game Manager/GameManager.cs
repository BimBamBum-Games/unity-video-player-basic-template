using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Dummy GameManager.
    public delegate void GameManagerEvent_NonParNonRet();
    public delegate IEnumerator GameManagerEvent_IntParEnmRet(int intValue);

    public GameManagerEvent_NonParNonRet OnVideoButtonClick;
    public GameManagerEvent_IntParEnmRet OnVideoButtonClick_IntParEnmRet;

    [SerializeField] Button videoPlayBtn;

    private int dummyIntValue;

    protected void Awake() {
        if (videoPlayBtn == null) Debug.LogWarning("Button has not assigned yet!");
    }

    protected void OnEnable() {
        videoPlayBtn.onClick.AddListener(ClickOnVideoButton);
    }

    protected void OnDisable() {
        videoPlayBtn.onClick.RemoveListener(ClickOnVideoButton);
    }

    public void ClickOnVideoButton() {
        OnVideoButtonClick?.Invoke();
        StartCoroutine(OnVideoButtonClick_IntParEnmRet?.Invoke(dummyIntValue));
    }

}
