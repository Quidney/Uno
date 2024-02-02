using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uno.Classes
{
    public class CustomControls{}

    public class CustomLabel : Label
    {
        public Card AssignedCard { get; set; }
        public CustomLabel() : base() 
        {
            DoubleBuffered = true;
        }
    }
}
