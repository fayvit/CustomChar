using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMessageAgregator;
using FayvitMove;
using FayvitBasicTools;
using Criatures2021;
using System;

public class HumanAnimationListener : MonoBehaviour
{
    [SerializeField] private string velString = "velocidade";
    [SerializeField] private string jumpAnimationName = "pulando";
    [SerializeField] private string jumpAnimationBool = "pulo";
    [SerializeField] private string groundedBool = "noChao";
    [SerializeField] private string callBool = "chama";
    [SerializeField] private string sendBool = "envia";
    [SerializeField] private string lockBool = "travar";
    [SerializeField] private string captureAnimationName = "capturou";

    private Animator A;

    // Start is called before the first frame update
    void Start()
    {
        A = GetComponent<Animator>();
        MessageAgregator<ChangeMoveSpeedMessage>.AddListener(OnChangeMoveSpeed);
        MessageAgregator<AnimateStartJumpMessage>.AddListener(OnStartJump);
        MessageAgregator<AnimateDownJumpMessage>.AddListener(OnDownJump);
        MessageAgregator<MsgRequestCallAnimation>.AddListener(OnRequestCall);
        MessageAgregator<MsgRequestSendAnimation>.AddListener(OnRequestSend);
        MessageAgregator<MsgRequestEndArmsAnimations>.AddListener(OnRequestEndArmAnimations);
        MessageAgregator<MsgAnimaCaptura>.AddListener(OnStartCapture);
        MessageAgregator<MsgEndOfCaptureAnimate>.AddListener(OnEndCapture);
    }

    private void OnDestroy()
    {
        MessageAgregator<ChangeMoveSpeedMessage>.RemoveListener(OnChangeMoveSpeed);
        MessageAgregator<AnimateStartJumpMessage>.RemoveListener(OnStartJump);
        MessageAgregator<AnimateDownJumpMessage>.RemoveListener(OnDownJump);
        MessageAgregator<MsgRequestCallAnimation>.RemoveListener(OnRequestCall);
        MessageAgregator<MsgRequestSendAnimation>.RemoveListener(OnRequestSend);
        MessageAgregator<MsgRequestEndArmsAnimations>.RemoveListener(OnRequestEndArmAnimations);
        MessageAgregator<MsgAnimaCaptura>.RemoveListener(OnStartCapture);
        MessageAgregator<MsgEndOfCaptureAnimate>.RemoveListener(OnEndCapture);
    }

    private void OnEndCapture(MsgEndOfCaptureAnimate obj)
    {
        if (obj.dono == gameObject)
        {
            A.SetBool(lockBool, false);
        }
    }

    private void OnStartCapture(MsgAnimaCaptura obj)
    {
        if (obj.dono == gameObject)
        {
            A.SetBool(lockBool, true);
            A.SetBool(callBool, false);
            A.Play(captureAnimationName);
        }
    }

    private void OnRequestEndArmAnimations(MsgRequestEndArmsAnimations obj)
    {
        if (obj.oAnimado == gameObject)
        {
            A.SetBool(callBool, false);
            A.SetBool(sendBool, false);
        }
    }

    private void OnRequestSend(MsgRequestSendAnimation obj)
    {
        if (obj.oAnimado == gameObject)
        {
            A.SetBool(sendBool, true);
        }
    }

    private void OnRequestCall(MsgRequestCallAnimation obj)
    {
        if (obj.oAnimado == gameObject)
        {
            A.SetBool(callBool, true);
        }
    }

    private void OnDownJump(AnimateDownJumpMessage obj)
    {
        if (obj.gameObject == gameObject)
        {
            A.SetBool(jumpAnimationBool, false);
            A.SetBool(groundedBool, true);
        }
    }

    private void OnStartJump(AnimateStartJumpMessage obj)
    {
        if (obj.gameObject == gameObject)
        {
            A.Play(jumpAnimationName);
            A.SetBool(jumpAnimationBool, true);
            A.SetBool(groundedBool, false);

            MessageAgregator<MsgRequest3dSound>.Publish(new MsgRequest3dSound()
            {
                sfxId = FayvitSounds.SoundEffectID.XP_Swing03,
                sender = transform
            });
        }
    }

    private void OnChangeMoveSpeed(ChangeMoveSpeedMessage obj)
    {
        if (obj.gameObject == gameObject)
        {
            float f = obj.velocity.sqrMagnitude;
            A.SetFloat(velString, f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
