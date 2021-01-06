using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoXmlFile
{
    public class Data
    {
        public class Left
        {
            public string MenuButton
            {
                get { return MenuButton; }
                set { MenuButton = value; }
            }

            public string SystemButton
            {
                get { return SystemButton; }
                set { SystemButton = value; }
            }

            public string TriggerPressed
            {
                get { return TriggerPressed; }
                set { TriggerPressed = value; }
            }

            public string GripButton
            {
                get { return GripButton; }
                set { GripButton = value; }
            }

            public string TouchpadClicked
            {
                get { return TouchpadClicked; }
                set { TouchpadClicked = value; }
            }

            public string TouchpadTouched
            {
                get { return TouchpadTouched; }
                set { TouchpadTouched = value; }
            }
        }

        
    }
}
