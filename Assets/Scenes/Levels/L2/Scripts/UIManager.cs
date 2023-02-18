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
    public GameObject rocketInfoPanel;
    private TextMeshProUGUI _rocketInfoText;
    public GameObject playerDialogue;

    void Start()
    {
        _btnText = engineControllerButton.GetComponentInChildren<TextMeshProUGUI>();
        _rocketInfoText = rocketInfoPanel.GetComponentInChildren<TextMeshProUGUI>();
        _btnText.text = _enginesOffText;
        Init();
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
    public void OnClickFinishedBuildingBtn()
    {
        // rocketInfo greater than or equal to 1 is needed for the rocket to takeoff
        if (rocket.TWR >= 1)
        {
            blackPanel.SetActive(true);
        }
        else
        {
            playerDialogue.SetActive(true);
        }
    }
    private void Init()
    {
        tooltip.SetActive(true);
        engineControllerButton.SetActive(false);
        finishedBuildingButton.SetActive(true);
        blackPanel.SetActive(false);
        controlsTutorial.SetActive(false);
        player.SetActive(true);
        _rocketInfoText.text = "Mass: 0t\nThrust: 0t\nThrust/Weight: 0 \nFuel: 0t \nFuel Consumption: 0t/sec";

    }
    public void OnFinishedBuilding()
    {
        finishedBuildingButton.SetActive(false);
        rocket.BuildFinished();
        controlsTutorial.SetActive(true);
        player.SetActive(false);
        rocketInfoPanel.SetActive(false);
    }
    public void ActivateEngineControllerBtn()
    {
        engineControllerButton.SetActive(true);
    }
    public void OnCorrectlyPlacedRocketPart()
    {
        if (rocket.TWR > 0)
        {
            _rocketInfoText.text = "Mass: " + rocket.totalMass.ToString("F1") + "t\nThrust: " + rocket.totalThrust + "t\nThrust/Weight: " + rocket.TWR.ToString("F2") + "\nFuel: " + rocket.totalFuel.ToString("F1") + "t\nFuel Consumption: " + rocket.totalFuelConsumptionRate.ToString("F1") + "t/sec";
        }
        else
        {
            _rocketInfoText.text = "Mass: " + rocket.totalMass.ToString("F1") + "t\nThrust: " + rocket.totalThrust + "t\nThrust/Weight: " + rocket.TWR + "\nFuel: " + rocket.totalFuel.ToString("F1") + "t\nFuel Consumption: " + rocket.totalFuelConsumptionRate.ToString("F1") + "t/sec";
        }
    }
}
