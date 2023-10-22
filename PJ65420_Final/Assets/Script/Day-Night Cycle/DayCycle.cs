using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DayCycle : MonoBehaviour
{
    [Range ( 0f, 24f )] //hours in a day
    public float timeOfDay;

    public float orbitSpeed = 1.0f;
    public Light Sun;
    public Light Moon;
    public AnimationCurve starCurve;

    public Volume skyVolume;

    private bool isNight;
    private PhysicallyBasedSky sky;


    public void Start()
    {
        skyVolume.profile.TryGet( out sky);
    }
    public void Update()
    {
        timeOfDay += Time.deltaTime * orbitSpeed;
        if (timeOfDay > 24.0f)
        {
            timeOfDay = 0f;
        }

        UpdateTime();
    }

    private void OnValidate()
    {
        skyVolume.profile.TryGet(out sky);
        UpdateTime();
    }

    private void UpdateTime()
    {
        float alpha = timeOfDay / 24.0f;
        float sunRotation = Mathf.Lerp(-90f, 270f, alpha);
        float moonRotation = sunRotation - 180f;

        Sun.transform.rotation = Quaternion.Euler(sunRotation, Sun.transform.rotation.y, 0f);
        Moon.transform.rotation = Quaternion.Euler(moonRotation, Moon.transform.rotation.y, 0f);

        CheckDayNightTransition();

        sky.spaceEmissionMultiplier.value = starCurve.Evaluate(alpha) * 30000.0f;
    }

    private void CheckDayNightTransition()
    {
        if (isNight)
        {
            if (Moon.transform.rotation.eulerAngles.x > 180)
            {
                StartDay();
            }
        }

        else
        {
            if (Sun.transform.rotation.eulerAngles.x > 180)
            {
                StartNight();
            }
        }
    }

    private void StartDay()
    {
        isNight = false;
        Sun.shadows = LightShadows.Soft;
        Moon.shadows = LightShadows.None;
    }

    private void StartNight()
    {
        isNight = true;
        Sun.shadows = LightShadows.None;
        Moon.shadows = LightShadows.Soft;
    }
}
