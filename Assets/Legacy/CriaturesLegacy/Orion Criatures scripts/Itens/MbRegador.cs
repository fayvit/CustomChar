namespace CriaturesLegado
{
    [System.Serializable]
    /// <summary>
    /// Classe responsavel pelo uso do Regador
    /// </summary>
    public class MbRegador : ItemDeEnergiaBase
    {
        public MbRegador(int estoque = 1) : base(new ContainerDeCaracteristicasDeItem(nomeIDitem.regador)
        {
            valor = 40
        }
            )
        {
            Estoque = estoque;
            recuperaDoTipo = NomeTipos.Planta;
            valorDeRecuperacao = 40;
        }
    }
}