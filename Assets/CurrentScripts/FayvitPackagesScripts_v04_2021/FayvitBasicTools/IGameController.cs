﻿using UnityEngine;
using System.Collections;

namespace FayvitBasicTools
{
    public interface IGameController
    {
        public KeyVar MyKeys { get; }
        public GameObject ThisGameObject{get;}
    }
}