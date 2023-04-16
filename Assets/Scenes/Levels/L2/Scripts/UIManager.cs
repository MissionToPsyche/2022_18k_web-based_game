using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public GameObject rocketPartsSidePanel;
    public SpriteRenderer nebulaSpace;
    public GameObject space;
    public GameObject endPortal;
    public Slider fuelBarSlider;
    public GameObject rocketStats;
    private TextMeshProUGUI _rocketStatsText;
    public GameObject trashCan;

    public GameObject Intro;

    void Start()
    {

        _btnText = engineControllerButton.GetComponentInChildren<TextMeshProUGUI>();
        _rocketInfoText = rocketInfoPanel.GetComponentInChildren<TextMeshProUGUI>();
        _rocketStatsText = rocketStats.GetComponentInChildren<TextMeshProUGUI>();
        _btnText.text = _enginesOffText;
        Init();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            Intro.SetActive(false);
        }
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
        fuelBarSlider.gameObject.SetActive(false);
        Intro.SetActive(true);
        _rocketInfoText.text = "Mass: 0t\nThrust: 0t\nThrust/Weight: 0 \nFuel: 0t \nFuel Consumption: 0t/sec";
        _rocketStatsText.text = "Height: 0.0m\nVelocity: 0.0m/s";
    }
    public void OnFinishedBuilding()
    {
        finishedBuildingButton.SetActive(false);
        rocket.BuildFinished();
        controlsTutorial.SetActive(true);
        player.SetActive(false);
        rocketInfoPanel.SetActive(false);
        rocketPartsSidePanel.SetActive(false);
        fuelBarSlider.gameObject.SetActive(true);
        trashCan.SetActive(false);
    }
    public void OnWinGame()
    {
        nebulaSpace.enabled = true;
        space.SetActive(false);
        engineControllerButton.SetActive(false);
        endPortal.SetActive(false);
        rocket.gameObject.SetActive(false);
    }
    public void ActivateEngineControllerBtn()
    {
        engineControllerButton.SetActive(true);
    }
    public void UpdateRocketProperties()
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
    public void UpdateRocketStats()
    {
        if (rocket.heightAboveGround > 1000)
        {
            float tmp = rocket.heightAboveGround / 1000;
            _rocketStatsText.text = "Height: " + tmp.ToString("F1") + "km\nVelocity: " + rocket.velocity.ToString("F1") + "m/s";
        }
        else
        {
            _rocketStatsText.text = "Height: " + rocket.heightAboveGround.ToString("F1") + "m\nVelocity: " + rocket.velocity.ToString("F1") + "m/s";
        }
    }
    public void SetMaxFuelBarAmount(float fuel)
    {
        fuelBarSlider.maxValue = fuel;
        fuelBarSlider.value = fuel;
    }
    public void SetFuelBar(float fuel)
    {
        fuelBarSlider.value = fuel;
    }
}
