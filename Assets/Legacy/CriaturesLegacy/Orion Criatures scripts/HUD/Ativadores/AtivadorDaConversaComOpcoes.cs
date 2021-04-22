using UnityEngine;
using System.Collections;
using CriaturesLegado;

public class AtivadorDaConversaComOpcoes : AtivadorDoBotaoConversa
{
    [SerializeField] private NPCcomOpcoesDeConversa npcOpcoes;

    new void Start()
    {
        npc = npcOpcoes;
        base.Start();
    }

    public override void FuncaoDoBotao()
    {
        npcOpcoes.IniciarOpcoes();
        base.FuncaoDoBotao();
    }
}
