using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using System.Collections.Generic;

namespace Npc2021
{
    public class NpcAppearanceConfiguration : MonoBehaviour
    {

        [SerializeField] private string sId;

        [SerializeField] private TesteMeshCombiner tCombiner;
        //[SerializeField] private SectionCustomizationManager secM;
        //[SerializeField] private SectionCustomizationManager secH;

        // Use this for initialization
        void Start()
        {
            MessageAgregator<MsgApperanceTransport>.AddListener(OnReceiveApperance);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgApperanceTransport>.RemoveListener(OnReceiveApperance);
        }

        private void OnReceiveApperance(MsgApperanceTransport obj)
        {
            foreach (var v in obj.lccd)
                if (v.Sid == sId)
                {
                    Create(v);
                }
        }

        void Create(CustomizationContainerDates ccd)
        {
            tCombiner.StartCombiner(ccd);

            //GameObject G;
            //if (ccd.PersBase == PersonagemBase.feminino)
            //{
            //    secM.SetCustomDates(ccd);
            //    G = secM.gameObject;
            //    Destroy(secH.gameObject);
            //}
            //else
            //{
            //    Destroy(secM.gameObject);
            //    secH.SetCustomDates(ccd);
            //    G = secH.gameObject;
            //}
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    
}

public struct MsgApperanceTransport : IMessageBase
{
    public List<CustomizationContainerDates> lccd;
}
