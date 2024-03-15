using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Uno.Classes;

namespace Uno.Class
{
    public class Deck
    {
        public List<Card> cardsDeckList;
        public List<Card> playingDeck;
        public Dictionary<int, Card> idToCard;

        public Deck()
        {

        }

        public void InitDeck()
        {
            cardsDeckList = new List<Card>();
            idToCard = new Dictionary<int, Card>();

            Card newCard;
            int id = 1;

            //Repeat 3 times, (0, 1, 2) for each Type
            for (int i = 0; i < 3; i++)
            {
                //Type = Normal
                if (i == 0)
                {
                    //For each color
                    for (int j = 1; j < 5; j++)
                    {
                        //For each number
                        for (int k = 0; k < 10; k++)
                        {
                            newCard = new Card((Card.TypeEnum)i, (Card.ColorEnum)j, k);
                            newCard.ID = id;
                            cardsDeckList.Add(newCard);
                            idToCard.Add(id++, newCard);

                            if (k != 0) //If K (number) isn't 0 (there are two of each number except for 0 in UNO)
                            {
                                newCard = new Card((Card.TypeEnum)i, (Card.ColorEnum)j, k);
                                newCard.ID = id;
                                cardsDeckList.Add(newCard);
                                idToCard.Add(id++, newCard);
                            }
                        }
                    }
                }
                //Type = Action
                else if (i == 1)
                {
                    //For each color
                    for (int j = 1; j < 5; j++)
                    {
                        //For each action type
                        for (int k = 1; k < 4; k++)
                        {
                            //Add two times
                            for (int l = 0; l < 2; l++)
                            {
                                newCard = new Card((Card.TypeEnum)i, (Card.ColorEnum)j, (Card.ActionEnum)k);
                                newCard.ID = id;
                                cardsDeckList.Add(newCard);
                                idToCard.Add(id++, newCard);
                            }
                        }

                    }
                }
                //Type = Wild
                else
                {
                    //Iterate per wild card Type (2 times)
                    for (int j = 1; j < 3; j++)
                    {
                        //Add 4 wild cards per wild card type
                        for (int k = 0; k < 4; k++)
                        {
                            newCard = new Card((Card.TypeEnum)i, (Card.WildEnum)j);
                            newCard.ID = id;
                            cardsDeckList.Add(newCard);
                            idToCard.Add(id++, newCard);
                        }
                    }
                }
            }

            /*  -- Enable to Debug Deck --
             
            string allCards = string.Empty;

            foreach (Card card in cardsDeckList)
            {
                allCards += card.ToColor().ToString() + " " + card.ToString() + " " + card.ID + "\n";
            }

            MessageBox.Show(allCards, "Card List");

            */

            playingDeck = new List<Card>(cardsDeckList);
        }

        Random random = new Random();
        public async Task Shuffle()
        {
            //Fisher-Yates shuffle algorithm
            for (int i = 0; i < 4; i++)
            {
                int deckCount = playingDeck.Count;

                while (deckCount > 1)
                {
                    deckCount--;

                    int randomIndex = random.Next(deckCount + 1);

                    Card card = playingDeck[randomIndex];
                    playingDeck[randomIndex] = playingDeck[deckCount];
                    playingDeck[deckCount] = card;
                    await Task.Delay(0);
                }
            }
        }

        public Card DrawCard()
        {
            Card drawnCard = playingDeck.LastOrDefault();
            playingDeck.Remove(drawnCard);
            return drawnCard;
        }
    }
}
