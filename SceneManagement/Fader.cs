using System;
using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentActiveRoutine;

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }
        public IEnumerator FadeOut(float time)
        {
           return Fade(1,time);
        }

        public IEnumerator FadeIn(float time)
        {
            return Fade(0,time);
        }

        private IEnumerator Fade(float target, float time)
        {
            if(currentActiveRoutine!= null)
            {
                StopCoroutine(currentActiveRoutine);
            }
            currentActiveRoutine= StartCoroutine(FadeRoutine(target,time));
            yield return currentActiveRoutine;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {   
            while(!Mathf.Approximately(canvasGroup.alpha,target))
            {
                canvasGroup.alpha=Mathf.MoveTowards(canvasGroup.alpha,target,Time.deltaTime / time);
                yield return null;
            }
        }
    }
} 