using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class DamageColliderFactory
    {
        public static DamageColliderBase Get(GameObject G, ProjetilType t)
        {
            return t switch
            {
                ProjetilType.basico => G.AddComponent<DamageCollider>(),
                ProjetilType.rigido => G.AddComponent<DamageColliderRigid>(),
                _ => null
            };
        }
    }
}