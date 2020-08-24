using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    public float fadeSpeed = 5;
    private BoxCollider correspondingBoxCollider;
    [SerializeField] GameObject mainCameraGameObject;
    private Coroutine fadeInCoroutine;
    private Coroutine fadeOutCoroutine;
    [SerializeField] bool useFadeToBlackMechanism = true;
    public BoxCollider SetCorrespondingBoxCollider { set { correspondingBoxCollider = value; } }
    private void Start()
    {
        fadeImage = GetComponentInChildren<Image>();
        if (fadeImage == null)
            return;

        StartCoroutine(CheckToFade());
    }

    IEnumerator CheckToFade()
    {
        while (correspondingBoxCollider == null)
            yield return null;

        while(correspondingBoxCollider != null)
        {
            if (!useFadeToBlackMechanism)
                yield break;

            if (correspondingBoxCollider.bounds.Contains(mainCameraGameObject.transform.position))
            {
                FadeIn();
            }
            else
            {
                FadeOut();
            }
        }
    }
    public void FadeOut()
    {
        if (fadeInCoroutine != null)
            return;

        if(fadeOutCoroutine == null)
        {
            if (fadeImage.color.a == 1)
                return;
            fadeOutCoroutine = StartCoroutine(FadeInCoroutine());
        }
    }

    public void FadeIn()
    {
        if (fadeOutCoroutine != null)
            return;

        if(fadeInCoroutine == null)
        {
            if (fadeImage.color.a == 0) 
                return;
            fadeInCoroutine = StartCoroutine(FadeOutCoroutine());
        }
    }

    IEnumerator FadeInCoroutine()
    {
        Color objectColor = fadeImage.color;
        float fadeAmount;
        while(fadeImage.color.a > 0)
        {
            fadeAmount = fadeImage.color.a - (fadeSpeed + Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            fadeImage.color = objectColor;
            yield return null;
        }
        objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, 0f);

        fadeOutCoroutine = null;
    }

    IEnumerator FadeOutCoroutine()
    {
        Color objectColor = fadeImage.color;
        float fadeAmount;
        while (fadeImage.color.a < 1)
        {
            fadeAmount = fadeImage.color.a + (fadeSpeed + Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            fadeImage.color = objectColor;
            yield return null;
        }
        objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, 1f);
        fadeInCoroutine = null;
    }
}
