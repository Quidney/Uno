using System.Drawing;

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
        public Card(TypeEnum type, WildEnum wild)
        {
            this.Type = type;
            this.Wild = wild;
        }

        #region Enums
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
        #endregion

        public override string ToString()
        {
            string color = Color != Card.ColorEnum.None ? $"{Color}" : string.Empty;
            string type = $"{Type}";
            string wild = Wild != Card.WildEnum.None ? $"{Wild}" : string.Empty;
            string action = Action != Card.ActionEnum.None ? $"{Action}" : string.Empty;
            string number = Number != -1 ? $"{Number}" : string.Empty;

            return $"{color} {type} {wild}{action}{number}";
        }

        public System.Drawing.Color ToColor()
        {
            switch (Color)
            {
                case ColorEnum.Red:
                    return System.Drawing.Color.OrangeRed;
                case ColorEnum.Green:
                    return System.Drawing.Color.LimeGreen;
                case ColorEnum.Yellow: 
                    return System.Drawing.Color.Lime;
                case ColorEnum.Blue:
                    return System.Drawing.Color.CornflowerBlue;
                default:
                    return SystemColors.Control;

            }
        }
    }
}
