using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using FayvitBasicTools;

namespace Criatures2021
{
    public class StandardImpactUpdate
    {
        private bool procurouAlvo = false;
        private bool addView = false;
        private float tempoDecorrido = 0;
        private Transform alvoProcurado;
        private CharacterController controle;

        public void ReiniciaAtualizadorDeImpactos()
        {
            tempoDecorrido = 0;
            addView = false;
            procurouAlvo = false;
        }

        public static void AttackHelper(Transform alvo, Transform T)
        {
            Vector3 forwardInicial = T.forward;
            if (alvo != null)
            {
                if (Vector3.Dot(alvo.position - T.position, T.forward) > -0.25f)
                    forwardInicial = alvo.position - T.position;


                forwardInicial.y = 0;
                T.rotation = Quaternion.LookRotation(forwardInicial);

            }
        }

        public void ImpatoAtivo(GameObject G, PetAttackBase ativa, ImpactFeatures caracteristica,GameObject focado=null)
        {

            if (focado)
                alvoProcurado = focado.transform;
            else
                alvoProcurado = FindBestTarget.Procure(G, new string[1] { "Criature" });

            tempoDecorrido += Time.deltaTime;
            if (!procurouAlvo)
            {
                alvoProcurado = CriaturesPerto.procureUmBomAlvo(G);
                procurouAlvo = true;
                Debug.Log(alvoProcurado + "  esse é o alvo");
                AttackHelper(alvoProcurado, G.transform);
            }

            if (!addView)
            {
                tempoDecorrido += ativa.TempoDeMoveMin;
                AttackColliders.AdicionaOColisor(G, ativa, caracteristica, tempoDecorrido);

                MessageAgregator<MsgRequest3dSound>.Publish(new MsgRequest3dSound()
                {
                    sfxId = ativa.SomDoGolpe,
                    sender = G.transform
                });

                addView = true;

            }

            if (tempoDecorrido < ativa.TempoDeMoveMax)
            {
                if (((int)(tempoDecorrido * 10)) % 2 == 0 && alvoProcurado)
                    AttackHelper(alvoProcurado, G.transform);

                ativa.DirDeREpulsao = G.transform.forward;

                if (!controle)
                    controle = G.GetComponent<CharacterController>();
                controle.Move(ativa.VelocidadeDeGolpe * G.transform.forward * Time.deltaTime + Vector3.down * 9.8f);
            }
        }
    }

    [System.Serializable]
    public struct ImpactFeatures
    {
        public AttacksTrails nomeTrail;
        public ImpactParticles noImpacto;
        public bool parentearNoOsso;

        public ImpactFeatures(AttacksTrails trail, ImpactParticles noImpacto, bool parentearOsso)
        {
            nomeTrail = trail;
            this.noImpacto = noImpacto;
            this.parentearNoOsso = parentearOsso;
        }
    }

    public enum AttacksTrails
    {
        umCuboETrail,
        tresCubos,
        doisCubos,
        dentada,
        tempestadeDeFolhas,
        tempestadeEletrica,
        hidroBomba,
        tosteAtaque,
        chuvaVenenosa,
        avalanche,
        colisorParaGarra,
        colisorDentada
    }

    public enum ImpactParticles
    {
        impactoComum,
        impactoDeFolhas,
        impactoDeAgua,
        impactoDeFogo,
        impactoDeVento,
        impactoDeGosma,
        impactoDeGosmaAcida,
        impactoVenenoso,
        impactoDePedra,
        impactoEletrico,
        impactoDeTerra,
        impactoDeGas,
        defeatedParticles,
        fogoAoChao,
        poeiraAoVento
    }

    public enum GeneralParticles
    {
        rollParticles,
        subindoSobreVoo,
        particulaLuvaDeGuarde,
        raioDeLuvaDeGuarde,
        replaceParticles,
        acaoDeCura1,
        captureEscape,
        luz1captura
    }
}