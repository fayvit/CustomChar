using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FayvitUI_10_2020
{
    public class A_MenuOption : AnOption
    {
        [SerializeField] private Text optionText;

        protected Text OptionText
        {
            get { return optionText; }
            set { optionText = value; }
        }

        public virtual void SetarOpcao(System.Action<int> optionAction, string optionText)
        {
            ThisAction += optionAction;
            OptionText.text = optionText;
        }
    }
}
