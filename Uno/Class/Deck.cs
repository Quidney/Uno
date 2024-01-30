using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uno.Class;

namespace Uno.Class
{
    public class Deck
    {
        List<Card> cardsDeckList;

        public Deck() 
        {
            cardsDeckList = new List<Card>();

            //Repeat 4 times, (0 - 3) for each color
            for (int i = 0; i < 4; i++)
            {
                //Repeat 10 times, (0 - 9) for each number
                for (int j = 0; j < 10; j++)
                {
                    cardsDeckList.Add(new Card(Card.TypeEnum.Number, (Card.ColorEnum)i, j));
                    if (j != 0)
                    cardsDeckList.Add(new Card(Card.TypeEnum.Number, (Card.ColorEnum)i, j));
                }
            }



        }
    }
}
