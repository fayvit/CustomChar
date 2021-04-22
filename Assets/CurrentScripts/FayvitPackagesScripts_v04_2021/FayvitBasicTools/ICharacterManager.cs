using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitBasicTools
{
    public interface ICharacterManager
    {
        public CharacterState ThisState{get;}
    }

    public enum CharacterState
    {
        notStarted = -1,
        onFree,
        stoped,
        withPet,
        externalMovement
    }
}
