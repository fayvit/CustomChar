using UnityEngine;
using System.Collections;
using CriaturesLegado;

public class MbMudeCena : MonoBehaviour
{
    [SerializeField]private Vector3 posicao;
    [SerializeField]private NomesCenas[] cenasParaCarregar;
    [SerializeField]private float olhePraLa;
    [SerializeField]private Color corDoFade = Color.black;

    private bool iniciarCarregamento = false;
    private float tempoDecorrido = 0;
    private FadeView p;
    private Vector3 dirDeMove;

    // Use this for initialization
    void Start()
    {
        gameObject.tag = "cenario";
        gameObject.layer = 9;
    }

    // Update is called once per frame
    void Update()
    {
        if (iniciarCarregamento)
        {
            tempoDecorrido += Time.deltaTime;
            GameController.g.ReiniciarContadorDeEncontro();
            GameController.g.Manager.Mov.AplicadorDeMovimentos(dirDeMove);
            if (tempoDecorrido > 1.5f)
            {
                CharacterManager manager = GameController.g.Manager;
                manager.transform.position = posicao;
                manager.transform.rotation = Quaternion.Euler(0, olhePraLa, 0);
                GameController.g.Salvador.SalvarAgora(cenasParaCarregar);
                GameObject G = new GameObject();
                SceneLoader loadScene = G.AddComponent<SceneLoader>();
                loadScene.CenaDoCarregamento(GameController.g.Salvador.IndiceDoJogoAtual);
                Destroy(gameObject);
            }
        }
    }

    void IniciarCarregamentoDeCena()
    {
        iniciarCarregamento = true;
        GameController.EntrarNoFluxoDeTexto();
        p = GameController.g.gameObject.AddComponent<FadeView>();
        p.cor = corDoFade;
        p.vel = 1.5f;
        GameController.g.Manager.Estado = EstadoDePersonagem.movimentoDeFora;
    }

    void OnTriggerEnter(Collider col)
    {
       // if (!heroi.emLuta)
        {
            if (col.tag == "Player")
            {
                DontDestroyOnLoad(gameObject);
                dirDeMove = col.transform.forward;
                IniciarCarregamentoDeCena();
            }

            if (col.tag == "Criature"&&!GameController.g.estaEmLuta)
            {
                EvitaAvancarNoTrigger.Evita();
            }
        }
    }
}
