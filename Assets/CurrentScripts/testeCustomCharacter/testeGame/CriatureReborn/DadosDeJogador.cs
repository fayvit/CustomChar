using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriaturesLegado;

namespace Criatures2021
{
    [System.Serializable]
    public class DadosDeJogador
    {
        [SerializeField,ArrayElementTitle("petId")] private List<PetBase> criaturesAtivos = new List<PetBase>();
        [SerializeField, ArrayElementTitle("petId")] private List<PetBase> criaturesArmagedados = new List<PetBase>();
        [SerializeField, ArrayElementTitle("nomeID")] private List<ItemBase> itens = new List<ItemBase>();
        [SerializeField] private int cristais = 0;

        public int ItemSai { get; set; } = 0;
        public int MaxCarregaveis { get; set; } = 5;

        public List<ItemBase> Itens { get => itens; set => itens = value; }
        public IndiceDeArmagedoms UltimoArmagedom { get; set; } = IndiceDeArmagedoms.cavernaIntro;
        public float TempoDoUltimoUsoDeItem { get; set; } = 0;
        public int CriatureSai { get; set; } = 0;

        public List<PetBase> CriaturesAtivos { get=>criaturesAtivos; set=>criaturesAtivos=value; }
        public List<PetBase> CriaturesArmagedados { get=>criaturesArmagedados; set=>criaturesArmagedados=value; }


        public void InicializadorDosDados()
        {

            CriaturesAtivos = new List<PetBase>() {
                new PetBase(PetName.Batler,10),
                new PetBase(PetName.Florest,9),
                new PetBase(PetName.Xuash,8),
                new PetBase(PetName.Florest,9),
                new PetBase(PetName.Xuash,8),
                //new PetBase(PetName.Iruin,2),
                //new PetBase(PetName.Cabecu,10)
            };

            criaturesAtivos[0].PetFeat.meusAtributos.PV.Corrente = 1;

            //CriaturesArmagedados = new List<PetBase>() {
            //    new PetBase(PetName.Onarac,1),
            //    new PetBase(PetName.Babaucu,3),
            //    new PetBase(PetName.Wisks,2),
            //    new PetBase(PetName.Serpente,3)
            //};
            Itens = new List<ItemBase>()
        {
            //ItemFactory.Get(NameIdItem.pergaminhoDePerfeicao,14),
            ItemFactory.Get(NameIdItem.maca,1),
            ItemFactory.Get(NameIdItem.cartaLuva,1),
            //ItemFactory.Get(NameIdItem.pergVentosCortantes,2),
            //ItemFactory.Get(NameIdItem.pergFuracaoDeFolhas,5),
            //ItemFactory.Get(NameIdItem.pergaminhoDeFuga,10),
            //ItemFactory.Get(NameIdItem.regador,10),
            //ItemFactory.Get(NameIdItem.inseticida,2),
            //ItemFactory.Get(NameIdItem.ventilador,2),
            //ItemFactory.Get(NameIdItem.pergSinara,2),
            //ItemFactory.Get(NameIdItem.pergAlana,1)
        };

        }
    }
}