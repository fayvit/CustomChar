using UnityEngine;
using System.Collections;
using CriaturesLegado;

[System.Serializable]
public class MbBombaDeGas : ProjetilBase
{
    public MbBombaDeGas() : base(new ContainerDeCaracteristicasDeGolpe()
    {
        nome = nomesGolpes.bombaDeGas,
        tipo = NomeTipos.Gas,
        carac = caracGolpe.projetil,
        custoPE = 1,
        potenciaCorrente = 3,
        potenciaMaxima = 7,
        potenciaMinima = 1,
        tempoDeReuso = 5,
        tempoDeMoveMax = 1,
        tempoDeMoveMin = 0.3f,
        tempoDeDestroy = 2,
        TempoNoDano = 0.75f,
        velocidadeDeGolpe = 10
    }
        )
    {
        carac = new CaracteristicasDeProjetil()
        {
            noImpacto = NoImpacto.impactoDeGas,
            tipo = TipoDoProjetil.basico
        };
    }

}
