using UnityEngine;
using System.Collections.Generic;

namespace FayvitCommandReader
{
    public interface IKeyDict
    {
        Dictionary<int, List<KeyCode>> DicKeys { get; }
        Dictionary<string, List<ValAxis>> DicAxis { get; }
        
    }

    public interface ICommandConverter
    {
        Dictionary<CommandConverterInt, int> DicCommandConverterInt { get; }
        Dictionary<CommandConverterString, string> DicCommandConverterString { get; }
    }

    public enum CommandConverterInt
    { 
        jump,
        attack,
        itemUse,
        criatureChange,
        run,
        lightAttack,
        dodge,
        heroToCriature,
        camFocus
    }

    public enum CommandConverterString
    { 
        moveH,
        moveV,
        camX,
        camY,
        attack,
        focusInTheEnemy
    }
}
/*
 * 0=>Jump
 * 1=>run
 * 2=>dodge
 * 3=>changeCriatures
 * 4=>item
 * 5=>lightAttack
 * 8=>heroToCriature
 * 9=>camFocus
 * 
 * triggerL=>
 * triggerR=>mainSpecialAttack
 */