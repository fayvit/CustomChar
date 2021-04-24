[System.Serializable]
public class ContraTipos  {

	[UnityEngine.SerializeField]private float _mod;
	[UnityEngine.SerializeField]private string _nome;

    public static ContraTipos[] AplicaContraTipos(NomeTipos nomeDoTipo)
    {
        ContraTipos[] retorno = new ContraTipos[System.Enum.GetValues(typeof(NomeTipos)).Length];

        switch (nomeDoTipo)
        {
            case NomeTipos.Agua:
                retorno = new ContraTipos[]
                    {
                        new ContraTipos (){ Nome = NomeTipos.Agua.ToString(),    Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Fogo.ToString(),    Mod = 0.25f},
                        new ContraTipos (){ Nome = NomeTipos.Planta.ToString(),  Mod = 1.75f},
                        new ContraTipos (){ Nome = NomeTipos.Gelo.ToString(),    Mod = 2},
                        new ContraTipos (){ Nome = NomeTipos.Terra.ToString(),   Mod = 0.5f},
                        new ContraTipos (){ Nome = NomeTipos.Pedra.ToString(),   Mod = 0.5f},
                        new ContraTipos (){ Nome = NomeTipos.Psiquico.ToString(),Mod = 1.75f},
                        new ContraTipos (){ Nome = NomeTipos.Eletrico.ToString(),Mod = 2f},
                        new ContraTipos (){ Nome = NomeTipos.Normal.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Veneno.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Inseto.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Voador.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Gas.ToString(),     Mod = 1}
                    };

            break;
            case NomeTipos.Planta:
                retorno = new ContraTipos[]
                    {
                        new ContraTipos (){ Nome = NomeTipos.Agua.ToString(),    Mod = 0.25f},
                        new ContraTipos (){ Nome = NomeTipos.Fogo.ToString(),    Mod = 2f},
                        new ContraTipos (){ Nome = NomeTipos.Planta.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Gelo.ToString(),    Mod = 1.25f},
                        new ContraTipos (){ Nome = NomeTipos.Terra.ToString(),   Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Pedra.ToString(),   Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Psiquico.ToString(),Mod = 1.25f},
                        new ContraTipos (){ Nome = NomeTipos.Eletrico.ToString(),Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Normal.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Veneno.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Inseto.ToString(),  Mod = 1.25f},
                        new ContraTipos (){ Nome = NomeTipos.Voador.ToString(),  Mod = 1.25f},
                        new ContraTipos (){ Nome = NomeTipos.Gas.ToString(),     Mod = 1}
                    };

            break;
            case NomeTipos.Fogo:
                retorno = new ContraTipos[]
                    {
                        new ContraTipos (){ Nome = NomeTipos.Agua.ToString(),    Mod = 1.5f},
                        new ContraTipos (){ Nome = NomeTipos.Fogo.ToString(),    Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Planta.ToString(),  Mod = 0.5f},
                        new ContraTipos (){ Nome = NomeTipos.Gelo.ToString(),    Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Terra.ToString(),   Mod = 1.5f},
                        new ContraTipos (){ Nome = NomeTipos.Pedra.ToString(),   Mod = 1.25f},
                        new ContraTipos (){ Nome = NomeTipos.Psiquico.ToString(),Mod = 1.75f},
                        new ContraTipos (){ Nome = NomeTipos.Eletrico.ToString(),Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Normal.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Veneno.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Inseto.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Voador.ToString(),  Mod = 1.25f},
                        new ContraTipos (){ Nome = NomeTipos.Gas.ToString(),     Mod = 2}
                    };

            break;
            case NomeTipos.Voador:
                retorno = new ContraTipos[]
                    {
                        new ContraTipos (){ Nome = NomeTipos.Agua.ToString(),    Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Fogo.ToString(),    Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Planta.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Gelo.ToString(),    Mod = 2f},
                        new ContraTipos (){ Nome = NomeTipos.Terra.ToString(),   Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Pedra.ToString(),   Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Psiquico.ToString(),Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Eletrico.ToString(),Mod = 1.5f},
                        new ContraTipos (){ Nome = NomeTipos.Normal.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Veneno.ToString(),  Mod = 1.5f},
                        new ContraTipos (){ Nome = NomeTipos.Inseto.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Voador.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Gas.ToString(),     Mod = 0.25f}
                    };
            break;
            case NomeTipos.Inseto:
                retorno = new ContraTipos[]
                    {
                        new ContraTipos (){ Nome = NomeTipos.Agua.ToString(),    Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Fogo.ToString(),    Mod = 1.75f},
                        new ContraTipos (){ Nome = NomeTipos.Planta.ToString(),  Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Gelo.ToString(),    Mod = 1.25f},
                        new ContraTipos (){ Nome = NomeTipos.Terra.ToString(),   Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Pedra.ToString(),   Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Psiquico.ToString(),Mod = 0.5f},
                        new ContraTipos (){ Nome = NomeTipos.Eletrico.ToString(),Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Normal.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Veneno.ToString(),  Mod = 2f},
                        new ContraTipos (){ Nome = NomeTipos.Inseto.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Voador.ToString(),  Mod = 1.75f},
                        new ContraTipos (){ Nome = NomeTipos.Gas.ToString(),     Mod = 1.5f}
                    };
            break;
            case NomeTipos.Psiquico:
                retorno = new ContraTipos[]
                    {
                        new ContraTipos (){ Nome = NomeTipos.Agua.ToString(),    Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Fogo.ToString(),    Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Planta.ToString(),  Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Gelo.ToString(),    Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Terra.ToString(),   Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Pedra.ToString(),   Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Psiquico.ToString(),Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Eletrico.ToString(),Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Normal.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Veneno.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Inseto.ToString(),  Mod = 1.75f},
                        new ContraTipos (){ Nome = NomeTipos.Voador.ToString(),  Mod = 1.75f},
                        new ContraTipos (){ Nome = NomeTipos.Gas.ToString(),     Mod = 1.5f}
                    };
            break;
            case NomeTipos.Normal:
                retorno = new ContraTipos[]
                    {
                        new ContraTipos (){ Nome = NomeTipos.Agua.ToString(),    Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Fogo.ToString(),    Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Planta.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Gelo.ToString(),    Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Terra.ToString(),   Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Pedra.ToString(),   Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Psiquico.ToString(),Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Eletrico.ToString(),Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Normal.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Veneno.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Inseto.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Voador.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Gas.ToString(),     Mod = 1},
                    };
            break;
            case NomeTipos.Veneno:
                retorno = new ContraTipos[]
                    {
                        new ContraTipos (){ Nome = NomeTipos.Agua.ToString(),    Mod = 1.25f},
                        new ContraTipos (){ Nome = NomeTipos.Fogo.ToString(),    Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Planta.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Gelo.ToString(),    Mod = 1.25f},
                        new ContraTipos (){ Nome = NomeTipos.Terra.ToString(),   Mod = 1.25f},
                        new ContraTipos (){ Nome = NomeTipos.Pedra.ToString(),   Mod = 1.25f},
                        new ContraTipos (){ Nome = NomeTipos.Psiquico.ToString(),Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Eletrico.ToString(),Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Normal.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Veneno.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Inseto.ToString(),  Mod = 0.5f},
                        new ContraTipos (){ Nome = NomeTipos.Voador.ToString(),  Mod = 1},
                        new ContraTipos (){ Nome = NomeTipos.Gas.ToString(),     Mod = 0.5f},
                    };
            break;
            case NomeTipos.Pedra:
                retorno = new ContraTipos[]
                    {
                        new ContraTipos (){ Nome = NomeTipos.Agua.ToString(),    Mod = 2f},
                        new ContraTipos (){ Nome = NomeTipos.Fogo.ToString(),    Mod = 0.25f},
                        new ContraTipos (){ Nome = NomeTipos.Planta.ToString(),  Mod = 1.75f},
                        new ContraTipos (){ Nome = NomeTipos.Gelo.ToString(),    Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Terra.ToString(),   Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Pedra.ToString(),   Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Psiquico.ToString(),Mod = 1.5f},
                        new ContraTipos (){ Nome = NomeTipos.Eletrico.ToString(),Mod = 0.1f},
                        new ContraTipos (){ Nome = NomeTipos.Normal.ToString(),  Mod = 0.5f},
                        new ContraTipos (){ Nome = NomeTipos.Veneno.ToString(),  Mod = 0.5f},
                        new ContraTipos (){ Nome = NomeTipos.Inseto.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Voador.ToString(),  Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Gas.ToString(),     Mod = 0.5f},
                    };
            break;
            case NomeTipos.Eletrico:
                retorno = new ContraTipos[]
                    {
                        new ContraTipos (){ Nome = NomeTipos.Agua.ToString(),    Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Fogo.ToString(),    Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Planta.ToString(),  Mod = 1.25f},
                        new ContraTipos (){ Nome = NomeTipos.Gelo.ToString(),    Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Terra.ToString(),   Mod = 1.5f},
                        new ContraTipos (){ Nome = NomeTipos.Pedra.ToString(),   Mod = 1.75f},
                        new ContraTipos (){ Nome = NomeTipos.Psiquico.ToString(),Mod = 1.5f},
                        new ContraTipos (){ Nome = NomeTipos.Eletrico.ToString(),Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Normal.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Veneno.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Inseto.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Voador.ToString(),  Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Gas.ToString(),     Mod = 1f},
                    };
            break;
            case NomeTipos.Terra:
                retorno = new ContraTipos[]
                    {
                        new ContraTipos (){ Nome = NomeTipos.Agua.ToString(),    Mod = 2f},
                        new ContraTipos (){ Nome = NomeTipos.Fogo.ToString(),    Mod = 0.1f},
                        new ContraTipos (){ Nome = NomeTipos.Planta.ToString(),  Mod = 1.75f},
                        new ContraTipos (){ Nome = NomeTipos.Gelo.ToString(),    Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Terra.ToString(),   Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Pedra.ToString(),   Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Psiquico.ToString(),Mod = 1.5f},
                        new ContraTipos (){ Nome = NomeTipos.Eletrico.ToString(),Mod = 0.15f},
                        new ContraTipos (){ Nome = NomeTipos.Normal.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Veneno.ToString(),  Mod = 0.95f},
                        new ContraTipos (){ Nome = NomeTipos.Inseto.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Voador.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Gas.ToString(),     Mod = 0.75f},
                    };
            break;
            case NomeTipos.Gas:
                retorno = new ContraTipos[]
                    {
                        new ContraTipos (){ Nome = NomeTipos.Agua.ToString(),    Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Fogo.ToString(),    Mod = 2f},
                        new ContraTipos (){ Nome = NomeTipos.Planta.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Gelo.ToString(),    Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Terra.ToString(),   Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Pedra.ToString(),   Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Psiquico.ToString(),Mod = 0.5f},
                        new ContraTipos (){ Nome = NomeTipos.Eletrico.ToString(),Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Normal.ToString(),  Mod = 1f},
                        new ContraTipos (){ Nome = NomeTipos.Veneno.ToString(),  Mod = 0.75f},
                        new ContraTipos (){ Nome = NomeTipos.Inseto.ToString(),  Mod = 0.5f},
                        new ContraTipos (){ Nome = NomeTipos.Voador.ToString(),  Mod = 2f},
                        new ContraTipos (){ Nome = NomeTipos.Gas.ToString(),     Mod = 1f},
                    };
            break;
        }
        return retorno;
    }

    public ContraTipos()
	{
		_mod = 1.0f;
		_nome = "";
	}

	public float Mod
	{
		get{return _mod;}
		set{_mod = value;}
	}

	public string Nome
	{
		get{return _nome;}
		set{_nome = value;}
	}

    public static string NomeEmLinguas(NomeTipos nome)
    {
        return nome.ToString();
    }
}

public enum NomeTipos
{
    nulo=-1,
	Agua,
	Fogo,
	Planta,
	Terra,
	Pedra,
	Psiquico,
	Eletrico,
	Normal,
	Veneno,
	Inseto,
	Voador,
	Gas,
    Gelo
}
