using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace CriaturesLegado
{
    [System.Serializable]
    public class SceneLoader : MonoBehaviour
    {

        [SerializeField] private LoadBar loadBar;

        private SaveDates S;
        private AsyncOperation[] a2;
        private FasesDoLoad fase = FasesDoLoad.emEspera;
        private bool podeIr = false;
        private int indiceDoJogo = 0;
        private float tempo = 0;

        private const float tempoMin = 0.25f;

        private enum FasesDoLoad
        {
            emEspera,
            carregando,
            escurecendo,
            clareando
        }

        public void CenaDoCarregamento(int indice)
        {
            DontDestroyOnLoad(gameObject);
            indiceDoJogo = indice;
            SceneManager.LoadScene("CenaDeCarregamento");
            SceneManager.sceneLoaded += IniciarCarregamento;
        }

        void IniciarCarregamento(Scene cena, LoadSceneMode mode)
        {

            loadBar = FindObjectOfType<LoadBar>();

            SceneManager.LoadSceneAsync("comunsDeFase", LoadSceneMode.Additive);
            SceneManager.sceneLoaded -= IniciarCarregamento;
            SceneManager.sceneLoaded += CarregouComuns;
        }

        void CarregouComuns(Scene cena, LoadSceneMode mode)
        {
            ComunsCarregado();
        }

        void ComunsCarregado()
        {
            if (ExistenciaDoController.AgendaExiste(ComunsCarregado, this))
            {
                fase = FasesDoLoad.carregando;

                //S = new LoadAndSaveGame().Load(indiceDoJogo);

                if (SaveDatesForJolt.s.SavedGames.Count > indiceDoJogo)
                    S = SaveDatesForJolt.s.SavedGames[indiceDoJogo];
                else
                    S = null;

                if (S == null)
                {
                    a2 = new AsyncOperation[2];
                    a2[0] = SceneManager.LoadSceneAsync("cavernaIntro", LoadSceneMode.Additive);
                    a2[1] = SceneManager.LoadSceneAsync(NomesCenas.katidsVsTempleZone.ToString(), LoadSceneMode.Additive);
                }
                else
                {

                    a2 = new AsyncOperation[S.VariaveisChave.CenasAtivas.Count];
                    for (int i = 0; i < a2.Length; i++)
                    {
                        a2[i] = SceneManager.LoadSceneAsync(S.VariaveisChave.CenasAtivas[i].ToString(), LoadSceneMode.Additive);
                    }
                }
                Time.timeScale = 0;

                SceneManager.sceneLoaded -= CarregouComuns;
                SceneManager.sceneLoaded += SetarCenaPrincipal;
            }
        }

        void ComoPode()
        {

            if (ExistenciaDoController.AgendaExiste(ComoPode, this))
            {
                //Debug.Log(GameController.g+"  segunda vez");
                CharacterManager manager = GameController.g.Manager;
                manager.eLoad = true;
                AplicadorDeCamera.cam.transform.position = S.Posicao + new Vector3(0, 12, -10);//new Vector3(483, 12f, 745);
                manager.transform.position = S.Posicao;//new Vector3(483,1.2f,755);  
                manager.transform.rotation = S.Rotacao;
                manager.Dados = S.Dados;
                GameController.g.ReiniciarContadorDeEncontro();

                GameObject[] Gs = GameObject.FindGameObjectsWithTag("Criature");

                for (int i = 0; i < Gs.Length; i++)
                    Destroy(Gs[i]);

                // if (manager.CriatureAtivo != null)
                {
                    //   MonoBehaviour.Destroy(manager.CriatureAtivo.gameObject);
                    manager.InserirCriatureEmJogo();
                    manager.CriatureAtivo.transform.position = S.Posicao + new Vector3(0, 0, 1);//new Vector3(483, 1.2f, 756);
                }


                manager.Dados.ZeraUltimoUso();
                GameController.g.MyKeys = S.VariaveisChave;
                GameController.g.Salvador.SetarJogoAtual(indiceDoJogo);

                podeIr = true;

                StartCoroutine(Status());
            }
        }

        IEnumerator Status()
        {
            yield return new WaitForEndOfFrame();
            RecolocadorDeStatus.VerificaStatusDosAtivos();
        }

        void SetarCenaPrincipal(Scene scene, LoadSceneMode mode)
        {
            if (S != null)
            {
                //Debug.Log(S.VariaveisChave.CenaAtiva.ToString()+" : "+ scene.name);
                if (scene.name == S.VariaveisChave.CenaAtiva.ToString())
                {
                    InvocarSetScene(scene);
                    SceneManager.sceneLoaded -= SetarCenaPrincipal;

                    ComoPode();

                    if (scene.name == NomesCenas.cavernaIntro.ToString())
                    {
                        //Debug.Log("cavernaInicial");
                    }


                }
            }
            else
            if (scene.name != "comunsDeFase")
            {
                podeIr = true;
                InvocarSetScene(scene);
                SceneManager.sceneLoaded -= SetarCenaPrincipal;


                CharacterManager manager = GameController.g.Manager;

                /*
                    novo jogo inicia sem itens e sem criatures
                */
                manager.Dados.CriaturesAtivos = new System.Collections.Generic.List<CriatureBase>();
                manager.Dados.CriaturesArmagedados = new System.Collections.Generic.List<CriatureBase>();
                manager.Dados.Itens = new System.Collections.Generic.List<MbItens>();
                GameController.g.ComCriature = false;
                /***************************/


                AplicadorDeCamera.cam.transform.position = new Vector3(49, 15f, 155); //new Vector3(411, 15f, 1569);
                manager.transform.position = new Vector3(39, 5.4f, 155); //new Vector3(519, 5.4f, 1894);
                manager.transform.rotation = Quaternion.LookRotation(Vector3.right);
                GameController.g.ReiniciarContadorDeEncontro();

                if (manager.CriatureAtivo != null)
                {
                    Debug.Log("Arquivo de save era nulo");
                    Destroy(manager.CriatureAtivo.gameObject);
                    manager.InserirCriatureEmJogo();
                    manager.CriatureAtivo.transform.position = new Vector3(49, 6, 155); //new Vector3(411, 5.101f, 1560);
                }

                GameController.g.MyKeys.SetarCenasAtivas(new NomesCenas[1] { NomesCenas.cavernaIntro });
                GameController.g.Salvador.SetarJogoAtual(indiceDoJogo);
            }

            GameController.g.Manager.SeletaDeCriatures();
            AplicadorDeCamera.cam.FocarDirecionavel();
        }

        IEnumerator setarScene(Scene scene)
        {
            yield return new WaitForSeconds(0.5f);
            InvocarSetScene(scene);
        }

        public void InvocarSetScene(Scene scene)
        {
            //Debug.Log(scene.name);
            SceneManager.SetActiveScene(scene);
            //Debug.Log(GameController.g+" : "+scene.name);
            if (SceneManager.GetActiveScene() != scene)
                StartCoroutine(setarScene(scene));

            //Debug.Log("nomeAtiva: " + SceneManager.GetActiveScene().name);
        }

        public void Update()
        {
            switch (fase)
            {
                case FasesDoLoad.carregando:

                    tempo += Time.fixedDeltaTime;

                    float progresso = 0;

                    for (int i = 0; i < a2.Length; i++)
                    {
                        progresso += a2[i].progress;
                    }

                    progresso /= a2.Length;

                    //Debug.Log(progresso + " : " + (tempo / tempoMin) + " : " + Mathf.Min(progresso, tempo / tempoMin, 1));

                    loadBar.ValorParaBarra(Mathf.Min(progresso, tempo / tempoMin, 1));

                    if (podeIr && tempo >= tempoMin)
                    {
                        GameObject go = GameObject.Find("EventSystem");
                        if (go)
                            SceneManager.MoveGameObjectToScene(go, SceneManager.GetSceneByName("comunsDeFase"));

                        FadeView pm = GameController.g.gameObject.AddComponent<FadeView>();
                        pm.vel = 2;
                        fase = FasesDoLoad.escurecendo;
                        tempo = 0;
                    }

                    break;
                case FasesDoLoad.escurecendo:
                    tempo += Time.fixedDeltaTime;
                    if (tempo > 0.95f)
                    {
                        FindObjectOfType<FadeView>().entrando = false;
                        FindObjectOfType<Canvas>().enabled = false;
                        fase = FasesDoLoad.clareando;
                        SceneManager.SetActiveScene(
                           SceneManager.GetSceneByName(GameController.g.MyKeys.CenaAtiva.ToString()));
                        InformacoesDeCarregamento.FacaModificacoes();
                        GameController.g.Salvador.SalvarAgora();
                        Time.timeScale = 1;
                        SceneManager.UnloadSceneAsync("CenaDeCarregamento");
                        tempo = 0;
                    }
                    break;
                case FasesDoLoad.clareando:
                    tempo += Time.fixedDeltaTime;
                    if (tempo > 0.5f)
                    {

                        Destroy(gameObject);
                    }
                    break;
            }
        }
    }
}