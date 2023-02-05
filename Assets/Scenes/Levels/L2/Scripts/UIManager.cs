using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public GameObject engineControllerButton;
    public GameObject finishedBuildingButton;
    public bool engineControllerBtnActive = false;
    private TextMeshProUGUI _btnText;
    private bool _enginesOn = false;
    private string _enginesOnText = "Engines on";
    private string _enginesOffText = "Engines off";
    public Rocket rocket;
    public GameObject tooltip;
    public GameObject blackPanel;
    public GameObject controlsTutorial;
    public GameObject player;

    void Start()
    {
        Init();
        _btnText = engineControllerButton.GetComponentInChildren<TextMeshProUGUI>();
        _btnText.text = _enginesOffText;
    }
    public void ToggleEngines()
    {
        if (_enginesOn)
        {
            _btnText.text = _enginesOffText;
            rocket.EnginesOff();
            _enginesOn = false;
        }
        else
        {
            _btnText.text = _enginesOnText;
            rocket.EnginesOn();
            _enginesOn = true;
        }
    }
    public void FinishedBuildingTheRocket()
    {
        blackPanel.SetActive(true);
    }
    private void Init()
    {
        tooltip.SetActive(true);
        engineControllerButton.SetActive(false);
        finishedBuildingButton.SetActive(true);
        blackPanel.SetActive(false);
        controlsTutorial.SetActive(false);
        player.SetActive(true);
    }
    public void OnFinishedBuilding()
    {
        finishedBuildingButton.SetActive(false);
        rocket.BuildFinished();
        controlsTutorial.SetActive(true);
        player.SetActive(false);
    }
    public void ActivateEngineControllerBtn()
    {
        engineControllerButton.SetActive(true);
    }
}
