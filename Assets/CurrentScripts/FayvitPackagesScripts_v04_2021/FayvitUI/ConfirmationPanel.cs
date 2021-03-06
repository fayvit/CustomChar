using FayvitMessageAgregator;
using UnityEngine;
using UnityEngine.UI;

namespace FayvitUI
{

    public class ConfirmationPanel : MonoBehaviour
    {
        public delegate void ConfirmationAction();
        public event ConfirmationAction yesBtn;
        public event ConfirmationAction noBtn;

#pragma warning disable 0649
        [SerializeField] private Text btnYesText;
        [SerializeField] private Text btnNoText;
        [SerializeField] private Text panelText;
        [SerializeField] private Image btnYesSelector;
        [SerializeField] private Image btnNoSelector;
#pragma warning restore 0649
        private bool selectedYes = false;
        private bool cancelIsNo = true;

        // Use this for initialization
        void Start()
        {

        }

        public void StartConfirmationPanel(
            ConfirmationAction yes,
            ConfirmationAction no,
            string textoDoPainel,
            bool selectedYes = false,
            bool cancelIsNo = true,
            bool hideSelections = false
            )
        {

            gameObject.SetActive(true);
            yesBtn += yes;
            noBtn += no;
            //selectedYes = !selectedYes;

            if (hideSelections)
            {
                btnNoSelector.enabled = false;
                btnYesSelector.enabled = false;
            }
            else
                ChangeSelectedOption(selectedYes);


            Debug.Log("Seleção é: "+selectedYes);

            this.selectedYes = selectedYes;
            this.panelText.text = textoDoPainel;

            this.cancelIsNo = cancelIsNo;
        }

        // Update is called once per frame
        public void ThisUpdate(bool changeOption, bool inputSelectedButton, bool inputCancel)
        {

            if (changeOption)
            {
                selectedYes = !selectedYes;
                ChangeSelectedOption(selectedYes);
            }

            if (inputSelectedButton)
            {

                if (selectedYes)
                    BtnYes();
                else
                    BtnNo();
            }
            else
            if (inputCancel)
            {
                if (cancelIsNo)
                    BtnNo();
                else
                    BtnYes();
            }
        }

        void ChangeSelectedOption(bool selection)
        {

            #region Suprimido
            //if (!selectedYes)
            //{

            //    //btnNoSelector.color = new Color(1, 1, 1, 1f);
            //    //btnYesSelector.color = new Color(1, 0, 0, 1);
            //}
            //else
            //{
            //    //btnYesSelector.color = new Color(1, 1, 1, 1f);
            //    //btnNoSelector.color = new Color(1, 0, 0, 1);
            //}
            #endregion

            btnYesSelector.enabled = selection;
            btnNoSelector.enabled = !selection;
        }        

        public void ChangeBtnYesText(string s)
        {
            btnYesText.text = s;
        }

        public void ChangeBtnNoText(string s)
        {
            btnNoText.text = s;
        }

        public void ChangePanelText(string s)
        {
            panelText.text = s;
        }

        public void ChangeTexts(string textoDoBotaoSim, string textoDoBotaoNao, string textoDoPainel)
        {
            this.panelText.text = textoDoPainel;
            this.btnNoText.text = textoDoBotaoNao;
            this.btnYesText.text = textoDoBotaoSim;
        }

        void ClearButtons()
        {
            yesBtn = null;
            noBtn = null;
        }

        public void BtnYes()
        {
            yesBtn();
            gameObject.SetActive(false);
            ClearButtons();
            MessageAgregator<ConfirmationPanelBtnYesMessage>.Publish();
            //EventAgregator.Publish(EventKey.confirmationPanelBtnYes);
        }

        public void BtnNo()
        {
            noBtn();
            gameObject.SetActive(false);
            ClearButtons();
            MessageAgregator<ConfirmationPanelBtnNoMessage>.Publish();
            //EventAgregator.Publish(EventKey.confirmationPanelBtnNo);
        }
    }
}