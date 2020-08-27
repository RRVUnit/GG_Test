using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HealthBarViewMediator : MonoBehaviour
    {
        [SerializeField]
        public Text Text;

        [SerializeField]
        public Image Icon;

        private int _value;
        
        public int MaxValue { private get; set; }

        private void Awake()
        {
            Text.gameObject.SetActive(false);
        }

        public int Value
        {
            private get { return _value; }
            set
            {
                _value = value;
                UpdateIconWidth();
            }
        }

        private void UpdateIconWidth()
        {
            float iconFillAmount = (float) _value / (float) MaxValue;
            Icon.fillAmount = iconFillAmount;
        }

        public Text CreateHPRestoreText(int hpAmount)
        {
            return CreateTextEmitter(hpAmount, false);
        }

        public Text CreateHitText(int hitAmount)
        {
            return CreateTextEmitter(hitAmount, true);
        }
        
        private Text CreateTextEmitter(int amount, bool isNegative)
        {
            Text textInstance = Instantiate(Text, transform.parent, true);
            string prefix = isNegative ? "-" : "+";
            Color color = isNegative ? Color.red : Color.green;
            textInstance.color = color;
            textInstance.text = prefix + amount;
            textInstance.gameObject.SetActive(true);
            return textInstance;
        }
    }
}