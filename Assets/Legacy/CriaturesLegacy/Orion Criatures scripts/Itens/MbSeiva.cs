namespace CriaturesLegado
{
    [System.Serializable]
    /// <summary>
    /// Classe responsavel pelo uso da seiva d insetos
    /// </summary>
    public class MbSeiva : ItemDeEnergiaBase
    {
        public MbSeiva(int estoque = 1) : base(new ContainerDeCaracteristicasDeItem(nomeIDitem.seiva)
        {
            valor = 40
        }
            )
        {
            Estoque = estoque;
            recuperaDoTipo = NomeTipos.Inseto;
            valorDeRecuperacao = 40;
        }
    }
}