using UnityEngine;
using System.Collections;
using CriaturesLegado;

[System.Serializable]
public class VentaniaG2 : ProjetilBase
{

    public VentaniaG2() : base(new ContainerDeCaracteristicasDeGolpe()
    {
        nome = nomesGolpes.ventania,
        tipo = NomeTipos.Voador,
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
        velocidadeDeGolpe = 18,
        podeNoAr = true
    }
        )
    {
        carac = new CaracteristicasDeProjetil()
        {
            noImpacto = NoImpacto.impactoDeVento,
            tipo = TipoDoProjetil.basico
        };
    }
    

}
