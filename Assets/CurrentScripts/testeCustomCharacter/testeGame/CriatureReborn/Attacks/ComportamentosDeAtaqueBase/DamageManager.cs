using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;

namespace Criatures2021
{
    public class DamageManager
    {

        public static void VerificaDano(GameObject atacado, GameObject atacante, PetAttackBase golpe)
        {
            if (atacado.tag == "eventoComGolpe" /*&& !GameController.g.estaEmLuta*/)
            {
                Debug.LogWarning("Evento com golpe removido, necessário refazer, condicional de esta em luta removido");
                //atacado.GetComponent<EventoComGolpe>().DisparaEvento(golpe.Nome);
            }

            PetManager GdC = atacado.GetComponent<PetManager>();
            if (GdC /*&& !GameController.g.UsandoItemOuTrocandoCriature*/)
            {
                Debug.Log("Um condicional para impedir dano em uso de item foi retirado");
                if (GdC.MeuCriatureBase.PetFeat.meusAtributos.PV.Corrente > 0)
                {
                    AplicaDano(GdC, atacante, golpe);
                }
            }
        }

        //public static void EmEstadoDeDano(Animator animatorDoAtacado, PetManager doAtacado)
        //{
        //    Debug.LogError("não sei o que fazer com isso");
        //    //doAtacado.MudaParaEstouEmDano();
        //    //Transform T = doAtacado.transform;

        //    //  doAtacado.MudaEmDano();

        //    animatorDoAtacado.Play("dano2");
        //    animatorDoAtacado.SetBool("dano1", true);
        //    animatorDoAtacado.Play("dano1");
        //}

        public static void InsereEstouEmDano(GameObject doAtacado,PetAttackBase golpe,GameObject atacante)
        {
            MessageAgregator<MsgEnterInDamageState>.Publish(new MsgEnterInDamageState() { 
                oAtacado = doAtacado.gameObject,
                golpe = golpe,
                atacante = atacante
            });
            //EstouEmDano eED = doAtacado.gameObject.AddComponent<EstouEmDano>();
            //eED.esseGolpe = golpe;
            //eED.animator = animatorDoAtacado;
            //eED.gerente = doAtacado;
        }

        public static void AplicaDano(PetManager doAtacado, GameObject atacante, PetAttackBase golpe)
        {
            //Animator animatorDoAtacado = doAtacado.GetComponent<Animator>();
            //EmEstadoDeDano(animatorDoAtacado, doAtacado);

            CalculaDano(doAtacado, atacante, golpe);

            InsereEstouEmDano(doAtacado.gameObject, golpe,atacante);

            VerificaVida(atacante, doAtacado);
        }

        static void VerificaVida(GameObject atacante, PetManager doAtacado)
        {
            ConsumableAttribute A = doAtacado.MeuCriatureBase.PetFeat.meusAtributos.PV;
            if (A.Corrente <= 0)
            {
                
                MessageAgregator<MsgCriatureDefeated>.Publish(new MsgCriatureDefeated() 
                { 
                    defeated = doAtacado.gameObject,
                    atacker = atacante,
                    doDerrotado = doAtacado.MeuCriatureBase
                });
                Debug.LogError("Morte não implementada");
                //a.SetBool("cair", true);

                //UnityEngine.AI.NavMeshAgent nav = a.GetComponent<UnityEngine.AI.NavMeshAgent>();
                //if (nav.enabled)
                //    nav.Stop();// nav.isStopped = true;

                /*   doAtacado.MudaParaDerrotado();

                   if (!doAtacado.gerenteCri)
                   {
                       AplicaSelvagemDerrotado aSD =  doAtacado.gameObject.AddComponent<AplicaSelvagemDerrotado>();
                       aSD.oDerrotado = doAtacado;
                       aSD.oQDerrotou = atacante.GetComponent<GerenciadorDeCriature>();

                   }
                   else
                   {
                       //Morte de um criature selvagem
                   }
                   */
            }
        }

