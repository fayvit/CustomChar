using UnityEngine;
using System.Collections;
using FayvitUI;

namespace FayvitBasicTools
{
    public interface IGameController
    {
        public KeyVar MyKeys { get; }
        public GameObject ThisGameObject { get; }
    }
}