using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InputTextDoCriandoNovoJogo : MonoBehaviour
{
    [SerializeField]private InputField input;
    
    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Iniciar()
    {
        gameObject.SetActive(true);
    }

    public void CriandoJogo()
    {
        PropriedadesDeSave prop = new PropriedadesDeSave() { nome = "Jogo Criado: "+System.DateTime.Now, ultimaJogada = System.DateTime.Now };
        LoadAndSaveGame salvador = new LoadAndSaveGame();
        List<PropriedadesDeSave> lista = SaveDatesForJolt.s.SaveProps;
        //List<PropriedadesDeSave> lista = (List<PropriedadesDeSave>)(salvador.CarregarArquivo("criaturesGames.ori"));

        if (lista != null)
        {
            int maior = 0;

            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].indiceDoSave > maior)
                    maior = lista[i].indiceDoSave;
            }

            prop.indiceDoSave = maior+1;
            lista.Add(prop);
        }
        else
            lista = new List<PropriedadesDeSave>() { prop };

        SaveDatesForJolt.s.SaveProps = lista;
        SaveAndLoadInJolt.Save();
        //salvador.SalvarArquivo("criaturesGames.ori", lista);

        // Voltar();//Deve ser retirado
        IniciarCarregarCena(prop.indiceDoSave);
    }

    void IniciarCarregarCena(int indice)
    {
        gameObject.SetActive(false);
        GameObject G = new GameObject();
        SceneLoader loadScene = G.AddComponent<SceneLoader>();
        loadScene.CenaDoCarregamento(indice);
    }

    public void Voltar()
    {
        FindObjectOfType<InitialSceneManager>().FecharInputText();
    }
}
