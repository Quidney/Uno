using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno.Class
{
    public class Card
    {
        public TypeEnum Type { get; set; }
        public WildEnum Wild { get; set; }
        public ActionEnum Action { get; set; }
        public ColorEnum Color { get; set; }
        public int Number { get; set; }

        //Normal Card Constructor
        public Card(TypeEnum type, ColorEnum color, int number) 
        {
            this.Type = type;
            this.Number = number;
            this.Color = color;
        }
        //Action Card Constructor
        public Card(TypeEnum type, ActionEnum action, ColorEnum color) 
        {
            this.Type = type;
            this.Action = action;
            this.Color = color;
        }
        //Wild Card Constructor
        public Card(TypeEnum type,  WildEnum wild)
        {

        }
        public enum TypeEnum
        {
            Color, Action, Wild
        }
        public enum WildEnum
        {
            WildDraw, WildColor
        }
        public enum ActionEnum
        {
            Reverse, Skip, DrawTwo
        }
     
        public enum ColorEnum
        {
            Red, Yellow, Blue, Green
        }
    }
}
