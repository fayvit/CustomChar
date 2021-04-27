﻿using UnityEngine;
using System.Collections;
using CriaturesLegado;
using Criatures2021;

[System.Serializable]
public class OlharMal : ProjetilBase
{
    public OlharMal() : base(
        new ContainerDeCaracteristicasDeGolpe()
        {
            nome = nomesGolpes.olharMal,
            tipo = NomeTipos.Normal,
            carac = caracGolpe.projetil,
            custoPE = 4,
            potenciaCorrente = 1,
            potenciaMaxima = 8,
            potenciaMinima = 1,
            tempoDeReuso = 30f,
            tempoDeMoveMax = 1,
            tempoDeMoveMin = 0.3f,
            tempoDeDestroy = 4,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 10,//18
        }
        )
    {
        carac = new CaracteristicasDeProjetil()
        {
            noImpacto = NoImpacto.impactoComum,
            tipo = TipoDoProjetil.statusExpansivel
        };
    }

    public override void VerificaAplicaStatus(CriatureBase atacante, CreatureManager cDoAtacado)
    {
        StatusTemporarioBase S = new Amedrontado()
        {
            Dados = new DatesForTemporaryStatus()
            {
                Quantificador = 2,
                TempoSignificativo = 240,
                Tipo = TipoStatus.amedrontado
            },
            CDoAfetado = cDoAtacado,
            OAfetado = cDoAtacado.MeuCriatureBase
        };

        int num = StatusTemporarioBase.ContemStatus(TipoStatus.amedrontado, cDoAtacado.MeuCriatureBase);

        AdicionaStatusSimples.InsereStatusSimples(cDoAtacado, S,num);

        Debug.Log("amedrontou");
    }


}
