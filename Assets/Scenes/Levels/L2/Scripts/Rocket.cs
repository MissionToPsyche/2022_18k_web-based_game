using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float _acceleration;
    private float _accelerationRate = 0.1f;
    public float _speed = 0f;
    public float _maxSpeed = 5f;
    private float _rotationSpeed = 0f;
    private float _rotationMaxSpeed = 100f;
    private float _torque = 0;
    private float _torqueByMass = 0;
    private float _torqueByThrust = 0;
    private bool _enginesOn = false;
    public bool isOnGround = true;
    public float totalThrust = 0f;
    public float totalLeftThrust = 0f;
    public float totalRightThrust = 0f;
    public float totalMass = 0f;
    public float totalLeftMass = 0f;
    public float totalRightMass = 0f;
    public float totalFuel = 0f;
    public float totalFuelConsumptionRate = 0f;
    public float TWR = 0f;
    private List<Transform> rocketParts = new List<Transform>();
    private List<Rigidbody2D> rocketPartRigidbodies = new List<Rigidbody2D>();
    public CameraFollow cameraFollowScript;
    private Coroutine _accelerationCoroutine;
    private Coroutine _torqueCoroutine;
    public UIManager uiManager;
    public GameObject endingPanel;
    private IEnumerator _consumeFuelEnumerator;
    public int numberOfFuelTanks;
    public float fuelDrainagePerTank;
    public float fuelDrainageRatePerTank;
    void Start()
    {
        _consumeFuelEnumerator = ConsumeFuel();
        Init();
    }

    IEnumerator Accelerate()
    {
        while (true)
        {
            _speed += _acceleration;
            yield return new WaitForSeconds(_accelerationRate);
        }
    }
    IEnumerator AccelerateTorque()
    {
        while (true)
        {
            _torque = _torqueByMass + _torqueByThrust;
            _rotationSpeed += _torque;
            yield return new WaitForSeconds(_accelerationRate);
        }
    }
    void Update()
    {
        // Set max speed
        if (Mathf.Abs(_speed) >= (Mathf.Abs(_maxSpeed)))
        {
            // if falling, set max fall velocity as negative
            if (_speed < 0)
            {
                _speed = -_maxSpeed;
            }
            else
            {
                _speed = _maxSpeed;
            }
        }
        // Fly in the direction of the rocket
        if (_enginesOn)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        // Debug.Log(_rotationSpeed);
        if (!isOnGround)
        {
            // When falling, the rocket needs to fall straight down without care for rotation
            transform.Translate(Vector3.up * _speed * Time.deltaTime, Space.World);
        }
        if (_rotationSpeed != 0)
        {
            // Set max speed
            if (Mathf.Abs(_rotationSpeed) >= (Mathf.Abs(_rotationMaxSpeed)))
            {
                // if falling, set max fall velocity as negative
                if (_rotationSpeed < 0)
                {
                    _rotationSpeed = -_rotationMaxSpeed;
                }
                else
                {
                    _rotationSpeed = _rotationMaxSpeed;
                }
            }
            transform.Rotate(Vector3.back * _rotationSpeed * Time.deltaTime);
        }
    }
    void Init()
    {
        _enginesOn = false;
    }
    public void BuildFinished()
    {
        _accelerationCoroutine = StartCoroutine(Accelerate());
        _torqueCoroutine = StartCoroutine(AccelerateTorque());
        isOnGround = false;
        GetReferenceToRocketParts();
        EnginesOff();
        CalculateMassDistribution();
        foreach (Transform child in rocketParts)
        {
            child.SendMessage("OnFinishedBuilding");
        }
        CalculateFuelDrainageRatePerTank();
        uiManager.SetMaxFuelBarAmount(totalFuel);
    }
    public void CalculateMassDistribution()
    {
        float massDifference = Mathf.Abs(totalLeftMass - totalRightMass);
        float percentage = massDifference * totalMass / 100;
        if (percentage >= 25)
        {
            float torqueToApply = percentage / 100;
            if (totalLeftMass > totalRightMass)
            {
                _torqueByMass = -torqueToApply;
            }
            else if (totalLeftMass < totalRightMass)
            {
                _torqueByMass = torqueToApply;
            }
        }
        // Debug.Log(totalLeftMass);
        // Debug.Log(totalRightMass);
        // Debug.Log(massDifference);
        // Debug.Log(percentage);
        // Debug.Log(_torqueByMass);
    }
    private void GetReferenceToRocketParts()
    {
        foreach (Transform child in transform)
        {
            rocketParts.Add(child);
            rocketPartRigidbodies.Add(child.GetComponent<Rigidbody2D>());
            if (child.tag == "Capsule")
            {
                cameraFollowScript.target = child;
            }
            else if (child.tag == "FuelTank")
            {
                numberOfFuelTanks++;
            }
        }
    }
    public void EnginesOn()
    {
        _acceleration = 0.01f;
        _enginesOn = true;
        GetReferenceToRocketParts();
        foreach (Rigidbody2D rb in rocketPartRigidbodies)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.useFullKinematicContacts = true;
        }
        StartCoroutine(_consumeFuelEnumerator);
    }
    public void EnginesOff()
    {
        _enginesOn = false;
        StopCoroutine(_consumeFuelEnumerator);
        ApplyGravity();
        MakeRocketPartsDynamicWithoutGravity();
        // Turn off engine animation

        // Turn off engine audio
    }
    public void ApplyGravity()
    {
        if (!_enginesOn && !isOnGround)
        {
            _acceleration = -0.2f;
        }
    }
    private void MakeRocketPartsDynamicWithoutGravity()
    {
        foreach (Transform child in rocketParts)
        {
            if (child.GetComponent<RocketPart>().isPartOfTheRocket)
            {
                Rigidbody2D childRigidbody = child.GetComponent<Rigidbody2D>();
                childRigidbody.bodyType = RigidbodyType2D.Dynamic;
                childRigidbody.simulated = true;
                childRigidbody.drag = 0.3f;
                childRigidbody.gravityScale = 0f;   // Off gravity so it doesn't affect the movement
                childRigidbody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
    void CalculateFuelDrainageRatePerTank()
    {
        fuelDrainageRatePerTank = totalFuelConsumptionRate / numberOfFuelTanks;
    }
    void OnHitGround()
    {
        _speed = 0;
        if (!_enginesOn)
        {
            _acceleration = 0;
        }
        if (!uiManager.engineControllerBtnActive)
        {
            uiManager.ActivateEngineControllerBtn();
        }
    }
    IEnumerator ConsumeFuel()
    {
        while (totalFuel > 0)
        {
            totalFuel -= totalFuelConsumptionRate * Time.deltaTime;
            fuelDrainagePerTank += fuelDrainageRatePerTank * Time.deltaTime;
            uiManager.SetFuelBar(totalFuel);
            if (totalFuel < 0)
            {
                totalFuel = 0;
                // Turn off the engine when there is no fuel
                EnginesOff();
            }
            yield return null;
        }
    }
    public void CalculateTWR()
    {
        TWR = totalThrust / totalMass;
    }
    void OnCrash()
    {
        gameObject.SetActive(false);
    }
    public float GetSpeed()
    {
        return _speed;
    }
    public float GetRotationSpeed()
    {
        return _rotationSpeed;
    }
    public void OnReduceFuel(float fuelVal)
    {
        totalFuel -= fuelVal;
        CalculateFuelDrainageRatePerTank();
        uiManager.UpdateRocketProperties();
        uiManager.SetFuelBar(totalFuel);
    }
    public void OnReduceTotalFuelConsumptionRate(float fuelConsumptionRateVal)
    {
        totalFuelConsumptionRate -= fuelConsumptionRateVal;
        uiManager.UpdateRocketProperties();
        if (totalFuelConsumptionRate <= 0)
        {
            EnginesOff();
        }
    }
    public void OnReduceTotalThrust(float thrustVal)
    {
        totalThrust -= thrustVal;
        CalculateTWR();
        uiManager.UpdateRocketProperties();
    }
    public void OnReduceTotalRightThrust(float thrustVal)
    {
        totalRightThrust -= thrustVal;
        uiManager.UpdateRocketProperties();
    }
    public void OnReduceTotalLeftThrust(float thrustVal)
    {
        totalLeftThrust -= thrustVal;
        uiManager.UpdateRocketProperties();
    }

    public void OnReduceTotalMass(float massVal)
    {
        totalMass -= massVal;
        CalculateTWR();
        uiManager.UpdateRocketProperties();
    }
    public void OnReduceTotalRightMass(float massVal)
    {
        totalRightMass -= massVal;
        uiManager.UpdateRocketProperties();
    }
    public void OnReduceTotalLeftMass(float massVal)
    {
        totalLeftMass -= massVal;
        uiManager.UpdateRocketProperties();
    }
    public void OnWinGame()
    {
        _speed = 0.2f;
        _acceleration = 0;
        endingPanel.SetActive(true);
    }
}
