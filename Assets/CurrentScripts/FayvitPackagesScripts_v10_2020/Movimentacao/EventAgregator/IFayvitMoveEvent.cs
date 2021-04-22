using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitMove_10_2020
{
    public interface IFayvitMoveEvent
    {
        object[] MySendObjects { get; }
        FayvitMoveEventKey Key { get; }
    }
}