using UnityEngine;
using System.Collections;
using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitSupportSingleton;

namespace Criatures2021
{
    public class MyGameController : AbstractGameController
    {

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            ToSaveCustomizationContainer.Instance.Load();

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgApperanceTransport>.Publish(new MsgApperanceTransport()
                {
                    lccd = ToSaveCustomizationContainer.Instance.ccds
                });
            });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}
