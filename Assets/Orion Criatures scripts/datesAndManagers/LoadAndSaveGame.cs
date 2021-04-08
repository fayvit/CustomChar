using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LoadAndSaveGame
{
    public int indiceDoJogoAtualSelecionado = 0;

    public void ExcluirArquivo(string nomeArquivo)
    {
        if (File.Exists(Application.persistentDataPath + "/" + nomeArquivo))
        {
            File.Delete(Application.persistentDataPath + "/" + nomeArquivo);
        }
    }

    public static void SalvarArquivo(string nomeArquivo,object conteudo,string mainPath="")
    {
        if (string.IsNullOrEmpty(mainPath))
            mainPath = Application.persistentDataPath;

        BinaryFormatter bf = new BinaryFormatter();
        try
        {
            FileStream file = File.Create(mainPath + "/" + nomeArquivo);
            bf.Serialize(file, conteudo);
            file.Close();
        }
        catch (IOException e)
        {
            Debug.Log(e.StackTrace);
            Debug.LogWarning("Save falhou");
        }
    }

    public void Save(SaveDates paraSalvar)
    {
       // Debug.Log(indiceDoJogoAtualSelecionado);
        SalvarArquivo("criatures.ori" + indiceDoJogoAtualSelecionado,paraSalvar);
        
    }

    public SaveDates Load(int indice)
    {
        indiceDoJogoAtualSelecionado = indice;
        return CarregarArquivo<SaveDates>("criatures.ori" + indice);
    }

    public static T CarregarArquivo<T>(string nomeArquivo,string mainPath = "")
    {
        if (string.IsNullOrEmpty(mainPath))
            mainPath = Application.persistentDataPath;

        object retorno = null;
        if (File.Exists(mainPath+"/"+nomeArquivo))
        {
            Debug.Log(mainPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(mainPath+"/"+nomeArquivo, FileMode.Open);
            retorno = bf.Deserialize(file);
            file.Close();
        }

        return (T)retorno;
    }
}
