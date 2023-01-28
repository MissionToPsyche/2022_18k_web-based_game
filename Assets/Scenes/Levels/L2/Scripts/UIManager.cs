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
    public RocketMovement rocketMovement;
    public GameObject tooltip;
    // Start is called before the first frame update
    void Start()
    {
        tooltip.SetActive(true);
        engineControllerButton.SetActive(false);
        _btnText = engineControllerButton.GetComponentInChildren<TextMeshProUGUI>();
        _btnText.text = _enginesOffText;
    }

    public void ToggleEngines()
    {
        if (_enginesOn)
        {
            _btnText.text = _enginesOffText;
            rocketMovement.EnginesOff();
            _enginesOn = false;

        }
        else
        {
            _btnText.text = _enginesOnText;
            rocketMovement.EnginesOn();
            _enginesOn = true;
        }
    }
    public void FinishedBuildingTheRocket()
    {
        rocketMovement.BuildFinished();
        finishedBuildingButton.SetActive(false);
    }
    public void ActivateEngineControllerBtn()
    {
        engineControllerButton.SetActive(true);
    }
}