        static void CalculaDano(PetManager doAtacado, GameObject atacante, PetAttackBase golpe)
        {
            float multiplicador = 1;

            for (int i = 0; i < doAtacado.MeuCriatureBase.PetFeat.contraTipos.Length; i++)
            {
                if (golpe.Tipo.ToString() == doAtacado.MeuCriatureBase.PetFeat.contraTipos[i].Nome)
                {
                    multiplicador *= doAtacado.MeuCriatureBase.PetFeat.contraTipos[i].Mod;
                }
            }

            PetBase cDoAtacante = atacante.GetComponent<PetManager>().MeuCriatureBase;
            PetAtributes A = cDoAtacante.PetFeat.meusAtributos;

            int potenciaDoAtacante = (golpe.Caracteristica == AttackDiferentialId.colisao)
                ?
                    Mathf.RoundToInt(A.Ataque.Minimo + (A.Ataque.Corrente - A.Ataque.Minimo) * Random.Range(0.85f, 1))
                :
                    Mathf.RoundToInt(A.Poder.Minimo + (A.Poder.Corrente - A.Poder.Minimo) * Random.Range(0.85f, 1));

            int numStatus = StatusTemporarioBase.ContemStatus(StatusType.fraco, cDoAtacante);
            if (numStatus > -1)
            {
                potenciaDoAtacante = (int)Mathf.Max(1 / cDoAtacante.StatusTemporarios[numStatus].Quantificador * potenciaDoAtacante, (A.Ataque.Minimo + A.Poder.Minimo) / 2);
                golpe.ModCorrente = -(int)cDoAtacante.StatusTemporarios[numStatus].Quantificador;
            }
            else
                golpe.ModCorrente = 0;

            PetAttackDb golpePersonagem = cDoAtacante.GerenteDeGolpes.ProcuraGolpeNaLista(cDoAtacante.NomeID, golpe.Nome);

            CalculoC(multiplicador, golpe, golpePersonagem, potenciaDoAtacante, doAtacado, cDoAtacante);

            golpe.VerificaAplicaStatus(cDoAtacante, doAtacado);
        }

        static void CalculoC(
            float multiplicador,
            PetAttackBase golpe,
            PetAttackDb golpePersonagem,
            int potenciaDoAtacante,
            PetManager doAtacado,
            PetBase cDoAtacado)
        {
            PetAtributes aDoAtacado = doAtacado.MeuCriatureBase.PetFeat.meusAtributos;
            float rd = Random.Range(0.85f, 1);
            int level = cDoAtacado.PetFeat.mNivel.Nivel;
            float STAB = 1;
            if (cDoAtacado.PetFeat.TemOTipo(golpe.Tipo))
            {
                STAB = 1.5f;
            }
            Debug.Log("modificador de potencia para esse golpe é " + golpePersonagem.ModPersonagem);
            //int  dano = (int)((((((((2 * level / 5) + 2) * potenciaDoAtacante* 20*(golpe.PotenciaCorrente+golpePersonagem.ModPersonagem) )/ aDoAtacado.Defesa.Corrente)/ 50) +2) *STAB * multiplicador) *rd / 100);
            int dano = (int)(((2 * level) * potenciaDoAtacante * (golpe.PotenciaCorrente + golpePersonagem.ModPersonagem) / (45f*aDoAtacado.Defesa.Corrente + 250) + 2) * STAB * multiplicador * rd);
            AplicaCalculoComVIsaoDeDano(doAtacado, golpe, aDoAtacado, multiplicador, dano, aDoAtacado.Defesa.Corrente, potenciaDoAtacante);
        }

        static void AplicaCalculoComVIsaoDeDano(PetManager doAtacado,
            PetAttackBase golpe,
            PetAtributes aDoAtacado,
            float multiplicador,
            int dano,
            int defesa,
            int potenciaDoAtacante)
        {
            AplicaCalculoDoDano(aDoAtacado, dano);

            Debug.Log("O dano do Golpe e " + dano + " O nome do golpe e " + golpe.Nome + " o multiplicador e" + multiplicador
                + " A defesa do inimigo é " + defesa
                + " A potencia original é " + potenciaDoAtacante
                + " A potencia do Golpe é " + golpe.PotenciaCorrente);

            AplicaVisaoDeDano(doAtacado, dano,multiplicador);
        }

