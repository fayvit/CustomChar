using UnityEngine;
using System.Collections.Generic;

namespace FayvitCommandReader
{
    public class CommandConverterBase : ICommandConverter
    {
        public Dictionary<CommandConverterInt, int> dicCommandConverterInt = new Dictionary<CommandConverterInt, int>()
        {
            { CommandConverterInt.jump,0},
            { CommandConverterInt.dodge,1},
            { CommandConverterInt.run,2},
            { CommandConverterInt.criatureChange,3},
            { CommandConverterInt.itemUse,4},
            { CommandConverterInt.lightAttack,5},
            { CommandConverterInt.heroToCriature,8},
            { CommandConverterInt.camFocus,9},
            { CommandConverterInt.confirmButton,1},
            { CommandConverterInt.returnButton,0},
            { CommandConverterInt.humanAction,1},
        };

        public Dictionary<CommandConverterString, string> dicCommandConverterString = new Dictionary<CommandConverterString, string>()
        {
            { CommandConverterString.camX,"Xcam"},
            { CommandConverterString.camY,"Ycam"},
            { CommandConverterString.moveH,"horizontal"},
            { CommandConverterString.moveV,"vertical"},
            { CommandConverterString.attack,"triggerR"},
            { CommandConverterString.focusInTheEnemy,"triggerL"},
            { CommandConverterString.selectAttack_selectCriature,"VDpad"},
            { CommandConverterString.itemChange,"HDpad"}
        };

        public Dictionary<CommandConverterString, string> DicCommandConverterString => dicCommandConverterString;

        public Dictionary<CommandConverterInt, int> DicCommandConverterInt => dicCommandConverterInt;
    }
}