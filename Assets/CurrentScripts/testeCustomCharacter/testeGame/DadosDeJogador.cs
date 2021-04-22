using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriaturesLegado;

namespace Criatures2021
{
    public class DadosDeJogador
    {
        [SerializeField] private List<CriatureBase> criaturesAtivos = new List<CriatureBase>();
        [SerializeField] private List<CriatureBase> criaturesArmagedados = new List<CriatureBase>();
        [SerializeField] private int cristais = 0;

        public int ItemSai { get; set; } = 0;
        public int MaxCarregaveis { get; set; } = 5;

        public List<MbItens> Itens { get; set; } = new List<MbItens>();
        public IndiceDeArmagedoms UltimoArmagedom { get; set; } = IndiceDeArmagedoms.cavernaIntro;
        public float TempoDoUltimoUsoDeItem { get; set; } = 0;
        public int CriatureSai { get; set; } = 0;

        public List<CriatureBase> CriaturesAtivos { get; set; }
        public List<CriatureBase> CriaturesArmagedados { get; set; }


        public void InicializadorDosDados()
        {

            CriaturesAtivos = new List<CriatureBase>() {
                new CriatureBase(nomesCriatures.Xuash,10),
                new CriatureBase(nomesCriatures.Florest,2),
                new CriatureBase(nomesCriatures.PolyCharm,3),
                new CriatureBase(nomesCriatures.Iruin,2),
                new CriatureBase(nomesCriatures.Cabecu,10)
            };

            CriaturesArmagedados = new List<CriatureBase>() {
                new CriatureBase(nomesCriatures.Onarac,1),
                new CriatureBase(nomesCriatures.Babaucu,3),
                new CriatureBase(nomesCriatures.Wisks,2),
                new CriatureBase(nomesCriatures.Serpente,3)
            };
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