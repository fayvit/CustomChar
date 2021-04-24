namespace CriaturesLegado
{
    [System.Serializable]
    /// <summary>
    /// Classe responsavel pelo uso da Gasolina
    /// </summary>
    public class MbGasolina : ItemDeEnergiaBase
    {
        public MbGasolina(int estoque = 1) : base(new ContainerDeCaracteristicasDeItem(nomeIDitem.gasolina)
        {
            valor = 40
        }
            )
        {
            Estoque = estoque;
            recuperaDoTipo = NomeTipos.Fogo;
            valorDeRecuperacao = 40;
        }


    }
}