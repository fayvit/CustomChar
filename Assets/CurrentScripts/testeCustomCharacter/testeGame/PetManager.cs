using CriaturesLegado;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{

    [SerializeField] private LocalState state = LocalState.following;
    [SerializeField] private Transform tDono;
    [SerializeField] private CriatureBase meuCriatureBase;
    [SerializeField] private MovimentacaoBasica mov;

    private enum LocalState
    { 
        following
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
