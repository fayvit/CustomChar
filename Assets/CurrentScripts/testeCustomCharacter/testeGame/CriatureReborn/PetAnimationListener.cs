using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using FayvitMove;

public class PetAnimationListener : MonoBehaviour
{
    [SerializeField] private string velAnimatorString = "velocidade";
    [SerializeField] private string jumpNameBool = "pulo";
    [SerializeField] private string groundedNameBool = "noChao";
    [SerializeField] private string jumpAnimationStateName = "pulando";

    [SerializeField] private Animator A;


    // Use this for initialization
    void Start()
    {
        A = GetComponent<Animator>();
        MessageAgregator<ChangeMoveSpeedMessage>.AddListener(OnChangeSpeed);
        MessageAgregator<AnimateStartJumpMessage>.AddListener(OnStartJump);
        MessageAgregator<AnimateDownJumpMessage>.AddListener(OnDownJump);
    }

    private void OnDestroy()
    {
        MessageAgregator<ChangeMoveSpeedMessage>.RemoveListener(OnChangeSpeed);
        MessageAgregator<AnimateStartJumpMessage>.RemoveListener(OnStartJump);
        MessageAgregator<AnimateDownJumpMessage>.RemoveListener(OnDownJump);
    }

    private void OnDownJump(AnimateDownJumpMessage obj)
    {
        if (obj.gameObject == gameObject)
        {
            A.SetBool(jumpNameBool, false);
            A.SetBool(groundedNameBool, true);
        }
    }

    private void OnStartJump(AnimateStartJumpMessage obj)
    {
        if (obj.gameObject == gameObject)
        {
            A.Play(jumpAnimationStateName);
            A.SetBool(jumpNameBool,true);
            A.SetBool(groundedNameBool, false);
        }
    }

    private void OnChangeSpeed(ChangeMoveSpeedMessage obj)
    {
        if (obj.gameObject == gameObject)
        {
            A.SetFloat(velAnimatorString,obj.velocity.sqrMagnitude);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
