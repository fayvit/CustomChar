using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMessageAgregator;
using Criatures2021;
using FayvitCam;
using FayvitSupportSingleton;

public class ProvisionalStartGame   : MonoBehaviour
{
    private TesteMeshCombiner meshCombiner;
    private SectionCustomizationManager secManager;

    private Transform character;
    private string guid;

    public static void InitProvisionalStartGame(
        TesteMeshCombiner meshCombiner,
        SectionCustomizationManager secManager)
    {
        GameObject G = new GameObject();
        ProvisionalStartGame P = G.AddComponent<ProvisionalStartGame>();
        P.SetDates(meshCombiner, secManager);
        P.Init();
    }

    public void SetDates(
        TesteMeshCombiner meshCombiner,
        SectionCustomizationManager secManager)
    {
        this.meshCombiner = meshCombiner;
        this.secManager = secManager;
    }

    public void Init()
    {

        MessageAgregator<MsgCombinationComplete>.AddListener(OnCombinationComplete);
        guid = System.Guid.NewGuid().ToString();
        meshCombiner.StartCombiner(secManager,guid);
        
    }

    private void OnCombinationComplete(MsgCombinationComplete obj)
    {
        if (obj.checkKey == guid)
        {
            character = obj.combined;

            Debug.Log(character);

            Criatures2021.CharacterManager c = character.gameObject.AddComponent<Criatures2021.CharacterManager>();


            CameraAplicator.cam.FocusForDirectionalCam(character, .1f,3);
            CameraAplicator.cam.Cdir.VarVerticalHeightPoint = 1;
            
            
            Destroy(CameraAplicator.cam.GetComponent<CustomizationCamManager>());
            Destroy(CameraAplicator.cam.transform.GetChild(0).gameObject);
            CameraAplicator.cam.GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
            

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgCombinationComplete>.RemoveListener(OnCombinationComplete);
            });

            MessageAgregator<MsgFinishEdition>.Publish();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public struct MsgFinishEdition : IMessageBase { }