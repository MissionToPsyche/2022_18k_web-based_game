using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public GameObject engineControllerButton;
    private TextMeshProUGUI _btnText;
    private bool _enginesOn = false;
    private string _enginesOnText = "Engines on";
    private string _enginesOffText = "Engines off";
    public RocketMovement rocketMovement;
    // Start is called before the first frame update
    void Start()
    {
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
}
