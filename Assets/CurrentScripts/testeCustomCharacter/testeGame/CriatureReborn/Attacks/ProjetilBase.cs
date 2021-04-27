using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;

namespace Criatures2021
{

    [System.Serializable]
    public class ProjetilBase : PetAttackBase
    {
        private bool addView = false;
        private bool animaEmissor = true;
        private float tempoDecorrido = 0;

        protected ProjetilFeatures carac = new ProjetilFeatures()
        {
            noImpacto = ImpactParticles.impactoComum,
            tipo = ProjetilType.basico
        };

        protected bool AnimaEmissor
        {
            get { return animaEmissor; }
            set { animaEmissor = value; }
        }

        public ProjetilBase(PetAttackFeatures C) : base(C) { }

        public override void IniciaGolpe(GameObject G)
        {
            addView = false;
            tempoDecorrido = 0;
            

            carac.posInicial = EmissionPosition.Get(G,Nome);

            DirDeREpulsao = G.transform.forward;

            string animacao = "emissor";
            if (!AnimaEmissor)
                animacao = Nome.ToString();

            MessageAgregator<MsgRequestAtkAnimation>.Publish(new MsgRequestAtkAnimation()
            {
                gameObject = G,
                nomeAnima = animacao
            });

            //    AnimadorCriature.AnimaAtaque(G, "emissor");
            //else
            //    AnimadorCriature.AnimaAtaque(G, this.Nome.ToString());
        }

        public override void UpdateGolpe(GameObject G)
        {

            tempoDecorrido += Time.deltaTime;
            if (!addView)
            {
                addView = true;
                AplicadorDeProjeteis.AplicaProjetil(G, this, carac);
            }
        }
    }

}