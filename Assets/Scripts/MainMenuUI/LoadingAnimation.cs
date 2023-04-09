using System.Collections;
using TMPro;
using UnityEngine;

namespace VitaliyNULL.MainMenuUI
{
    public class LoadingAnimation : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private TMP_Text text;
        private string _startText;
        private Coroutine _animCoroutine;
        short _count = 0;

        #endregion

        #region MonoBehaviour Callbacks

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

        #endregion

        #region Private Methods

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

        #endregion
    }
}