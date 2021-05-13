using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class QuantitativeItem
    {
        public static bool CanUseRecoveryItem(PetAtributes A)
        {
            if (A.PV.Corrente < A.PV.Maximo && A.PV.Corrente > 0)
                return true;
            else
                return false;
        }

        public static void RecuperaPV(PetAtributes meusAtributos, int tanto)
        {
            int contador = meusAtributos.PV.Corrente;
            int maximo = meusAtributos.PV.Maximo;

            if (contador + tanto < maximo)
                meusAtributos.PV.Corrente += tanto;
            else
                meusAtributos.PV.Corrente = meusAtributos.PV.Maximo;
        }
    }
}