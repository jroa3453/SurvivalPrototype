using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    public float dayDuration = 120f;
    public float currentTime = 0.25f;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Ambient Light")]
    public Gradient ambientColor;

    [Header("Survival Multipliers")]
    public float dayDrainMultiplier = 1f;
    public float nightDrainMultiplier = 2f;

<<<<<<< HEAD
=======
    [Header("Skybox")]
    public Material daySkybox;
     public Material nightSkybox;


>>>>>>> 0a5989b6fd4b22784c4c20e3b41f614aac0069e4
    public static DayNightCycle Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        currentTime += Time.deltaTime / dayDuration;
        if (currentTime >= 1f) currentTime = 0f;

        UpdateSun();
        UpdateAmbient();
    }

    void UpdateSun()
    {
        sun.transform.rotation = Quaternion.Euler((currentTime * 360f) - 90f, 170f, 0f);
        sun.color = sunColor.Evaluate(currentTime);
        sun.intensity = sunIntensity.Evaluate(currentTime);
    }

    void UpdateAmbient()
    {
        RenderSettings.ambientLight = ambientColor.Evaluate(currentTime);
<<<<<<< HEAD
=======
        if (IsNight())
            RenderSettings.skybox = nightSkybox;
        else
            RenderSettings.skybox = daySkybox;
>>>>>>> 0a5989b6fd4b22784c4c20e3b41f614aac0069e4
    }

    public bool IsNight()
    {
        return currentTime >= 0.75f || currentTime < 0.25f;
    }

    public float GetDrainMultiplier()
    {
        return IsNight() ? nightDrainMultiplier : dayDrainMultiplier;
    }

    public string GetTimeOfDay()
    {
        if (currentTime < 0.25f) return "Night";
        if (currentTime < 0.5f) return "Morning";
        if (currentTime < 0.75f) return "Afternoon";
        return "Night";
    }
}