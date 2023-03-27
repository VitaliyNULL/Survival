using System.Collections;
using TMPro;
using UnityEngine;

namespace VitaliyNULL.UI
{
    public class LoadingAnimation : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        private string _startText;
        private Coroutine _animCoroutine;
        short _count = 0;

        private void OnEnable()
        {
            _count = 0;
            _animCoroutine = StartCoroutine(AnimationText());
            _startText = text.text;
        }

        private void OnDisable()
        {
            StopCoroutine(_animCoroutine);
            _count = 0;
            text.text = _startText;
        }

        private IEnumerator AnimationText()
        {
            while (true)
            {
                if (_count == 4)
                {
                    text.text += "....";
                    _count = 0;
                }

                yield return new WaitForSeconds(0.3f);
                text.text = text.text.Remove(text.text.Length - 1);
                _count++;
            }
        }
    }
}