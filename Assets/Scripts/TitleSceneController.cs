using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    public CanvasGroup firstText;
    public CanvasGroup secondText;
    public float fadeDuration = 1.5f;
    public float showDuration = 2.5f;
    public string nextsceneName = "StartScene";

    void Start()
    {
        firstText.alpha = 0f;
        secondText.alpha = 0f;
        StartCoroutine(PlayTitleSquence());
    }
    IEnumerator PlayTitleSquence()
    {
        yield return StartCoroutine(FadeIn(firstText));
        yield return new WaitForSeconds(showDuration);
        yield return StartCoroutine(FadeOut(firstText));

        yield return StartCoroutine(FadeIn(secondText));
        yield return new WaitForSeconds(showDuration);
        yield return StartCoroutine(FadeOut(secondText));

        SceneManager.LoadScene("StartScene");
    }

    IEnumerator FadeIn(CanvasGroup cg)
    {
        float timer = 0f;
        cg.alpha = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            cg.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }

        cg.alpha = 1f;
    }

    IEnumerator FadeOut(CanvasGroup cg)
    {
        float timer = 0f;
        cg.alpha = 1f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            cg.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }

        cg.alpha = 0f;
    }
    void Update()
    {
        
    }
}
