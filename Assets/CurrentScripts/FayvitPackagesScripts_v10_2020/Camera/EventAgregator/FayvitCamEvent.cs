using UnityEngine;
using System.Collections;

namespace FayvitCam_10_2020
{
    public class FayvitCamEvent : IFayvitCamEvent
    {
        public object[] MySendObjects { get; private set; }

        public FayvitCamEventKey Key { get; private set; }

        public FayvitCamEvent(FayvitCamEventKey key, params object[] o)
        {
            Key = key;
            MySendObjects = o;
        }
    }
}
