using UnityEngine;
using System.Collections;

namespace CriaturesLegado
{
    public class Fraco : StatusTemporarioSimplesBase
    {
        public override void Start()
        {
            if (CDoAfetado != null)
                ColocaAParticulaEAdicionaEsseStatus(DoJogo.particulasFraco.ToString(), CDoAfetado.transform);
        }
    }
}