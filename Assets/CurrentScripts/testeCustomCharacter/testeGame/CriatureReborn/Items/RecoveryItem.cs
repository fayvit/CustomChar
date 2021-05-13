using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Criatures2021
{
    [System.Serializable]
    public class RecoveryItem : ConsumableItemBase
    {
        protected int valorDeRecuperacao = 40;
        public RecoveryItem(ItemFeatures C) : base(C) { }

        public override void IniciaUsoComCriature(GameObject dono, List<ItemBase> lista)
        {
            Dono = dono;
            Lista = lista;
            PetAtributes P = dono.GetComponent<CharacterManager>().ActivePet.MeuCriatureBase.PetFeat.meusAtributos;
            IniciaUsoDesseItem(dono, QuantitativeItem.CanUseRecoveryItem(P));
        }

        public override bool AtualizaUsoComCriature()
        {
            return AtualizaUsoDesseItem(GeneralParticles.acaoDeCura1);
        }

        public override void IniciaUsoDeHeroi(GameObject dono, List<ItemBase> lista)
        {
            Dono = dono;
            Lista = lista;
            PetAtributes P = dono.GetComponent<CharacterManager>().ActivePet.MeuCriatureBase.PetFeat.meusAtributos;
            IniciaUsoDesseItem(dono, QuantitativeItem.CanUseRecoveryItem(P));
        }

        public override bool AtualizaUsoDeHeroi()
        {
            return AtualizaUsoDesseItem(GeneralParticles.acaoDeCura1);
        }

        protected override void EscolhiEmQuemUsar(int indice)
        {
            PetAtributes A = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos[indice].PetFeat.meusAtributos;            

            EscolhiEmQuemUsar(indice,
                QuantitativeItem.CanUseRecoveryItem(A), true,
                valorDeRecuperacao, A.PV.Corrente,
                A.PV.Maximo,
                PetTypeName.nulo);
        }

        public override void AcaoDoItemConsumivel(int indice)
        {
            
            PetAtributes P = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos[indice].PetFeat.meusAtributos;
            QuantitativeItem.RecuperaPV(P, valorDeRecuperacao);
            //ItemQuantitativo.RecuperaPV(C.CaracCriature.meusAtributos, valorDeRecuperacao);

            //if (!GameController.g.estaEmLuta)
            //    GameController.g.Salvador.SalvarAgora();
        }
    }

}