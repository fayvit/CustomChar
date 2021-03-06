using UnityEngine;
using System.Collections.Generic;
using FayvitSave;

namespace TextBankSpace
{
    public class TextBank {
        public static LanguageKey linguaChave = LanguageKey.pt_br;
        public static readonly Dictionary<LanguageKey, Dictionary<TextKey, List<string>>> falacoesComChave
        = new Dictionary<LanguageKey, Dictionary<TextKey, List<string>>>() {
        { LanguageKey.pt_br,
            PT_BR_TextList.Txt
        },
        { LanguageKey.en_google,
            EN_G_TextList.Txt
        }
        };

        public static readonly Dictionary<LanguageKey, Dictionary<InterfaceTextKey, string>> textosDeInterface
            = new Dictionary<LanguageKey, Dictionary<InterfaceTextKey, string>>() {
            {
                LanguageKey.pt_br,
                InterfaceTextList.txt
            },
            {
                LanguageKey.en_google,
                InterfaceTextListEN_G.txt
            }
            };

        //public static readonly Dictionary<LanguageKey, Dictionary<IndiceDeArmagedoms, string>> nomesArmagedoms
        //    = new Dictionary<LanguageKey, Dictionary<IndiceDeArmagedoms, string>>() {
        //    {
        //        LanguageKey.pt_br,
        //        NomeDeArmagedomPT_BR.n
        //    },
        //    {
        //        LanguageKey.en_google,
        //        NomeDeArmagedomEN_G.n
        //    }
        //    };

        public static void VerificaChavesFortes(LanguageKey primeiro, LanguageKey segundo)
        {
            if (falacoesComChave.ContainsKey(primeiro) && falacoesComChave.ContainsKey(segundo))
            {
                Dictionary<TextKey, List<string>>.KeyCollection keys = falacoesComChave[primeiro].Keys;

                foreach (TextKey k in keys)
                {
                    if (falacoesComChave[segundo].ContainsKey(k))
                    {
                        if (falacoesComChave[segundo][k].Count != falacoesComChave[primeiro][k].Count)
                        {
                            Debug.Log("As listas de mensagem no indice " + k + " tem tamanhos diferentes");
                        }
                    }
                    else
                    {
                        Debug.Log("A lista " + segundo + " nao contem a chave: " + k);
                    }
                }
            }
            else
            {
                Debug.Log("Falacoes nao contem alguma das chaves de LanguageKey");
            }

            //if (nomesArmagedoms.ContainsKey(primeiro) && nomesArmagedoms.ContainsKey(segundo))
            //{
            //    Dictionary<IndiceDeArmagedoms, string>.KeyCollection keys = nomesArmagedoms[primeiro].Keys;

            //    foreach (IndiceDeArmagedoms k in keys)
            //    {
            //        if (!nomesArmagedoms[segundo].ContainsKey(k))
            //        {
            //            Debug.Log("A lista " + segundo + " nao contem a chave de armagedom: " + k);
            //        }
            //    }
            //}
            //else
            //{
            //    Debug.Log("NomesArmagedoms nao contem alguma das chaves de LanguageKey");
            //}
        }

        public static List<string> RetornaListaDeTextoDoIdioma(TextKey chave)
        {
            return falacoesComChave[linguaChave][chave];
        }

        public static string RetornaFraseDoIdioma(TextKey chave)
        {
            return falacoesComChave[linguaChave][chave][0];
        }

        public static string RetornaTextoDeInterface(InterfaceTextKey chave)
        {
            return textosDeInterface[linguaChave][chave];
        }
    }

    public enum TextKey
    { 
        bomDia,
        apresentaInimigo,
        usoDeGolpe,
        nomesDosGolpes,
        listaDeItens,
        emQuem,
        itens,
        mensLuta,
        tentaCapturar,
        foiParaArmagedom,
        criatureParaMostrador,
        frasesDeArmagedom,
        apresentaDerrota,
        shopInfoItem,
        bau,
        textoBaseDeAcao,
        simOuNao,
        nomeTipos
    }
}