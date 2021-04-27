using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DbColors
{
    public static Color[] ColorsByDb(RegistroDeCores sdb)
    {
        return sdb switch
        {
            RegistroDeCores.skin => ColorDbManager.LoadColors("CurrentScripts/testeCustomCharacter/DateColors/skinColors.crs").ToArray(),
            _ => ColorDbManager.LoadColors("CurrentScripts/testeCustomCharacter/DateColors/mainColors.crs").ToArray()
        };
    }

    public static int GetApproximateColorIndex(Color[] cores, Color cor)
    {
        int retorno = 0;
        Vector3 Vb = new Vector3(cor.r, cor.g, cor.b);
        for (int i = 0; i < cores.Length; i++)
        {
            Color c = cores[retorno];
            Vector3 Vr = new Vector3(c.r, c.g, c.b);
            c = cores[i];
            Vector3 Vt = new Vector3(c.r, c.g, c.b);
            if (Vector3.Distance(Vb, Vt) < Vector3.Distance(Vb, Vr))
            {
                retorno = i;
            }
        }

        return retorno;
    }

    
}