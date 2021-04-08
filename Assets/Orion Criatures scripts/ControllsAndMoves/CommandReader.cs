using UnityEngine;
using System.Collections.Generic;

namespace CriaturesLegado
{
    public class CommandReader
    {
        public static bool useiAcao = false;
        private Dictionary<string, bool> zerados = new Dictionary<string, bool>()
    {
        { "Attack",true },
        { "Jump",true},
        { "alternarAttack_Criature",true},
        { "alternarItems",true },
        { "EscolhaV",true},
        { "EscolhaH",true},
        { "VerticalTeclado",true},
        { "HorizontalTeclado",true}

    };

        // Use this for initialization
        void Start()
        {

        }

        bool CondicaoDeUsoDeAlternadores
        {
            get
            {
                bool retorno = true;
                CharacterManager manager = GameController.g.Manager;
                if (manager.CriatureAtivo != null)
                {
                    CreatureManager.CreatureState estadoDoCriature = manager.CriatureAtivo.Estado;
                    if (manager.Estado == EstadoDePersonagem.parado && manager.Estado != EstadoDePersonagem.comMeuCriature)
                        retorno = false;
                    else if (manager.Estado == EstadoDePersonagem.comMeuCriature
                        &&
                        (estadoDoCriature == CreatureManager.CreatureState.parado || estadoDoCriature == CreatureManager.CreatureState.morto))
                        retorno = false;
                }
                else
                    retorno = false;

                return retorno;

            }
        }

        // Update is called once per frame
        public void Update()
        {
            if (!GameController.g.HudM.MenuDePause.EmPause)
            {
                CharacterManager manager = GameController.g.Manager;

                if (!ActionManager.useiCancel)
                {
                    if (Input.GetButtonDown("Jump"))//|| ValorDeGatilhos("Jump") > 0)
                        GameController.g.BotaoPulo();

                    if (GameController.g.MyKeys.VerificaAutoShift(KeyShift.estouNoTuto))
                        if (Input.GetButtonDown("Alternador")
                            && GameController.g.EmEstadoDeAcao()
                            && !GameController.g.estaEmLuta)
                            GameController.g.BotaoAlternar();

                    if (Input.GetButtonDown("Pause"))
                    {
                        GameController.g.HudM.MenuDePause.PausarJogo();
                        ActionManager.useiCancel = true;
                    }
                }
                else
                    ActionManager.useiCancel = false;

                if ((Input.GetButtonDown("Attack") && !useiAcao || ValorDeGatilhos("Attack") > 0) && manager.Estado == EstadoDePersonagem.comMeuCriature)
                    GameController.g.BotaoAtaque();
                else if (useiAcao)
                    useiAcao = false;

                float val = ValorDeGatilhos("alternarAttack_Criature");
                if ((val < 0 || Input.GetButtonDown("alternarAttack")) && manager.Estado == EstadoDePersonagem.comMeuCriature)
                    if (manager.CriatureAtivo.Estado != CreatureManager.CreatureState.parado
                        &&
                        manager.CriatureAtivo.Estado != CreatureManager.CreatureState.morto
                        )
                        GameController.g.BotaoMaisAtaques(1);


                if (val > 0 && manager.Dados.CriaturesAtivos.Count > 1 && CondicaoDeUsoDeAlternadores)
                    GameController.g.BotaoMaisCriature(1);

                val = ValorDeGatilhos("alternarItems");
                if (val != 0 && manager.Dados.Itens.Count > 0 && CondicaoDeUsoDeAlternadores)
                    GameController.g.BotaItens(val > 0 ? 1 : -1);

                if (Input.GetButtonDown("usaItem") && GameController.g.EmEstadoDeAcao() && manager.Dados.Itens.Count > 0)
                    GameController.g.BotaUsarItem();

                if (Input.GetButtonDown("trocaCriature") && GameController.g.EmEstadoDeAcao() && manager.Dados.CriaturesAtivos.Count > 1)
                    GameController.g.BotaTrocarCriature();

                if (Input.GetButtonDown("Acao"))
                    ActionManager.VerificaAcao();
            }
            /*
            if (Input.GetButtonDown("Cancel"))
                GameController.g.HudM.PauseM.PausarJogo();

            if (Input.GetButtonDown("Ataque") && manager.Estado == EstadoDePersonagem.comMeuCriature)
                GameController.g.BotaoAtaque();



            if (Input.GetButtonDown("maisAtaques") && manager.Estado == EstadoDePersonagem.comMeuCriature)
                GameController.g.BotaoMaisAtaques();



            if (Input.GetButtonDown("maisCriatures") && manager.Dados.CriaturesAtivos.Count > 1)
                GameController.g.BotaoMaisCriature();
                */
        }

        public bool DisparaAcao()
        {
            if (Input.GetButtonDown("Acao") && !ActionManager.anularAcao)
            {
                ActionManager.anularAcao = true;
                return true;
            }
            else
                ActionManager.anularAcao = false;

            return false;
        }

        public bool DisparaCancel()
        {
            if (Input.GetButtonDown("Cancel") && !ActionManager.useiCancel)
            {
                ActionManager.useiCancel = true;
                return true;
            }
            else
                ActionManager.useiCancel = false;
            return false;
        }

        public Vector3 DirDeEixos()
        {
            float h = Mathf.Clamp(Input.GetAxis("Horizontal") + Input.GetAxis("HorizontalTeclado"), -1, 1);
            float v = Mathf.Clamp(Input.GetAxis("Vertical") + Input.GetAxis("VerticalTeclado"), -1, 1);

            return Vector3.ProjectOnPlane(new Vector3(h, 0, v), Vector3.up).normalized;
        }

        public Vector3 VetorDirecao()
        {
            Vector3 V = Vector3.zero;

            float h = Mathf.Clamp(Input.GetAxis("Horizontal") + Input.GetAxis("HorizontalTeclado"), -1, 1);
            float v = Mathf.Clamp(Input.GetAxis("Vertical") + Input.GetAxis("VerticalTeclado"), -1, 1);

            Vector3 forward = new Vector3(1, 0, 0);
            if (AplicadorDeCamera.cam != null)
                if (AplicadorDeCamera.cam.Cdir != null)
                    forward = AplicadorDeCamera.cam.Cdir.DirecaoInduzida(h, v);

            forward.y = 0;
            forward = forward.normalized;



            Vector3 right = new Vector3(forward.z, 0, -forward.x);

            return h * right + v * forward;

        }

        public int ValorDeGatilhos(string esseGatilho)
        {
            int retorno = 0;

            float val = Input.GetAxisRaw(esseGatilho);
            if (zerados[esseGatilho])
            {
                if (val != 0)
                {
                    zerados[esseGatilho] = false;

                }

                if (val > 0)
                    retorno = 1;
                else if (val < 0)
                    retorno = -1;

            }
            else
            {

                retorno = 0;
                if (val > -0.1f && val < 0.1f)
                    zerados[esseGatilho] = true;

            }


            return retorno;

        }

        public int ValorDeGatilhosTeclado(string esseGatilho)
        {
            int retorno = 0;
            float val = Input.GetAxisRaw(esseGatilho);

            if (zerados[esseGatilho])
            {
                if (val != 0)
                {
                    zerados[esseGatilho] = false;
                }

                if (val > 0)
                    retorno = 1;
                else if (val < 0)
                    retorno = -1;

            }
            else
            {

                retorno = 0;
                if (val > -0.01f && val < 0.01f)
                    zerados[esseGatilho] = true;

            }

            return retorno;
        }
    }
}