using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private string _startText;
    private Coroutine _animCoroutine;
    short count = 0;

    private void OnEnable()
    {
        count = 0;
        _animCoroutine = StartCoroutine(AnimationText());
        _startText = text.text;
    }

    private void OnDisable()
    {
        StopCoroutine(_animCoroutine);
        count = 0;
        text.text = _startText;
    }

    private IEnumerator AnimationText()
    {
        while (true)
        {
            if (count == 4)
            {
                text.text += "....";
                count = 0;
            }

            yield return new WaitForSeconds(0.3f);
            text.text = text.text.Remove(text.text.Length - 1);
            count++;
        }
    }
}