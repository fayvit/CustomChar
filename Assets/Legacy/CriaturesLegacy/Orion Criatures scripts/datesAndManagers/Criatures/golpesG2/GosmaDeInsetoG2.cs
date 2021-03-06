using UnityEngine;
using System.Collections;
using CriaturesLegado;

[System.Serializable]
public class GosmaDeInsetoG2 : ProjetilBase
{
    public GosmaDeInsetoG2() : base(new ContainerDeCaracteristicasDeGolpe()
    {
        nome = nomesGolpes.gosmaDeInseto,
        tipo = NomeTipos.Inseto,
        carac = caracGolpe.projetil,
        custoPE = 1,
        potenciaCorrente = 3,
        potenciaMaxima = 7,
        potenciaMinima = 1,
        tempoDeReuso = 5,
        tempoDeMoveMax = 1,
        tempoDeMoveMin = 0.15f,
        tempoDeDestroy = 2,
        TempoNoDano = 0.75f,
        velocidadeDeGolpe = 10
    }
        )
    {
        carac = new CaracteristicasDeProjetil()
        {
            noImpacto = NoImpacto.impactoDeGosma,
            tipo = TipoDoProjetil.basico
        };
    }

}
