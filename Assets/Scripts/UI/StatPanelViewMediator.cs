using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class StatPanelViewMediator : MonoBehaviour
    {
        [SerializeField]
        public Text Text;

        [SerializeField]
        public Image Icon;

        public string IconName
        {
            set
            {
                string iconPath = "Icons/" + value;
                Icon.sprite = Resources.Load<Sprite>(iconPath);
            }
        }
        public float Value {
            set { Text.text = value.ToString(); }
        }
    }
}