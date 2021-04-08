using UnityEngine;
using System.Collections;

namespace FayvitMove_10_2020
{
    public class FayvitMoveEvent : IFayvitMoveEvent
    {
        public object[] MySendObjects { get; private set; }

        public FayvitMoveEventKey Key { get; private set; }

        public FayvitMoveEvent(FayvitMoveEventKey key, params object[] o)
        {
            Key = key;
            MySendObjects = o;
        }
    }
}
