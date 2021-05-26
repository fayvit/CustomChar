using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitBasicTools
{
    public interface ICharacterManager
    {
        public CharacterState ThisState{get;}
        public Transform transform { get; }
    }

    public enum CharacterState
    {
        notStarted = -1,
        onFree,
        stopedWithStoppedCam,
        withPet,
        externalMovement,
        externalPanelOpened,
        activeSingleMessageOpened,
        activeConfirmationOpened,
        stopped,
        NonBlockPanelOpened
    }
}
