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
            Wild = 3
        }
        public enum WildEnum
        {
            DrawFour = 0, 
            ChangeColor = 1
        }
        public enum ActionEnum
        {
            Reverse = 0, 
            Skip = 1, 
            DrawTwo = 2
        }
        public enum ColorEnum
        {
            Red = 0, 
            Yellow = 1, 
            Blue = 2, 
            Green = 3
        }
    }
}
