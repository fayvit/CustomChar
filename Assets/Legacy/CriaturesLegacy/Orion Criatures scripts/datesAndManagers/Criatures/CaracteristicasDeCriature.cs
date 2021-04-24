using UnityEngine;
using System.Collections;

[System.Serializable]
public class CaracteristicasDeCriature
{
    public NomeTipos[] meusTipos;
    public ContraTipos[] contraTipos;    
    public Atributos meusAtributos = new Atributos(new ContainerDeAtributos());
    public GerenciadorDeExperiencia mNivel = new GerenciadorDeExperiencia();
    public float distanciaFundamentadora = 0.2f;

    public void IncrementaNivel(int nivel)
    {
        UpDeNivel.calculaUpDeNivel(nivel, meusAtributos,true);

        mNivel.Nivel = nivel;
        mNivel.ParaProxNivel = mNivel.CalculaPassaNivelInicial(nivel);

    }

    public bool TemOTipo(NomeTipos tipo)
    {
        bool retorno = false;
        for (int i = 0; i < meusTipos.Length; i++)
        {
            if (meusTipos[i].ToString() == tipo.ToString())
                retorno = true;
        }

        return retorno;
    }
}
