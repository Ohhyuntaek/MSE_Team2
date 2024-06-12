using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectManager : MonoBehaviour
{
    public Transform cameraTransform; // The transform of the camera to shake
    public float shakeDuration = 0.5f; // Duration of the shake effect
    public float shakeMagnitude = 0.2f; // Magnitude of the shake effect

    private Vector3 originalPos;
    private float shakeTimeRemaining;

    private bool isShaking = false;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        if(isShaking)
        {
            Debug.Log("Shaking");

            if (shakeTimeRemaining > 0)
            {
                cameraTransform.localPosition = originalPos + Random.insideUnitSphere * shakeMagnitude;
                shakeTimeRemaining -= Time.deltaTime;
            }
            else
            {
                shakeTimeRemaining = 0f;
                cameraTransform.localPosition = originalPos;
                isShaking = false;
            }
        }
    }

    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeTimeRemaining = duration;
        originalPos = cameraTransform.localPosition;
        isShaking = true;
    }
}
