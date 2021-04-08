using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitCam_10_2020
{
    public interface IFayvitCamEvent
    {
        object[] MySendObjects { get; }
        FayvitCamEventKey Key { get; }
    }
}