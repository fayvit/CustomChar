using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitUI_10_2020
{
    public interface IFayvitUiEvent
    {
        object[] MySendObjects { get; }
        UIEventKey Key { get; }
    }
}