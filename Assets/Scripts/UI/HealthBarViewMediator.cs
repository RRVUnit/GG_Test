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

        public void EmitHPRestore(int hpAmount)
        {
            EmitText(hpAmount, false);
        }

        public void EmitHit(int hitAmount)
        {
            EmitText(hitAmount, true);
        }
        
        private void EmitText(int amount, bool isNegative)
        {
            
        }
    }
}