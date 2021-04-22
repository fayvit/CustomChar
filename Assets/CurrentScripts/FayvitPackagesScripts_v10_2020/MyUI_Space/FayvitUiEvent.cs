using UnityEngine;
using System.Collections;

namespace FayvitUI_10_2020
{
    public class FayvitUiEvent : IFayvitUiEvent
    {
        public object[] MySendObjects { get; private set; }

        public UIEventKey Key { get; private set; }

        public FayvitUiEvent(UIEventKey key, params object[] o)
        {
            Key = key;
            MySendObjects = o;
        }
    }
}
