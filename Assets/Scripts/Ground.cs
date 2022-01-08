using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private void Start()
    {

    }

    public IEnumerator Blink(float time)
    {
        float endTime = Time.time + time;
        while (Time.time < endTime)
        {
            iTween.FadeTo(gameObject, 0f, 0.2f);
            yield return new WaitForSeconds(0.2f);
            iTween.FadeTo(gameObject, 1f, 0.2f);
            yield return new WaitForSeconds(0.2f);
        }
        gameObject.SetActive(false);
    }
}
