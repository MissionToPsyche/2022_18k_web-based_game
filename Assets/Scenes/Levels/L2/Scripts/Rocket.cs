using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    public float _acceleration;
    private float _accelerationRate = 0.1f;
    public float _speed = 0f;
    public float _maxFlightSpeed = 15f;
    private float _maxFallSpeed = 15f;
    public float _rotationSpeed = 0f;
    private float _rotationMaxSpeed = 100f;
    private float _torque = 0;
    private float _torqueByMass = 0;
    private float _torqueByThrust = 0;
    private float _torqueByControls = 0;
    private float _torqueByControlsPower = 0.2f;
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
    private Coroutine _smoothVelocityChangeCoroutine;
    private Coroutine _torqueCoroutine;
    public UIManager uiManager;
    public GameObject endingPanel;
    private IEnumerator _consumeFuelEnumerator;
    public int numberOfFuelTanks;
    public float fuelDrainagePerTank;
    public float fuelDrainageRatePerTank;
    public bool hasLaunched = false;
    public bool alreadyPlayingLaunchAudio = false;
    public bool alreadyPlayingEngineOnAudio = false;
    public float heightAboveGround = 0;
    private bool _hasCapsule = false;
    private bool _hasEngine = false;
    private bool _hasFuelTank = false;
    public float velocity;
    public GameObject groundObject;
    private float _shakeAmount = 0.1f;
    bool once = true;
    void Start()
    {
        _consumeFuelEnumerator = ConsumeFuel();
        Init();
    }

    IEnumerator Accelerate()
    {
        while (true)
        {
            if (_enginesOn)
            {
                _speed += _acceleration;
            }
            else
            {
                _speed -= _acceleration;
            }
            SmoothVelocityIncrease(_speed * 50f);
            yield return new WaitForSeconds(_accelerationRate);
        }
    }
    IEnumerator AccelerateTorque()
    {
        while (true)
        {
            _torque = _torqueByMass + _torqueByThrust + _torqueByControls;
            _rotationSpeed += _torque;
            yield return new WaitForSeconds(_accelerationRate);
        }
    }
    IEnumerator IncreaseVelocity(float targetVelocity)
    {
        float currentVelocity = velocity;
        float smoothTime = 0.1f; // adjust this value to control the smoothness of the change
        float smoothVelocity = 0.0f;

        while (Mathf.Abs(currentVelocity - targetVelocity) > 0.01f)
        {
            currentVelocity = Mathf.SmoothDamp(currentVelocity, targetVelocity, ref smoothVelocity, smoothTime);
            velocity = currentVelocity;
            yield return null;
        }
    }
    private void SmoothVelocityIncrease(float targetVelocity)
    {
        StartCoroutine(IncreaseVelocity(targetVelocity));
    }
    void Update()
    {
        // Calculate Height Above Ground
        float currentHeightAboveGround = transform.position.y - groundObject.transform.position.y - 11.4f;
        heightAboveGround = currentHeightAboveGround * 50;

        // Keep height above 0
        if (heightAboveGround < 0)
        {
            heightAboveGround = 0;
        }
        uiManager.UpdateRocketStats();


        // Set max speed
        if (Mathf.Abs(_speed) >= (Mathf.Abs(_maxFlightSpeed)) || Mathf.Abs(_speed) >= (Mathf.Abs(_maxFallSpeed)))
        {
            // if falling, set max fall velocity as negative
            if (_speed < 0)
            {
                _speed = -_maxFallSpeed;
            }
            else
            {
                _speed = _maxFlightSpeed;
            }
        }
        {
            // if falling, set max fall velocity as negative
            if (_speed < 0)
            {
                _speed = -_maxFallSpeed;
            }
            else
            {
                _speed = _maxFlightSpeed;
            }
        }

        // Fly in the direction of the rocket
        if (_enginesOn)
        {
            // Move in the direction of the rocket
            Vector3 moveDirection = new Vector3(0, 1, 0);
            float transitionalSpeed = _speed / 5;
            if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270)
            {
                // If facing down, move in the opposite direction
                moveDirection = new Vector3(0, -1, 0);
                if (once)
                {
                    _speed = Mathf.Abs(transitionalSpeed);
                    once = false;
                }
                _acceleration = -Mathf.Abs(_acceleration);
            }
            else
            {
                if (!once)
                {
                    _speed = -Mathf.Abs(transitionalSpeed);
                    once = true;
                }
                _acceleration = Mathf.Abs(_acceleration);
            }
            transform.Translate(moveDirection * _speed * Time.deltaTime);
        }

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

        // Shake the rocket when velocity is above certain threshold
        // Check if velocity is above a threshold
        if (Mathf.Abs(velocity) > 100)
        {
            if (heightAboveGround < 10000)
            {
                _shakeAmount = 0.1f;
            }
            else if (heightAboveGround > 10000 && heightAboveGround < 20000)
            {
                _shakeAmount = 0.5f;
            }
            else if (heightAboveGround > 20000 && heightAboveGround < 30000)
            {
                _shakeAmount = 0.3f;
            }
            else if (heightAboveGround > 30000 && heightAboveGround < 50000)
            {
                _shakeAmount = 0.05f;
            }
            else if (heightAboveGround > 50000)
            {
                _shakeAmount = 0;
            }
            // Apply a random offset to the rocket rotation
            float angleZ = Random.Range(-_shakeAmount, _shakeAmount);
            Vector3 rotationOffset = new Vector3(0, 0, angleZ);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationOffset);
        }

        // Key controls
        if (Input.GetKey(KeyCode.A))
        {
            // Rotate left
            _torqueByControls = -_torqueByControlsPower;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // Rotate right
            _torqueByControls = _torqueByControlsPower;
        }
        else
        {
            // Stop rotating
            _torqueByControls = 0;
            if (_torqueByMass == 0f && _torqueByThrust == 0f)
            {
                _torque = 0;
                _rotationSpeed = 0;
            }
        }
    }
    void Init()
    {
        _enginesOn = false;
        heightAboveGround = 0;
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
        SetMaxSpeedBasedOnTWR();
        uiManager.SetMaxFuelBarAmount(totalFuel);
    }
    void SetMaxSpeedBasedOnTWR()
    {
        _maxFlightSpeed *= TWR;
    }
    public void CalculateMassDistribution()
    {
        float massDifference = Mathf.Abs(totalLeftMass - totalRightMass);
        float percentage = massDifference * totalMass / 100;
        if (percentage >= 20)
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
    public bool RocketHasBareMinimumParts()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "Capsule")
            {
                _hasCapsule = true;
            }
            else if (child.tag == "FuelTank")
            {
                _hasFuelTank = true;
            }
            else if (child.tag == "Engine")
            {
                _hasEngine = true;
            }
        }
        if (_hasCapsule && _hasEngine && _hasFuelTank)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool RocketHasOnlyOneCapsule()
    {
        int capsuleCount = 0;
        foreach (Transform child in transform)
        {
            if (child.tag == "Capsule")
            {
                capsuleCount++;
            }
        }
        if (capsuleCount == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
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
        if (totalFuel > 0 && totalFuelConsumptionRate > 0)
        {
            _acceleration = 0.05f;
            _enginesOn = true;
            GetReferenceToRocketParts();
            foreach (Rigidbody2D rb in rocketPartRigidbodies)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.useFullKinematicContacts = true;
            }
            StartCoroutine(_consumeFuelEnumerator);
            // Turn on engine animation
            foreach (Transform child in rocketParts)
            {
                if (child.tag == "Engine")
                {
                    child.GetComponent<RocketPart>().EngineOn();
                }
            }
        }
    }
    public void EnginesOff()
    {
        _enginesOn = false;
        StopCoroutine(_consumeFuelEnumerator);
        ApplyGravity();
        MakeRocketPartsDynamicWithoutGravity();
        // Turn off engine animation
        foreach (Transform child in rocketParts)
        {
            if (child.tag == "Engine")
            {
                child.GetComponent<RocketPart>().EngineOff();
            }
        }
        // Turn off engine audio
        SoundManager.instance.Stop("EngineOn");
        SoundManager.instance.Stop("Launch");
        alreadyPlayingEngineOnAudio = false;

    }
    public void ApplyGravity()
    {
        if (!_enginesOn && !isOnGround)
        {
            _acceleration = 0.2f;
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
                AllFuelRanOut();
            }
            yield return null;
        }
    }
    void AllFuelRanOut()
    {
        totalFuel = 0;
        // Turn off the engine when there is no fuel
        EnginesOff();
        // Change the Theme music into disasterTheme music
        SoundManager.instance.Stop("Theme");
        SoundManager.instance.Play("DisasterTheme");
    }
    public void CalculateTWR()
    {
        TWR = totalThrust / totalMass;
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
        SoundManager.instance.StopAll();
    }
    public void OnGameOver()
    {
        _speed = 0f;
        _acceleration = 0;
        gameObject.SetActive(false);
        SoundManager.instance.StopAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
