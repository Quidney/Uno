using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno.Classes
{
    public class Card
    {
        public TypeEnum Type { get; set; }
        public WildEnum Wild { get; set; }
        public ActionEnum Action { get; set; }
        public ColorEnum Color { get; set; }
        public int Number { get; set; } = -1;

        //Normal Card Constructor
        public Card(TypeEnum type, ColorEnum color, int number)
        {
            this.Type = type;
            this.Color = color;
            this.Number = number;
        }
        //Action Card Constructor
        public Card(TypeEnum type, ColorEnum color, ActionEnum action) 
        {
            this.Type = type;
            this.Color = color;
            this.Action = action;
        }
        //Wild Card Constructor
        public Card(TypeEnum type,  WildEnum wild)
        {
            this.Type = type;
            this.Wild = wild;
        }

        public enum TypeEnum
        {
            Number = 0, 
            Action = 1, 
            Wild = 2
        }
        public enum WildEnum
        {
            None = 0,
            DrawFour = 1, 
            ChangeColor = 2
        }
        public enum ActionEnum
        {
            None = 0,
            Reverse = 1, 
            Skip = 2, 
            DrawTwo = 3
        }
        public enum ColorEnum
        {
            None = 0,
            Red = 1, 
            Yellow = 2, 
            Blue = 3, 
            Green = 4
        }
    }
}
