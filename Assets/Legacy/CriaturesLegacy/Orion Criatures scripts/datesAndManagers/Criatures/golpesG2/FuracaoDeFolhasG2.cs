using UnityEngine;
using System.Collections;
using CriaturesLegado;

[System.Serializable]
public class FuracaoDeFolhasG2 : ProjetilBase
{
    public FuracaoDeFolhasG2() : base(new ContainerDeCaracteristicasDeGolpe()
    {
        nome = nomesGolpes.furacaoDeFolhas,
        tipo = NomeTipos.Planta,
        carac = caracGolpe.projetil,
        custoPE = 2,
        potenciaCorrente = 4,
        potenciaMaxima = 10,
        potenciaMinima = 2,
        tempoDeReuso = 7.5f,
        tempoDeMoveMax = 1 ,
        tempoDeMoveMin = 0.3f,
        tempoDeDestroy = 2,
        TempoNoDano = 0.75f,
        velocidadeDeGolpe = 18 ,
        podeNoAr = true
    }
        )
    {
        carac = new CaracteristicasDeProjetil()
        {
            noImpacto = NoImpacto.impactoDeFolhas,
            tipo = TipoDoProjetil.rigido
        };
    }

}
