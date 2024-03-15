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
        public Dictionary<int, Image> redNumbers;
        public Dictionary<int, Image> yellowNumbers;
        public Dictionary<int, Image> greenNumbers;
        public Dictionary<int, Image> blueNumbers;
        public Dictionary<int, Image> redActions;
        public Dictionary<int, Image> yellowActions;
        public Dictionary<int, Image> greenActions;
        public Dictionary<int, Image> blueActions;
        public Dictionary<int, Image> imageWild;


        public Deck()
        {

        }

        public void InitDeck()
        {
            cardsDeckList = new List<Card>();
            idToCard = new Dictionary<int, Card>();

            redNumbers = new Dictionary<int, Image>();
            yellowNumbers = new Dictionary<int, Image>();
            blueNumbers = new Dictionary<int, Image>();
            greenNumbers = new Dictionary<int, Image>();

            blueActions = new Dictionary<int, Image>();
            redActions = new Dictionary<int, Image>();
            yellowActions = new Dictionary<int, Image>();
            greenActions = new Dictionary<int, Image>();

            imageWild = new Dictionary<int, Image>();

            Dictionary<int, Image>[] numbers = new Dictionary<int, Image>[4];
            Dictionary<int, Image>[] actions = new Dictionary<int, Image>[4];

            imageWild.Add(1, Properties.Resources.black_plus_four);
            imageWild.Add(2, Properties.Resources.black_pick_color);

            redNumbers.Add(0, Properties.Resources.red_zero);
            redNumbers.Add(1, Properties.Resources.red_one);
            redNumbers.Add(2, Properties.Resources.red_two);
            redNumbers.Add(3, Properties.Resources.red_three);
            redNumbers.Add(4, Properties.Resources.red_four);
            redNumbers.Add(5, Properties.Resources.red_five);
            redNumbers.Add(6, Properties.Resources.red_six);
            redNumbers.Add(7, Properties.Resources.red_seven);
            redNumbers.Add(8, Properties.Resources.red_eight);
            redNumbers.Add(9, Properties.Resources.red_nine);

            yellowNumbers.Add(0, Properties.Resources.yellow_zero);
            yellowNumbers.Add(1, Properties.Resources.yellow_one);
            yellowNumbers.Add(2, Properties.Resources.yellow_two);
            yellowNumbers.Add(3, Properties.Resources.yellow_three);
            yellowNumbers.Add(4, Properties.Resources.yellow_four);
            yellowNumbers.Add(5, Properties.Resources.yellow_five);
            yellowNumbers.Add(6, Properties.Resources.yellow_six);
            yellowNumbers.Add(7, Properties.Resources.yellow_seven);
            yellowNumbers.Add(8, Properties.Resources.yellow_eight);
            yellowNumbers.Add(9, Properties.Resources.yellow_nine);

            greenNumbers.Add(0, Properties.Resources.green_zero);
            greenNumbers.Add(1, Properties.Resources.green_one);
            greenNumbers.Add(2, Properties.Resources.green_two);
            greenNumbers.Add(3, Properties.Resources.green_three);
            greenNumbers.Add(4, Properties.Resources.green_four);
            greenNumbers.Add(5, Properties.Resources.green_five);
            greenNumbers.Add(6, Properties.Resources.green_six);
            greenNumbers.Add(7, Properties.Resources.green_seven);
            greenNumbers.Add(8, Properties.Resources.green_eight);
            greenNumbers.Add(9, Properties.Resources.green_nine);

            blueNumbers.Add(0, Properties.Resources.blue_zero);
            blueNumbers.Add(1, Properties.Resources.blue_one);
            blueNumbers.Add(2, Properties.Resources.blue_two);
            blueNumbers.Add(3, Properties.Resources.blue_three);
            blueNumbers.Add(4, Properties.Resources.blue_four);
            blueNumbers.Add(5, Properties.Resources.blue_five);
            blueNumbers.Add(6, Properties.Resources.blue_six);
            blueNumbers.Add(7, Properties.Resources.blue_seven);
            blueNumbers.Add(8, Properties.Resources.blue_eight);
            blueNumbers.Add(9, Properties.Resources.blue_nine);

            redActions.Add(1, Properties.Resources.red_reverse);
            redActions.Add(2, Properties.Resources.red_skip);
            redActions.Add(3, Properties.Resources.red_plus_two);

            yellowActions.Add(1, Properties.Resources.yellow_reverse);
            yellowActions.Add(2, Properties.Resources.yellow_skip);
            yellowActions.Add(3, Properties.Resources.yellow_plus_two);

            greenActions.Add(1, Properties.Resources.green_reverse);
            greenActions.Add(2, Properties.Resources.green_skip);
            greenActions.Add(3, Properties.Resources.green_plus_two);

            blueActions.Add(1, Properties.Resources.blue_reverse);
            blueActions.Add(2, Properties.Resources.blue_skip);
            blueActions.Add(3, Properties.Resources.blue_plus_two);

            numbers[0] = redNumbers;
            numbers[3] = greenNumbers;
            numbers[2] = blueNumbers;
            numbers[1] = yellowNumbers;

            actions[0] = redActions;
            actions[3] = greenActions;
            actions[2] = blueActions;
            actions[1] = yellowActions;

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
                            numbers[j - 1].TryGetValue(k, out Image image);
                            newCard.Image = image;
                            cardsDeckList.Add(newCard);
                            idToCard.Add(id++, newCard);

                            if (k != 0) //If K (number) isn't 0 (there are two of each number except for 0 in UNO)
                            {
                                newCard = new Card((Card.TypeEnum)i, (Card.ColorEnum)j, k);
                                newCard.ID = id;
                                numbers[j - 1].TryGetValue(k, out Image image2);
                                newCard.Image = image2;
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
                                actions[j - 1].TryGetValue(k, out Image image);
                                newCard.Image = image;
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
                            imageWild.TryGetValue(j, out Image image);
                            newCard.Image = image;
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
