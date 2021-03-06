using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CriaturesLegado;

public class PainelUmaMensagem : MonoBehaviour
{
    public delegate void RetornarParaAntecessor();
    public event RetornarParaAntecessor retornar;

    [SerializeField] private Text textoDaMensagem;
    [SerializeField] private Text textoDoBotao;
    // Use this for initialization
    public void ConstroiPainelUmaMensagem(RetornarParaAntecessor r,string textoDaMensagem)
    {
        ActionManager.ModificarAcao(transform, BotaoEntendi);
        gameObject.SetActive(true);
        this.textoDaMensagem.text = textoDaMensagem;
        retornar = r;
    }

    public void ConstroiPainelUmaMensagem(RetornarParaAntecessor r)
    {
        gameObject.SetActive(true);
        retornar = r;
    }

    public void AtualizarTextoDaMensagem(string s)
    {
        textoDaMensagem.text = s;
    }

    public void AtualizaTextoDoBotao(string s)
    {
        textoDoBotao.text = s;
    }

    public void AtualizaTextoDaMensagemEBotao(string textoDoBotao, string textoDaMensagem)
    {
        AtualizarTextoDaMensagem(textoDaMensagem);
        AtualizaTextoDoBotao(textoDoBotao);
    }

    public void BotaoEntendi()
    {
        ActionManager.ModificarAcao(null, null);
        gameObject.SetActive(false);
        if (retornar != null)
        {
            retornar();
            retornar = null;
        }
        
    }

    void Update()
    {
        if (GameController.g.CommandR.DisparaAcao() || Input.GetButtonDown("Cancel"))
        {
            BotaoEntendi();
        }
    }
}
