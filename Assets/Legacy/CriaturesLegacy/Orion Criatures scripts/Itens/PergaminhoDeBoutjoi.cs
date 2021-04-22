using UnityEngine;
using System.Collections;
using CriaturesLegado;

[System.Serializable]
public class PergaminhoDeBoutjoi : ItemModificadorDeAtributoIntrinsico
{

    public PergaminhoDeBoutjoi(int estoque = 1) : base(new ContainerDeCaracteristicasDeItem(nomeIDitem.pergaminhoDeBoutjoi)
    {
        valor = 1500
    }
        )
    {
        Estoque = estoque;
        TextoDaMensagemInicial = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.usarPergaminhoDeBoutjoi).ToArray();
        Particula = DoJogo.particulaDaDefesaPergaminhoFora;
    }

    protected override void AplicaEfeito(CriatureBase C)
    {
        AtributoInstrinsico A = C.CaracCriature.meusAtributos.Defesa;

        A = ContaDeSubida(A);

        C.CaracCriature.meusAtributos.Defesa = A;

        EntraNoModoFinalizacao(C);
    }

}
