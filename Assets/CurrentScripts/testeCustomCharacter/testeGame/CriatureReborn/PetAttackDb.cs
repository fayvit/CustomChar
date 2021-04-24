﻿using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class PetAttackDb
    {
        [SerializeField] private AttackNameId _nome = AttackNameId.nulo;
        [SerializeField] private colisor _colisor = new colisor();

        [SerializeField] private int nivelDoGolpe = 1;
        [SerializeField] private float modPersonagem = 0;
        [SerializeField] private float acimaDoChao = 0;
        [SerializeField] private float distanciaEmissora = 0;
        [SerializeField] private float tempoDeInstancia = 0;
        [SerializeField] private float taxaDeUso = 1;

        //public static GolpePersonagem RetornaGolpePersonagem(GameObject G, nomesGolpes nomeDoGolpe)
        //{
        //    PetBase criatureBase = G.GetComponent<PetManager>().MeuCriatureBase;
        //    GerenciadorDeGolpes gg = criatureBase.GerenteDeGolpes;
        //    GolpePersonagem gP = gg.ProcuraGolpeNaLista(criatureBase.NomeID, nomeDoGolpe);
        //    return gP;
        //}

        public float AcimaDoChao
        {
            get
            {
                return acimaDoChao;
            }

            set
            {
                acimaDoChao = value;
            }
        }

        public colisor Colisor
        {
            get
            {
                return _colisor;
            }

            set
            {
                _colisor = value;
            }
        }

        public float DistanciaEmissora
        {
            get
            {
                return distanciaEmissora;
            }

            set
            {
                distanciaEmissora = value;
            }
        }

        public AttackNameId Nome
        {
            get
            {
                return _nome;
            }

            set
            {
                _nome = value;
            }
        }

        public float TempoDeInstancia
        {
            get
            {
                return tempoDeInstancia;
            }

            set
            {
                tempoDeInstancia = value;
            }
        }

        public float TaxaDeUso
        {
            get
            {
                return taxaDeUso;
            }

            set
            {
                taxaDeUso = value;
            }
        }

        public int NivelDoGolpe
        {
            get
            {
                return nivelDoGolpe;
            }

            set
            {
                nivelDoGolpe = value;
            }
        }

        public float ModPersonagem
        {
            get
            {
                return modPersonagem;
            }

            set
            {
                modPersonagem = value;
            }
        }
    }

}