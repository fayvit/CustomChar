using UnityEngine;
using System.Collections;

namespace CriaturesLegado
{
    [System.Serializable]
    public class ItemModificadorDeAtributoIntrinsico : ItemModificadorDeAtributoBase
    {

        public ItemModificadorDeAtributoIntrinsico(ContainerDeCaracteristicasDeItem cont) : base(cont) { }

        protected AtributoInstrinsico ContaDeSubida(AtributoInstrinsico A)
        {
            if ((A.Minimo + A.Maximo) / 2 + 1 > 5)
            {
                A = new AtributoInstrinsico(
                    A.Corrente + 1,
                    A.Taxa,
                    A.Maximo + 1,
                    A.Minimo + 1,
                    A.ModCorrente,
                    A.ModMaximo
                    );

            }
            else
            {
                Debug.Log("<5");
                A = new AtributoInstrinsico(
                    A.Corrente + 1,
                    A.Taxa,
                    A.Maximo + 2,
                    1,
                    A.ModCorrente,
                    A.ModMaximo
                    );

            }
            return A;
        }
    }
}