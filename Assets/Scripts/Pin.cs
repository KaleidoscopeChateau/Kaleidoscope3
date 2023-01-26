using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum PinType { Generator, Input, Output, Consumer }

public class Pin : MonoBehaviour
{
    public PinType pinType;
    public bool isGenerator = false;
    public bool dragging = false;
    public float input, power, neededPower, volts, fuel, powerConsumption;
    public GameObject previousPin;
    public TextMeshPro powerMeter;
    public TextMeshPro powerInfo;
    LineRenderer lineRenderer;

    public GameObject generator;
    float consumePower;
    bool EmergencyFail = false;
    float generatorinput, generatorPower, generatorFuel, generatorVolt;

    public void SetPreviousPin(GameObject previousPin)
    {
        this.previousPin = previousPin;
    }


    private void Start()
    {
        if (!GetComponent<LineRenderer>()) { gameObject.AddComponent<LineRenderer>(); }
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;

        generatorFuel = fuel;
        generatorinput = power;
        generatorPower = power;
        generatorVolt = volts;
        EmergencyFail = false;
    }

    public void Update()
    {
        if (dragging)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
            {
                transform.position = hit.point;
            }
        }

        ConsumePower();

        if (isGenerator == true)
        {
            if (fuel > 0) { fuel -= 1f * Time.deltaTime; } else { fuel = 0; }

            if (previousPin != null)
            {
                previousPin.GetComponent<Pin>().input = input;
                previousPin.GetComponent<Pin>().power = power;
                previousPin.GetComponent<Pin>().volts = volts;
            }
        }
        else
        {
            if (previousPin.GetComponent<Pin>().power <= 0 || previousPin.GetComponent<Pin>().input <= 0 || previousPin.GetComponent<Pin>().consumePower <= 0)
            {
                power = 0;
                input = 0;
                powerConsumption = 0;
                consumePower = 0;
                volts = 0;
            }
            else
            {
                input = previousPin.GetComponent<Pin>().input;
                power = previousPin.GetComponent<Pin>().consumePower;
                volts = previousPin.GetComponent<Pin>().volts;
            }
        }



        if (previousPin)
        {
            lineRenderer.positionCount = 0;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, previousPin.transform.position);
            lineRenderer.useWorldSpace = true;
        }


        if (power > 0)
        {
            consumePower = power - neededPower;
            powerConsumption = neededPower;
        }
        else
        {
            consumePower = 0;
            powerConsumption = 0;
            volts = 0;
        }
        if (isGenerator == false)
        {
            powerInfo.text = $"Input: {power}\n Needed: {neededPower}\n Volt: {volts}\n Consumption: {Mathf.RoundToInt(powerConsumption)}\n Output: {Mathf.RoundToInt(consumePower)}";
        }
        else
        {
            powerInfo.text = $"Fuel:{fuel}\n Output:{power}\n Volt: {volts}";
        }

    }
    void ConsumePower()
    {
        if (fuel <= 0)
        {
            fuel = 0;
            if (isGenerator == true)
            {
                input = 0;
                power = 0;
                volts = 0;
                consumePower = 0;
                EmergencyFail = true;
            }
            else
            {
                consumePower = 0;
            }
        }
        else
        {
            if (isGenerator == true)
            {
                volts = generatorVolt;
                input = generatorinput;
                power = generatorPower;
                EmergencyFail = true;
            }
            EmergencyFail = true;
        }
    }
    void OnMouseDown()
    {
        gameObject.layer = 2;
        dragging = true;
    }
    void OnMouseUp()
    {
        gameObject.layer = 0;
        dragging = false;
    }
}