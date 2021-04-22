using UnityEngine;
using System.Collections;
using CriaturesLegado;

[System.Serializable]
public class TinteiroSagradaDeLog : ItemNaoUsavel
{
    public TinteiroSagradaDeLog(int estoque) : base(new ContainerDeCaracteristicasDeItem(nomeIDitem.tinteiroSagradoDeLog)
    {
        valor = 0
    }
        )
    {
        Estoque = estoque;
    }

   
}
