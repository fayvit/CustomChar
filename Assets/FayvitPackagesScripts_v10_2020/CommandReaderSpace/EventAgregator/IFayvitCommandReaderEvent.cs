using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitCommandReader_10_2020
{
    public interface IFayvitCommandReaderEvent
    {
        object[] MySendObjects { get; }
        FayvitCR_EventKey Key { get; }
    }
}