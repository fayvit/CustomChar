using UnityEngine;
using System.Collections;
using CriaturesLegado;

[System.Serializable]
public class BolaPsiquicaG2 : ProjetilBase
{
    public BolaPsiquicaG2() : base(new ContainerDeCaracteristicasDeGolpe()
    {
        nome = nomesGolpes.bolaPsiquica,
        tipo = NomeTipos.Psiquico,
        carac = caracGolpe.projetil,
        custoPE = 2,
        potenciaCorrente = 4,
        potenciaMaxima = 10,
        potenciaMinima = 2,
        tempoDeReuso = 7.5f,
        tempoDeMoveMax = 1,
        tempoDeMoveMin = 0.3f,
        tempoDeDestroy = 2,
        TempoNoDano = 0.75f,
        velocidadeDeGolpe = 18
    }
        )
    {
        carac = new CaracteristicasDeProjetil()
        {
            noImpacto = NoImpacto.impactoComum,
            tipo = TipoDoProjetil.rigido
        };
    }

}
