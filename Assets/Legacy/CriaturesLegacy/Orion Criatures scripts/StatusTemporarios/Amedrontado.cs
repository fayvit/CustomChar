using UnityEngine;
using System.Collections;

namespace CriaturesLegado
{
    public class Amedrontado : StatusTemporarioSimplesBase
    {
        public override void Start()
        {
            if (CDoAfetado != null)
                ColocaAParticulaEAdicionaEsseStatus(DoJogo.particulasAmedrontado.ToString(), CDoAfetado.transform);

        }
    }
}