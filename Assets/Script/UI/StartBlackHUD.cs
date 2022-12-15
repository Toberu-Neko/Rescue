using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBlackHUD : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private GameObject blackBG;
    void Start()
    {
        blackBG = transform.Find("BlackBackground").gameObject;
        blackBG.SetActive(true);
        canvasGroup = blackBG.GetComponent<CanvasGroup>();
        StartCoroutine(AlphaChanger());
    }

    IEnumerator AlphaChanger()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= .1f;
            yield return new WaitForSecondsRealtime(0.08f);
        }
        blackBG.SetActive(false);
    }
}
