using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Texture2D skyboxNight;
    [SerializeField] private Texture2D skyboxSunrise;
    [SerializeField] private Texture2D skyboxDay;
    [SerializeField] private Texture2D skyboxSunset;

    [SerializeField] private Gradient gradientsNightToSunrise;
    [SerializeField] private Gradient gradientsSunriseToDay;
    [SerializeField] private Gradient gradientsDayToSunset;
    [SerializeField] private Gradient gradientsSunsetToNight;

    [SerializeField] private Light globalLight;

    private int _minutes;

    private int Minutes
    {
        get => _minutes;
        set
        {
            _minutes = value;
            OnMinutesChange(value);
        }
    }

    private int _hours = 5;

    private int Hours
    {
        get => _hours;
        set
        {
            _hours = value;
            OnHoursChange(value);
        }
    }

    private int Days { get; set; }

    private float _tempSecond;
    private static readonly int Texture1 = Shader.PropertyToID("_Texture1");
    private static readonly int Blend = Shader.PropertyToID("_Blend");
    private static readonly int Texture2 = Shader.PropertyToID("_Texture2");

    private void Update()
    {
        _tempSecond += Time.deltaTime * 10;

        if (!(_tempSecond >= 1)) return;

        Minutes += 1;
        _tempSecond = 0;
    }

    private void OnMinutesChange(int value)
    {
        globalLight.transform.Rotate(Vector3.up, (1f / (1440f / 4f)) * 360f, Space.World);
        if (value >= 60)
        {
            Hours++;
            _minutes = 0;
        }


        if (Hours < 24) return;
        Hours = 0;
        Days++;
    }

    private void OnHoursChange(int value)
    {
        switch (value)
        {
            case 6:
                StartCoroutine(LerpSkybox(skyboxNight, skyboxSunrise, 10f));
                StartCoroutine(LerpLight(gradientsNightToSunrise, 10f));
                break;
            case 8:
                StartCoroutine(LerpSkybox(skyboxSunrise, skyboxDay, 10f));
                StartCoroutine(LerpLight(gradientsSunriseToDay, 10f));
                break;
            case 18:
                StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 10f));
                StartCoroutine(LerpLight(gradientsDayToSunset, 10f));
                break;
            case 22:
                StartCoroutine(LerpSkybox(skyboxSunset, skyboxNight, 10f));
                StartCoroutine(LerpLight(gradientsSunsetToNight, 10f));
                break;
        }
    }

    private static IEnumerator LerpSkybox(Texture a, Texture b, float time)
    {
        RenderSettings.skybox.SetTexture(Texture1, a);
        RenderSettings.skybox.SetTexture(Texture2, b);
        RenderSettings.skybox.SetFloat(Blend, 0);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat(Blend, i / time);
            yield return null;
        }

        RenderSettings.skybox.SetTexture(Texture1, b);
    }

    private IEnumerator LerpLight(Gradient lightGradient, float time)
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            globalLight.color = lightGradient.Evaluate(i / time);
            RenderSettings.fogColor = globalLight.color;
            yield return null;
        }
    }
}