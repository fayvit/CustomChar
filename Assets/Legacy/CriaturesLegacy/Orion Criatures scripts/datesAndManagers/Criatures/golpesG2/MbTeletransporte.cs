﻿using CriaturesLegado;
using UnityEngine;

[System.Serializable]
public class MbTeletransporte : HitNoChaoBase
{

    public MbTeletransporte() : base(new ContainerDeCaracteristicasDeGolpe()
    {
        nome = nomesGolpes.teletransporte,
        tipo = NomeTipos.Psiquico,
        carac = caracGolpe.hitNoChao,
        custoPE = 3,
        potenciaCorrente = 7,
        potenciaMaxima = 14,
        potenciaMinima = 3,
        tempoDeReuso = 10.5f,
        TempoNoDano = 0.5f,
        velocidadeDeGolpe = 25f,
        distanciaDeRepulsao = 65f,
        velocidadeDeRepulsao = 33,
        tempoDeMoveMin = 2,//74
        tempoDeMoveMax = 0.8f,
        tempoDeDestroy = 2.5f,
    }
        )
    {

        noImpacto = NoImpacto.impactoComum;

    }


}
