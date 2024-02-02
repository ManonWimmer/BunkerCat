using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelFader : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 0.4f;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public bool isFaded = false;
    public void Fade()
    {
        StartCoroutine(FadeFromZero());
    }

    public IEnumerator FadeFromZero()
    {
        float counter = 0;

        while(counter < fadeDuration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, counter / fadeDuration);

            yield return null;
        }

        isFaded = true;
    }
}
