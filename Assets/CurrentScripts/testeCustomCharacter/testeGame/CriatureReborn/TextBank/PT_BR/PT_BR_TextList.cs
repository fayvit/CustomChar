using UnityEngine;
using System.Collections.Generic;

namespace TextBankSpace
{
    public class PT_BR_TextList: TextListBase
    {
        static Dictionary<TextKey, List<string>> txt;
        public static Dictionary<TextKey, List<string>> Txt
        {
            get
            {
                if (txt == null)
                {
                    txt = new Dictionary<TextKey, List<string>>();

                    ColocaTextos(ref txt, KeyTextPT_BR.txt);
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