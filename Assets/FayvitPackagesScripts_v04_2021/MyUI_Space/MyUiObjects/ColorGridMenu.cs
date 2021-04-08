using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FayvitUI
{
    [System.Serializable]
    public class ColorGridMenu : BaseGridMenu
    {
        private Color[] colors;

        public Color GetSelectedColor
        {
            get
            {
                AnImageOption[] umas = variableSizeContainer.GetComponentsInChildren<AnImageOption>();
                return umas[SelectedOption].OptionImage.color;
            }
        }

        public void StartHud(System.Action<int> acaoDeFora, Color[] colors,int selectIndex=0)
        {
            ThisAction += acaoDeFora;
            this.colors = colors;

            if (colors.Length > 0)
                StartHud(colors.Length, ResizeUiType.grid,selectIndex);
            else
                aContainerItem.SetActive(false);

            SetLineRowLength();

        }

        public override void SetContainerItem(GameObject G, int indice)
        {

            AnImageOption uma = G.GetComponent<AnImageOption>();

            Image I = uma.OptionImage;
            I.color = colors[indice];

            uma.SetarOpcoes(I.sprite, ThisAction);
        }
       
    }
}
