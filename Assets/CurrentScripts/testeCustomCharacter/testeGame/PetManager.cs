using Criatures2021;
using FayvitBasicTools;
using FayvitCam;
using FayvitCommandReader;
using FayvitMessageAgregator;
using FayvitMove;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{

    [SerializeField] private LocalState state = LocalState.following;
    [SerializeField] private Transform tDono;
    [SerializeField] private PetBase meuCriatureBase;
    [SerializeField] private ControlledMoveForCharacter controll;
    [SerializeField] private float targetAtualizationTax = 1;

    private float timeCount = 0;

    private enum LocalState
    { 
        following,
        onFree,
        inFight,
        stopped
    }

    private ICommandReader CurrentCommander { 
        get => CommandReader.GetCR(AbstractGlobalController.Instance.Control); 
    }

    public PetBase MeuCriatureBase { 
        get => meuCriatureBase; 
        set 
        { 
            meuCriatureBase = value;
            controll = new ControlledMoveForCharacter(transform);
            controll.SetCustomMove(meuCriatureBase.MovFeat);
        } 
    }

    public Transform T_Dono { get => tDono; set => tDono = value; }

    public BasicMove Mov
    {
        get
        {
            if (controll == null)
                SetaMov();
            return controll.Mov;
        }
    }

    void SetaMov()
    {
        controll = new ControlledMoveForCharacter(transform);
        controll.SetCustomMove(meuCriatureBase.MovFeat);
        //mov = new BasicMove(
        //               meuCriatureBase.Mov, new ElementosDeMovimentacao()
        //               {
        //                   animador = new AnimadorHumano(GetComponent<Animator>()),
        //                   controle = GetComponent<CharacterController>(),
        //                   transform = transform
        //               }
        //                );
    }

    void AplicaGolpe()
    {
        PetAtributes A = MeuCriatureBase.PetFeat.meusAtributos;
        PetAttackBase gg = meuCriatureBase.GerenteDeGolpes.meusGolpes[meuCriatureBase.GerenteDeGolpes.golpeEscolhido];
        Debug.Log("no chão: " + controll.Mov.IsGrounded);
        if ((controll.Mov.IsGrounded || gg.PodeNoAr))
        {
            Debug.Log("Criar disparador de golpes");

            //if (!DisparadorDoGolpe.Dispara(meuCriatureBase, gameObject))
            //{

            //    string[] textos = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.usoDeGolpe).ToArray();

            //    // Esse mostrador de tempo deveria ser uma classe separada,
            //    //Pode-se desacoplar GameController

            //    Debug.LogError("Modificação extremamente necessária");
            //    if (gg.UltimoUso + gg.TempoDeReuso >= Time.time)
            //        GameController.g.HudM.Painel.AtivarNovaMens(
            //              string.Format(textos[0], ShowConvertTime.Show(gg.UltimoUso - (Time.time - gg.TempoDeReuso)))
            //            , 25, 2);

            //    else if (A.PE.Corrente < gg.CustoPE)
            //        GameController.g.HudM.Painel.AtivarNovaMens(textos[1], 25, 2);
            //}
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MessageAgregator<MsgChangeToPet>.AddListener(OnChangeToPet);
    }

    private void OnDestroy()
    {
        MessageAgregator<MsgChangeToPet>.RemoveListener(OnChangeToPet);
    }

    private void OnChangeToPet(MsgChangeToPet obj)
    {
        if (obj.dono == tDono)
        {
            state = LocalState.onFree;
            CameraAplicator.cam.FocusForDirectionalCam(transform, meuCriatureBase.alturaCamera, meuCriatureBase.distanciaCamera);
            
        }
    }

    void BaseAction()
    {
        switch (state)
        {
            case LocalState.following:
                timeCount += Time.deltaTime;
                if (timeCount > targetAtualizationTax && tDono != null)
                {
                    timeCount = 0;
                    controll.ModificarOndeChegar(tDono.position);
                }

                if (controll.UpdatePosition(2.5f))
                {

                }
                break;
            case LocalState.onFree:
                Vector3 V = CameraAplicator.cam.SmoothCamDirectionalVector(
                CurrentCommander.GetAxis("horizontal"),
                CurrentCommander.GetAxis("vertical")
                );
                bool run = CurrentCommander.GetButton(2);
                bool startJump = CurrentCommander.GetButtonDown(3);
                bool pressJump = CurrentCommander.GetButton(3);
                controll.Mov.MoveApplicator(V, run, startJump, pressJump);
            break;
        }
    }

    void CamAction()
    {
        Vector2 V = new Vector3(
            CurrentCommander.GetAxis("Xcam"),
            CurrentCommander.GetAxis("Ycam")
            );

        bool focar = CurrentCommander.GetButtonDown(9);
        CameraAplicator.cam.ValoresDeCamera(V.x, V.y, focar, controll.Mov.Controller.velocity.sqrMagnitude > .1f);
    }

    // Update is called once per frame
    void Update()
    {
        BaseAction();
        CamAction();
    }

    // eram comandos para o android mas já há comandos melhores para isso
    //public void ComandoDeAtacar()
    //{
    //    if (state == LocalState.onFree || state == LocalState.inFight)
    //        AplicaGolpe();
    //}

    //public void IniciaPulo()
    //{
    //    if (!meuCriatureBase.Mov.caracPulo.estouPulando && (state == LocalState.onFree || state == LocalState.inFight))
    //        mov._Pulo.IniciaAplicaPulo();
    //}

    public void PararCriatureNoLocal()
    {
        state = LocalState.stopped;
        //ia.SuspendeNav();
        // indicativo de necessidade de divisão
    }
}
