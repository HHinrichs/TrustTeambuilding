using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class SWARMController : MonoBehaviour
{

    public float lerpLength = 0;
    public float maxValue = 0f;
    public float minValue = 0f;
    VisualEffect visualEffect;
    private void Awake()
    {
        visualEffect = GetComponent<VisualEffect>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartPingPongValue());
    }

    IEnumerator StartPingPongValue()
    {
        float currentLerpTime = 0f;

        while (true) {

            Debug.Log(Mathf.Lerp(minValue, maxValue, currentLerpTime));
            visualEffect.SetFloat("intensity", Mathf.Lerp(minValue, maxValue, currentLerpTime / lerpLength));

            currentLerpTime += Time.deltaTime;

            if(currentLerpTime > lerpLength)
            {
                float temp = maxValue;
                maxValue = minValue;
                minValue = temp;
                currentLerpTime = 0f;
            }

            yield return null;
        }
    }

}
