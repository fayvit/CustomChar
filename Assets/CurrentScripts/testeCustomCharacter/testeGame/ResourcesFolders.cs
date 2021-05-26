using UnityEngine;
using System.Collections;
using Criatures2021;

public class ResourcesFolders {
    public static Sprite GetMiniPet(PetName P)
    {
        return Resources.Load<Sprite>("miniCriatures/" + P.ToString());
    }

    public static Sprite GetMiniAttack(AttackNameId atk)
    {
        return Resources.Load<Sprite>("miniGolpes/" + atk.ToString());
    }

    public static Sprite GetMiniItem(NameIdItem nameItem)
    {
        return Resources.Load<Sprite>("miniItens/" + nameItem.ToString());
    }

    public static GameObject GetGeneralElements(GeneralElements G)
    {
        return Resources.Load<GameObject>("DamageView/"+G.ToString());
    }
}

public enum GeneralElements
{ 
    passouDeNivel
}
