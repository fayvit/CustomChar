using FayvitCommandReader;
using FayvitUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitBasicTools
{
    public interface IGlobalController
    { 
        //Essa propriedade deveria ser Statica mas não é permitido em interface
        //public IGlobalController InterfaceInstance { get; }
        public List<IPlayersInGameDb> Players { get; set; }
        public Controlador Control { get; }
        public ConfirmationPanel Confirmation { get; }
        public SingleMessagePanel OneMessage { get; }
        public IFadeView FadeV { get; }
        public float MusicVolume { get; set; }
        public float SfxVolume { get; set; }
        public bool EmTeste { get; set; }
        //public ContainerDeDadosDeCena SceneDates { get { return sceneDates; } private set { sceneDates = value; } }
        public Vector3 InitialGamePosition { get;}
    }
}
