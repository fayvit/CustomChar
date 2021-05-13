using FayvitCam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    public class FindBestTarget : MonoBehaviour
    {
        public static Transform Procure(GameObject esseObjeto, string[] tags, float distancia = 40)
        {
            return Procure(esseObjeto, EncontraveisComTag(tags), distancia);
        }

        public static Transform Procure(
            GameObject esseObjeto, 
            List<GameObject> encontraveisV, 
            float distancia = 40,
            bool melhorVisaoDaCamera = false)
        {
            Vector3 vendo;
            Vector3 c;


            GameObject alvo = null;

            List<GameObject> inimigosPerto = new List<GameObject>();
            Transform T = esseObjeto.transform;



            foreach (GameObject encontravel in encontraveisV)
            {
                if (encontravel != esseObjeto)
                {
                    c = encontravel.transform.position;
                    vendo = c - T.position;



                    if (vendo.sqrMagnitude < Mathf.Pow(distancia, 2))
                        inimigosPerto.Add(encontravel);
                }
            }



            if (inimigosPerto.Count != 0)
            {
                GameObject deMelhorVisao = null;
                GameObject maisPerto = null;
                Transform camT = melhorVisaoDaCamera? CameraAplicator.cam.transform:T;
                float targetDist = melhorVisaoDaCamera ? -0.25f : 0.5f;

                foreach (GameObject criature in inimigosPerto)
                {
                    c = criature.transform.position;
                    
                    maisPerto = maisPerto != null
                        ?
                            (c - T.position).sqrMagnitude
                            <
                            (maisPerto.transform.position - T.position).sqrMagnitude
                            ?
                            criature
                            :
                            maisPerto
                            : criature;

                    deMelhorVisao = deMelhorVisao == null
                        ?
                            criature
                            :
                            Vector3.Dot((c - camT.position).normalized,
                                         camT.forward)
                            >
                            Vector3.Dot(
                                (deMelhorVisao.transform.position - camT.position)
                                .normalized,
                                camT.forward
                                )
                            ?
                            criature
                            :
                            deMelhorVisao;
                }



                if (deMelhorVisao == maisPerto
                   &&
                   Vector3.Dot(
                    (deMelhorVisao.transform.position - T.position).normalized,
                    camT.forward) > 0)
                {
                    alvo = Vector3.Dot((maisPerto.transform.position -
                                        T.position).normalized,
                                       camT.forward) > targetDist
                        ? deMelhorVisao : null;
                }
                else
                {
                    if ((maisPerto.transform.position - T.position)
                       .sqrMagnitude < 25 &&
                       Vector3.Dot((maisPerto.transform.position -
                                 T.position).normalized,
                                T.forward) > targetDist
                       )
                        alvo = maisPerto;
                    else
                        alvo = Vector3.Dot((deMelhorVisao.transform.position -
                                            T.position).normalized,
                                           camT.forward) > targetDist
                            ? deMelhorVisao : null;
                }
            }

            //procurouAlvo = true;


            //AttackHelp(alvo, T);

            return alvo != null ? alvo.transform : null;
        }

        #region suprimido
        //private static void AttackHelp(GameObject alvo, Transform T)
        //{

        //    Vector3 gira;
        //    if (alvo != null)
        //    {
        //        gira = alvo.transform.position - T.position;

        //        gira.y = 0;
        //        T.rotation = Quaternion.LookRotation(gira);

        //    }
        //}
        #endregion

        private static List<GameObject> EncontraveisComTag(string[] tags)
        {
            List<GameObject> encontraveis = new List<GameObject>();
            for (int i = 0; i < tags.Length; i++)
            {
                encontraveis.AddRange(GameObject.FindGameObjectsWithTag(tags[i]));
            }

            return encontraveis;
        }

        public static List<GameObject> ProximosDoPonto(Vector3 pontoDeProximidade, string[] tags, float distancia = 40)
        {
            List<GameObject> retorno = new List<GameObject>();
            GameObject[] Gs = EncontraveisComTag(tags).ToArray();

            foreach (GameObject G in Gs)
            {
                if (Vector3.Distance(G.transform.position, pontoDeProximidade) < distancia &&
                   Vector3.Distance(G.transform.position, pontoDeProximidade) > 0 /*&&
               G.GetComponent<GerenciadorDeCriature>().MeuCriatureBase.CaracCriature.meusAtributos.PV.Corrente > 0*/)
                {
                    retorno.Add(G);
                }
            }
            return retorno;
        }

        public static List<GameObject> RemoveEu(List<GameObject> osPerto, GameObject eu)
        {
            bool remove = false;

            foreach (GameObject G in osPerto)
            {

                if (G == eu)
                    remove = true;
            }

            if (remove)
                osPerto.Remove(eu);

            return osPerto;
        }

        public static List<GameObject> ProximosDeMim(GameObject eu, string[] tags, float distancia = 40)
        {
            return RemoveEu(ProximosDoPonto(eu.transform.position, tags, distancia), eu);
        }
    }
}
