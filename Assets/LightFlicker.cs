using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Rendering.Universal;
using NaughtyAttributes;
using TigerForge;

public class LightFlicker : MonoBehaviour
{
    [Title("Intensity Flicker")]
    public bool usingIntensityFlicker;
    [PropertyTooltip("Min and Max intensity as a % of base intensity")]
    public Vector2 intensityRange;
    [PropertyTooltip("Min and Max time range in seconds")]
    public Vector2 intensityTimeRange;
    public int flickerSteps;

    [Title("Position Flicker")]
    public bool usingPositionFlicker;
    public Vector2 PositionRange;
    public Vector2 PositionTimeRange;
    public int positionSteps;

    [Title("Color Flicker")]
    public bool usingColorFlicker;
    public Vector2 ColorRange;
    public Vector2 ColorTimeRange;

    private float baseIntensity;
    private float ctimeIntensity;
    private float intensityTime;
    private float intensityMagnitude;

    private float ctimePosition;
    private float positionTime;
    private Vector2 positionMagnitude;
    private bool isChangingPos;

    private Light2D Light;
    // Start is called before the first frame update
    void Awake()
    {
        Light = GetComponent<Light2D>();
        ctimeIntensity = -2;
        ctimePosition = -2;
        baseIntensity = Light.intensity;
        Light.intensity = 0.1f * baseIntensity;
        StartupLight();
        intensityTime = Random.Range((intensityTimeRange.x), (intensityTimeRange.y));
        intensityMagnitude = Random.Range(baseIntensity * intensityRange.x, baseIntensity * intensityRange.y);

        positionTime = Random.Range((PositionTimeRange.x), (PositionTimeRange.y));
        positionMagnitude = new Vector2(Random.Range(-PositionRange.x, PositionRange.x), Random.Range(0, PositionRange.y));
        EventManager.StartListening("BURSTLIGHT", burstLight);
    }
    void StartupLight()
    {
        StartCoroutine(StartupIntensity());
    }
    // Update is called once per frame
    void Update()
    {
        if(usingIntensityFlicker)
        {
            ctimeIntensity += Time.deltaTime;
        }
        if(usingPositionFlicker)
        {
            ctimePosition += Time.deltaTime;
        }
        

        if (ctimeIntensity > intensityTime)
        {
            ctimeIntensity -= intensityTime;
            StartCoroutine(FlickerIntensity());
            intensityTime = Random.Range(intensityTimeRange.x, intensityTimeRange.y);
            intensityMagnitude = Random.Range(baseIntensity * intensityRange.x, baseIntensity * intensityRange.y);
        }

        if (ctimePosition > positionTime)
        {
            ctimePosition -= intensityTime;
            if(!isChangingPos)
            {
                StartCoroutine(FlickerPosition());
            }
            
            positionTime = Random.Range((PositionTimeRange.x), (PositionTimeRange.y));
            positionMagnitude = new Vector2(Random.Range(-PositionRange.x, PositionRange.x), Random.Range(0, PositionRange.y));

        }
    }

    void burstLight()
    {
        //Suspend timers
        ctimePosition = -999;
        ctimeIntensity = -999;

        StartCoroutine(BurstIntensity());
    }

    private IEnumerator BurstIntensity()
    {
        float stepchange = (2 * baseIntensity - Light.intensity) / (flickerSteps * 2);
        for (int i = flickerSteps; i >= 0; i--)
        {
            Light.intensity += stepchange;
            Light.pointLightOuterRadius += 0.08f;
            yield return new WaitForSeconds(0.07f);
        }

    }

    private IEnumerator StartupIntensity()
    {
        float stepchange = (baseIntensity - Light.intensity) / (flickerSteps * 2);
        for (int i = flickerSteps; i >= 0; i--)
        {
            Light.intensity += stepchange;
            yield return new WaitForSeconds(0.07f);
        }

    }

    private IEnumerator FlickerIntensity()
    {
        float stepchange = (intensityMagnitude - Light.intensity) / flickerSteps;
        for (int i = flickerSteps; i >= 0; i--)
        {
            Light.intensity += stepchange;
            yield return new WaitForSeconds(0.08f);
        }
        
    }

    private IEnumerator FlickerPosition()
    {
        isChangingPos = true;
        
        Vector2 stepchange = new Vector2(positionMagnitude.x - Light.transform.localPosition.x, (positionMagnitude.y - Light.transform.localPosition.y));
        stepchange /= positionSteps;

        for (int i = positionSteps; i >= 0; i--)
        {
            //Debug.Log(i + "  " + stepchange + "  " + positionMagnitude + "  " + (Vector2)Light.transform.localPosition);
            Light.transform.localPosition += new Vector3(stepchange.x, stepchange.y, 0);
            yield return new WaitForSeconds(0.08f);
        }
        
        isChangingPos = false;
    }
}
