using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextBankSpace
{
    public class EN_G_TextList : MonoBehaviour
    {
        static Dictionary<TextKey, List<string>> txt;
        public static Dictionary<TextKey, List<string>> Txt
        {
            get
            {
                if (txt == null)
                {
                    txt = new Dictionary<TextKey, List<string>>();

                    //ColocaTextos(ref txt, TextosChaveEmPT_BR.txt);
                    //ColocaTextos(ref txt, TextosDeBarreirasPT_BR.txt);
                    //ColocaTextos(ref txt, TextosDaCavernaInicialPT_BR.txt);
                    //ColocaTextos(ref txt, TextosDeKatidsPT_BR.txt);
                    //ColocaTextos(ref txt, TextosDeMarjanPT_BR.txt);
                    //ColocaTextos(ref txt, TextosDeInfoPT_BR.txt);
                }

                return txt;
            }
        }
    }
}