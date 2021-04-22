using UnityEngine;
using System.Collections;
//using GameJolt.API;

namespace CriaturesLegado
{
    public class SaveAndLoadInJolt
    {
        public static bool estaCarregado = false;
        public static void Save()
        {
            /*
            if (SaveDatesForJolt.s != null && Manager.Instance.CurrentUser != null)
            {
                Debug.Log("salvou: "+ Manager.Instance.CurrentUser.ID.ToString());
                byte[] sb = SaveDatesForJolt.SaveDatesForBytes();
                preJSON pre = new preJSON() { b = sb };

                DataStore.Set(Manager.Instance.CurrentUser.ID.ToString(),
                        JsonUtility.ToJson(pre), true,
                       Acertou);
            }*/
        }

        public static void Load()
        {
            /*
            if (Manager.Instance.CurrentUser != null)
            {

                DataStore.Get(Manager.Instance.CurrentUser.ID.ToString(), true, (string S2) => {
                    if (!string.IsNullOrEmpty(S2))
                    {
                        Debug.Log("Dados Carregados do Jolt");
                        SaveDatesForJolt.SetSavesWithBytes(JsonUtility.FromJson<preJSON>(S2).b);
                    }
                    else
                    {
                        Debug.Log("string nula do Jolt");
                        new SaveDatesForJolt();
                    }

                    GameObject.FindObjectOfType<LoginJoltManager>().StartCoroutine(Carregado());
                });
            }*/
        }

        static IEnumerator Carregado()
        {
            yield return new WaitForEndOfFrame();
            estaCarregado = true;
        }

        static void Acertou(bool foi)
        {
            if (foi)
            {
                Debug.Log("Deu certo" + SaveDatesForJolt.s.SavedGames[0].Posicao);
            }
            else
                Debug.Log("algo errado");
        }
    }


    [System.Serializable]
    public class preJSON
    {
        public byte[] b;
    }
}