        public static void AplicaVisaoDeDano(PetManager doAtacado, int dano,float mod)
        {
            GameObject G = null;
            if (mod < .25f)
            {
                G = Resources.Load<GameObject>("DamageView/DanoMuitoReduzido");
            }
            else if (mod < .8f)
            {
                G = Resources.Load<GameObject>("DamageView/DanoReduzido");
            }
            else if (mod < 1.2f)
            {
                G = Resources.Load<GameObject>("DamageView/DanoNormal");
            }
            else if (mod < 1.8f)
            {
                G = Resources.Load<GameObject>("DamageView/DanoAumentado");
            }else
                G = Resources.Load<GameObject>("DamageView/DanoMuitoAumentado");


            //Debug.LogError("temos que mudar visao de dano para um evento evnviado ao gamecontroller");
            //GameObject visaoDeDano = null;// GameController.g.El.retorna("visaoDeDano");
            GameObject visaoDeDano = (GameObject)MonoBehaviour.Instantiate(G, doAtacado.transform.position, Quaternion.identity);
            DanoAparecendo danoAp = visaoDeDano.GetComponent<DanoAparecendo>();
            danoAp.dano = dano.ToString();
            danoAp.atacado = doAtacado.transform;

            /* INSERIDO PARA ATUALIZAR A HUD VIDA */
            //GameController.g.HudM.AtualizaDadosDaHudVida(false);
            //if (GameController.g.estaEmLuta)
            //    GameController.g.HudM.AtualizaDadosDaHudVida(true);
        }

        static void CalculoB(float multiplicador, PetAttackBase golpe, PetAttackDb golpePersonagem, int potenciaDoAtacante, PetManager doAtacado)
        {
            PetAtributes aDoAtacado = doAtacado.MeuCriatureBase.PetFeat.meusAtributos;

            int defesa = Mathf.RoundToInt(aDoAtacado.Defesa.Corrente * Random.Range(0.85f, 1));
            int dano = (int)(multiplicador * (golpe.PotenciaCorrente + golpePersonagem.ModPersonagem + potenciaDoAtacante / defesa));

            AplicaCalculoComVIsaoDeDano(doAtacado, golpe, aDoAtacado, multiplicador, dano, defesa, potenciaDoAtacante);
        }

        static void CalculoA(float multiplicador, PetAttackBase golpe, PetAttackDb golpePersonagem, int potenciaDoAtacante, PetManager doAtacado)
        {

            int dano = Mathf.Abs(
                Mathf.RoundToInt(
                    multiplicador * (golpe.PotenciaCorrente
                                + golpePersonagem.ModPersonagem
                                + potenciaDoAtacante)
                                ));

            potenciaDoAtacante = dano;

            PetAtributes aDoAtacado = doAtacado.MeuCriatureBase.PetFeat.meusAtributos;

            int defesa = Mathf.RoundToInt(aDoAtacado.Defesa.Corrente * Random.Range(0.85f, 1));

            if (defesa < 0.75f * dano)
                dano = (dano - defesa >= 1) ? dano - defesa : 1;
            else
                dano = (int)(0.25f * dano) >= 1 ? Mathf.Max((int)(0.25f * dano * Random.Range(0.9f, 1.15f)), 1) : 1;



            AplicaCalculoComVIsaoDeDano(doAtacado, golpe, aDoAtacado, multiplicador, dano, defesa, potenciaDoAtacante);

    
        }

        public static void AplicaCalculoDoDano(PetAtributes A, int dano)
        {
            if (A.PV.Corrente - dano > 0)
                A.PV.Corrente -= dano;
            else
                A.PV.Corrente = 0;
        }


    }

    public struct MsgEnterInDamageState : IMessageBase
    {
        public GameObject atacante;
        public GameObject oAtacado;
        public PetAttackBase golpe;        
    }

    public struct MsgCriatureDefeated : IMessageBase
    {
        public GameObject defeated;
        public GameObject atacker;
        public PetBase doDerrotado;
    }

}