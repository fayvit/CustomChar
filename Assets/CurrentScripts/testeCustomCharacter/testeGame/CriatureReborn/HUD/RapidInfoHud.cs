using FayvitMessageAgregator;
using FayvitUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Criatures2021Hud
{
    public class RapidInfoHud : MonoBehaviour
    {
        [SerializeField] private Text infoText;
        [SerializeField] private RectTransform messagePanel;
        [SerializeField] private float tempoVisualizando;
        [SerializeField] private float tempoVisualizandoMensIgual;
        [SerializeField] private float tempoVindo;
        [SerializeField] private float tempoIndo;

        private Queue<string> filaDeMensagens = new Queue<string>();
        private Vector2 originalAnchoredPosition;
        private TextDisplay.MessagePhase phase = TextDisplay.MessagePhase.boxExited;
        private float contadorDeTempo = 0;

        // Start is called before the first frame update
        void Start()
        {
            originalAnchoredPosition = messagePanel.anchoredPosition;
            messagePanel.gameObject.SetActive(false);
            MessageAgregator<MsgRequestRapidInfo>.AddListener(OnRequestRapidInfo);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgRequestRapidInfo>.AddListener(OnRequestRapidInfo);
        }

        private void OnRequestRapidInfo(MsgRequestRapidInfo obj)
        {
            Debug.Log(obj.message);
            filaDeMensagens.Enqueue(obj.message);
            if (phase == TextDisplay.MessagePhase.boxExited)
                StartMessageExibition();
            
        }

        void VerifyStartNewMessage()
        {
            if (filaDeMensagens.Count > 0)
                StartMessageExibition();
            
        }

        void StartMessageExibition()
        {
            messagePanel.gameObject.SetActive(true);
            messagePanel.anchoredPosition = new Vector2(Screen.width, originalAnchoredPosition.y);
            infoText.text = filaDeMensagens.Dequeue();
            contadorDeTempo = 0;
            phase = TextDisplay.MessagePhase.boxOut;
        }

        // Update is called once per frame
        void Update()
        {
            if(phase!=TextDisplay.MessagePhase.boxExited)
                contadorDeTempo += Time.deltaTime;

            switch (phase)
            {
                case TextDisplay.MessagePhase.boxOut:
                    if (Vector2.Distance(messagePanel.anchoredPosition, originalAnchoredPosition) > 0.1f)
                    {
                        messagePanel.anchoredPosition = Vector2.Lerp(
                            messagePanel.anchoredPosition, originalAnchoredPosition, contadorDeTempo/tempoVindo);
                    }
                    else
                    {
                        phase = TextDisplay.MessagePhase.messageFilling;
                        contadorDeTempo = 0;
                    }
                    break;
                case TextDisplay.MessagePhase.messageFilling:
                    float tempoDeView = tempoVisualizando;

                    if (filaDeMensagens.Count > 1 && infoText.text==filaDeMensagens.Peek())
                    {
                        tempoDeView = tempoVisualizandoMensIgual;
                    }

                    if (contadorDeTempo > tempoDeView)
                    {
                        contadorDeTempo = 0;
                        phase = TextDisplay.MessagePhase.boxGoingOut;
                    }
                break;
                case TextDisplay.MessagePhase.boxGoingOut:
                    if (Mathf.Abs(messagePanel.anchoredPosition.x - Screen.width) > 0.1f)
                    {
                        messagePanel.anchoredPosition = Vector2.Lerp(messagePanel.anchoredPosition,
                                                            new Vector2( Screen.width,messagePanel.anchoredPosition.y),
                                                            contadorDeTempo/tempoIndo);
                    }
                    else
                    {
                        phase = TextDisplay.MessagePhase.boxExited;
                        VerifyStartNewMessage();
                    }
                break;
            }


        }
    }

    public struct MsgRequestRapidInfo : IMessageBase
    {
        public string message;
    }
}