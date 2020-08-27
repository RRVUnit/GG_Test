using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HealthBarEmitter
    {
        private readonly List<Text> _texts = new List<Text>();
        private readonly Dictionary<Text, Vector2> _initialTextPositions = new Dictionary<Text, Vector2>();
        
        private List<Text> _textsToClear = new List<Text>();
        
        public void Add(Text text)
        {
            _texts.Add(text);
            SaveTextInitialPosition(text);
        }

        private void SaveTextInitialPosition(Text text)
        {
            Vector2 position = text.GetComponentInParent<RectTransform>().anchoredPosition;
            _initialTextPositions.Add(text, position);
        }

        public void Tick()
        {
            _texts.ForEach(MoveUp);

            if (_textsToClear.Count > 0) {
                _textsToClear.ForEach(RemoveTextInstance);
                _textsToClear = new List<Text>();
            }
        }

        private void MoveUp(Text textInstance)
        {
            RectTransform rectTransform = textInstance.GetComponentInParent<RectTransform>();
            Vector2 anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.y += 0.5f;
            rectTransform.anchoredPosition = anchoredPosition;

            if (anchoredPosition.y - _initialTextPositions[textInstance].y > 75) {
                _textsToClear.Add(textInstance);
            }
        }

        private void RemoveTextInstance(Text textInstance)
        {
            _texts.Remove(textInstance);
            _initialTextPositions.Remove(textInstance);
            GameObject.Destroy(textInstance.gameObject);
        }
    }
}