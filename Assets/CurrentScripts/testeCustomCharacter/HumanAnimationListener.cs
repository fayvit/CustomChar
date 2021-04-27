using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMessageAgregator;
using FayvitMove;
using FayvitBasicTools;

public class HumanAnimationListener : MonoBehaviour
{
    [SerializeField] private string velString = "velocidade";
    [SerializeField] private string jumpAnimationName = "pulando";
    [SerializeField] private string jumpAnimationBool = "pulo";
    [SerializeField] private string groundedBool = "noChao";
    private Animator A;
    // Start is called before the first frame update
    void Start()
    {
        A = GetComponent<Animator>();
        MessageAgregator<ChangeMoveSpeedMessage>.AddListener(OnChangeMoveSpeed);
        MessageAgregator<AnimateStartJumpMessage>.AddListener(OnStartJump);
        MessageAgregator<AnimateDownJumpMessage>.AddListener(OnDownJump);
    }

    private void OnDestroy()
    {
        MessageAgregator<ChangeMoveSpeedMessage>.RemoveListener(OnChangeMoveSpeed);
        MessageAgregator<AnimateStartJumpMessage>.RemoveListener(OnStartJump);
        MessageAgregator<AnimateDownJumpMessage>.RemoveListener(OnDownJump);
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
