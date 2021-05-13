using UnityEngine;
using System.Collections;
using FayvitBasicTools;
using Criatures2021;
using FayvitMessageAgregator;

namespace Criatures2021Hud
{
    public abstract class ButtonActivate : MonoBehaviour
    {
        //[SerializeField] protected GameObject btn;
        [SerializeField] protected float distanciaParaAcionar = 1.5f;
        protected string textoDoBotao = "";
        private bool estaNoTrigger = false;

        //public GameObject Btn { get { return btn; } }

        // Use this for initialization
        void Start()
        {

        }

        protected void FluxoDeBotao()
        {

            Update();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (AbstractGlobalController.Instance!=null 
                && AbstractGlobalController.Instance.Players!=null 
                && AbstractGlobalController.Instance.Players.Count > 0
                )
                
                    if (Vector3.Distance(AbstractGlobalController.Instance.Players[0].Manager.transform.position, transform.position) < distanciaParaAcionar
                        &&
                        estaNoTrigger
                        &&
                        AbstractGlobalController.Instance.Players[0].Manager.ThisState == CharacterState.onFree
                        &&
                        ActionManager.Instance.PodeVisualizarEste(this)
                        //  &&
                        // GameController.g.EmEstadoDeAcao()
                        &&
                        gameObject.activeSelf
                        )
                    {
                        MessageAgregator<MsgRequestShowActionHud>.Publish(
                            new MsgRequestShowActionHud() { 
                            infoCommand = "L",
                            infoText = "Conversar"
                            }
                            );
                        //btn.SetActive(true);
                    }
                    else
                    {
                        MessageAgregator<MsgRequestHideActionHud>.Publish();
                        /*
                        if (Vector3.Distance(GameController.g.Manager.transform.position, transform.position) >= distanciaParaAcionar)
                        {*/
                        //btn.SetActive(false);
                        /*  }else
                          if (ActionManager.TransformDeActionE(transform))
                          {

                              btn.SetActive(true);
                          }*/

                    }

        }
        protected void SempreEstaNoTrigger()
        {
            estaNoTrigger = true;
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                estaNoTrigger = true;
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                estaNoTrigger = false;
            }
        }

        /*
        public virtual void SomDoIniciar()
        {
            EventAgregator.Publish(new StandardSendStringEvent(gameObject, SoundEffectID.Decision1.ToString(), EventKey.disparaSom));
        }*/

        public abstract void FuncaoDoBotao();
    }

}