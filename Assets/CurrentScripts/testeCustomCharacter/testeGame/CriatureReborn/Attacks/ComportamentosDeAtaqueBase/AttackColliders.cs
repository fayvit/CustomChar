using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class AttackColliders
    {
        public static colisor GetCollider(GameObject G, AttackNameId nomeColisor)
        {
            PetBase criatureBase = G.GetComponent<PetManager>().MeuCriatureBase;
            PetAttackManager gg = criatureBase.GerenteDeGolpes;
            PetAttackDb gP = gg.ProcuraGolpeNaLista(criatureBase.NomeID, nomeColisor);

            return gP.Colisor;
                /*GolpePersonagem.RetornaGolpePersonagem(G, nomeColisor).Colisor;*/

        }
        public static void AdicionaOColisor(GameObject G,
                                            PetAttackBase golpeAtivo,
                                            ImpactFeatures caracteristica,
                                            float tempoDecorrido)
        {
            GameObject view = Resources.Load<GameObject>("DamageColliders/"+caracteristica.nomeTrail.ToString());
            //GameObject view = GameController.g.El.retornaColisor(caracteristica.nomeTrail);
            //		print(nomeColisor);
            colisor C = GetCollider(G, golpeAtivo.Nome);// = new colisor();

            GameObject view2 = (GameObject)MonoBehaviour.Instantiate(view, C.deslColisor, Quaternion.identity);

            view2.transform.localRotation = view.transform.rotation;


            if (caracteristica.parentearNoOsso)
                view2.transform.parent = G.transform.Find(C.osso).transform;
            else
                view2.transform.parent = G.transform;

            view2.transform.localPosition = C.deslTrail;
            view2.transform.localRotation = view.transform.rotation;
            view2.GetComponent<BoxCollider>().center = C.deslColisor;
            view2.name = "colisor" + golpeAtivo.Nome.ToString();
            view2.transform.localScale *= (golpeAtivo.ColisorScale * C.ColisorScale);


            /*
                    PARA DESTUIR O COLISOR .
                    QUANDO O GOLPE ERA INTERROMPIDO 
                    O COLISOR PERMANECIA NO PERSONAGEM
                 */
            MonoBehaviour.Destroy(view2, golpeAtivo.TempoDeDestroy - tempoDecorrido);


            /*************************************************************/


            DamageCollider proj = view2.AddComponent<DamageCollider>();
            proj.velocidadeProjetil = 0f;
            proj.noImpacto = caracteristica.noImpacto;
            proj.dono = G;
            proj.esseGolpe = golpeAtivo;
            //			proj.forcaDoDano = 25f;
            //addView = true;
        }
    }

}