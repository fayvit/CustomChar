using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Criatures2021
{
    public class ProvisionalEnemyInstantiate : MonoBehaviour
    {
        [SerializeField] private List<PetManager> emCampo = new List<PetManager>();
        [SerializeField] private int maxCriatures = 3;
        [SerializeField] private float timeToSpawn = 7.5f;

        private float tempoDecorrido = 0;

        // Use this for initialization
        void Start()
        {
            Spawn();
        }

        void Spawn()
        {
            Vector3 V = new Vector3(
                    Random.Range(450, 470),
                    1,
                    Random.Range(510, 530)
                    );
            emCampo.Add(WildPetInitialize.Initialize(PetName.Xuash, 11, V));
            emCampo.Add(WildPetInitialize.Initialize(PetName.Florest, 11, V));
            emCampo.Add(WildPetInitialize.Initialize(PetName.PolyCharm, 11, V));
        }

        // Update is called once per frame
        void Update()
        {
            tempoDecorrido += Time.deltaTime;
            if (tempoDecorrido > timeToSpawn)
            {
                List<int> retirar = new List<int>();
                for (int i = 0; i < emCampo.Count; i++)
                {
                    if (emCampo[i] == null)
                        retirar.Add(i);
                }

                for (int i = retirar.Count; i > 0; i--)
                    emCampo.RemoveAt(retirar[i - 1]);

                if ( emCampo.Count < 3)
                {
                    Spawn();
                }

                tempoDecorrido = 0;
            }

            
        }
    }
}