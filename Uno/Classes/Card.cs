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
        public int ID { get; set; }


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
            Black = 0,
            Red = 1,
            Yellow = 2,
            Blue = 3,
            Green = 4
        }
        #endregion

        public override string ToString()
        {
            switch (Type)
            {
                case TypeEnum.Number:
                    return Number.ToString();
                case TypeEnum.Action:
                    switch (Action)
                    {
                        case ActionEnum.DrawTwo:
                            return $"+2";
                        case ActionEnum.Reverse:
                            return "Reverse";
                        case ActionEnum.Skip:
                            return "Skip";
                    }
                    break;
                case TypeEnum.Wild:
                    switch (Wild)
                    {
                        case WildEnum.DrawFour:
                            return "+4";
                        case WildEnum.ChangeColor:
                            return "Change Color";
                    }
                    break;
            }
            return string.Empty;
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
                    return System.Drawing.Color.Yellow;
                case ColorEnum.Blue:
                    return System.Drawing.Color.CornflowerBlue;
                default:
                    return System.Drawing.Color.Black;

            }
        }
    }
}
