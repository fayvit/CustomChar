using UnityEngine;
using System.Collections.Generic;

namespace FayvitCommandReader
{
    public class CommandConverterBase : ICommandConverter
    {
        public Dictionary<CommandConverterInt, List<int>> dicCommandConverterInt = new Dictionary<CommandConverterInt, List<int>>()
        {
            { CommandConverterInt.jump,new List<int>{0} },
            { CommandConverterInt.dodge,new List<int>{1} },
            { CommandConverterInt.run,new List<int>{2} },
            { CommandConverterInt.criatureChange,new List<int>{3} },
            { CommandConverterInt.itemUse,new List<int>{4} },
            { CommandConverterInt.lightAttack,new List<int>{5} },
            { CommandConverterInt.heroToCriature,new List<int>{8} },
            { CommandConverterInt.camFocus,new List<int>{9} },
            { CommandConverterInt.confirmButton,new List<int>{1,4} },
            { CommandConverterInt.returnButton,new List<int>{8,6} },
            { CommandConverterInt.humanAction,new List<int>{1} },
            { CommandConverterInt.updateMenu,new List<int>{6} },
        };

        public Dictionary<CommandConverterString, List<string>> dicCommandConverterString 
            = new Dictionary<CommandConverterString, List<string>>()
        {
            { CommandConverterString.camX,new List<string>{"Xcam"} },
            { CommandConverterString.camY,new List<string>{"Ycam"} },
            { CommandConverterString.moveH,new List<string>{"horizontal"} },
            { CommandConverterString.moveV,new List<string>{"vertical"} },
            { CommandConverterString.attack,new List<string>{"triggerR"} },
            { CommandConverterString.focusInTheEnemy,new List<string>{"triggerL"} },
            { CommandConverterString.selectAttack_selectCriature,new List<string>{"VDpad"} },
            { CommandConverterString.itemChange,new List<string>{"HDpad"} },
            { CommandConverterString.alternativeV_Change,new List<string>{"VDpad"} },
            { CommandConverterString.alternativeH_Change,new List<string>{"HDpad"} }
        };

        public Dictionary<CommandConverterString, List<string>> DicCommandConverterString => dicCommandConverterString;

        public Dictionary<CommandConverterInt,List<int>> DicCommandConverterInt => dicCommandConverterInt;
    }
}