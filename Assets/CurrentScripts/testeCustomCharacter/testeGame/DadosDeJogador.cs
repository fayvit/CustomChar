using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriaturesLegado;

namespace Criatures2021
{
    [System.Serializable]
    public class DadosDeJogador
    {
        [SerializeField] private List<PetBase> criaturesAtivos = new List<PetBase>();
        [SerializeField] private List<PetBase> criaturesArmagedados = new List<PetBase>();
        [SerializeField] private int cristais = 0;

        public int ItemSai { get; set; } = 0;
        public int MaxCarregaveis { get; set; } = 5;

        public List<MbItens> Itens { get; set; } = new List<MbItens>();
        public IndiceDeArmagedoms UltimoArmagedom { get; set; } = IndiceDeArmagedoms.cavernaIntro;
        public float TempoDoUltimoUsoDeItem { get; set; } = 0;
        public int CriatureSai { get; set; } = 0;

        public List<PetBase> CriaturesAtivos { get; set; }
        public List<PetBase> CriaturesArmagedados { get; set; }


        public void InicializadorDosDados()
        {

            CriaturesAtivos = new List<PetBase>() {
                new PetBase(PetName.Xuash,10),
                //new PetBase(PetName.Florest,2),
                //new PetBase(PetName.PolyCharm,3),
                //new PetBase(PetName.Iruin,2),
                //new PetBase(PetName.Cabecu,10)
            };

            //CriaturesArmagedados = new List<PetBase>() {
            //    new PetBase(PetName.Onarac,1),
            //    new PetBase(PetName.Babaucu,3),
            //    new PetBase(PetName.Wisks,2),
            //    new PetBase(PetName.Serpente,3)
            //};
            Itens = new List<MbItens>()
        {
            PegaUmItem.Retorna(nomeIDitem.pergaminhoDePerfeicao,14),
            PegaUmItem.Retorna(nomeIDitem.maca,16),
            PegaUmItem.Retorna(nomeIDitem.pergVentosCortantes,2),
            PegaUmItem.Retorna(nomeIDitem.pergFuracaoDeFolhas,5),
            PegaUmItem.Retorna(nomeIDitem.pergaminhoDeFuga,10),
            PegaUmItem.Retorna(nomeIDitem.regador,10),
            PegaUmItem.Retorna(nomeIDitem.inseticida,2),
            PegaUmItem.Retorna(nomeIDitem.ventilador,2),
            PegaUmItem.Retorna(nomeIDitem.pergSinara,2),
            PegaUmItem.Retorna(nomeIDitem.pergAlana,1)
        };

        }
    }
}