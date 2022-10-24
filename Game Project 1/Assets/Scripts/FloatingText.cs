using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    TMP_Text text;

    public AnimationCurve fadeCurve;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void Spawned(string content, float speed, float duration, Color color)
    {
        text.text = content;

        //text.color = color;

        StartCoroutine(TextMover(speed, duration));
    }

    IEnumerator TextMover(float speed, float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            text.color = new Color(text.color.r, text.color.g, text.color.b, fadeCurve.Evaluate(timer / duration));
            transform.position += Vector3.up * Time.deltaTime * speed;
            yield return null;
        }

        text.text = "";



    }

}
