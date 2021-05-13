﻿using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class FindByOwner : MonoBehaviour
    {
        public static Transform GetEnemy(GameObject owner)
        {
            return owner.GetComponent<CharacterManager>().ActivePet.Mov.LockTarget;
        }

        public static PetManager GetManagerEnemy(GameObject owner)
        {
            if (owner.GetComponent<CharacterManager>().ActivePet.Mov.LockTarget != null)
                return owner.GetComponent<CharacterManager>().ActivePet.Mov.LockTarget.GetComponent<PetManager>();
            else
                return null;
        }

    }
}