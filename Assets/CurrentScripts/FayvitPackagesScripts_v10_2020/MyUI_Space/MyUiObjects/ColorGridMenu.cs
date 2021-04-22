using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FayvitUI_10_2020
{
    [System.Serializable]
    public class ColorGridMenu : BaseGridMenu
    {
        private Color[] colors;
        public void StartHud(System.Action<int> acaoDeFora, Color[] colors)
        {
            ThisAction += acaoDeFora;
            this.colors = colors;

            if (colors.Length > 0)
                StartHud(colors.Length, ResizeUiType.grid);
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
