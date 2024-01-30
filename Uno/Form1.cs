using System.Windows.Forms;
using Uno.Class;
using Uno.Classes;

namespace Uno
{
    public partial class Form1 : Form
    {
        Deck deck;
        public Form1()
        {
            InitializeComponent();

            InitMethod();
        }

        void InitMethod()
        {
            deck = new Deck();

            DebugCardChecker();

            deck.Shuffle();

            DebugCardChecker();
        }

        void DebugCardChecker()
        {
            string messageFromDeck = string.Empty;

            foreach (Card card in deck.cardsDeckList)
            {

                if (card.Color != 0)
                {
                    messageFromDeck += $"{card.Color} ";
                }
                messageFromDeck += $"{card.Type} ";

                if (card.Number != -1)
                {
                    messageFromDeck += $"({card.Number}) + ";
                }
                if (card.Action != 0)
                {
                    messageFromDeck += $"{card.Action} + ";
                }
                if (card.Wild != 0)
                {
                    messageFromDeck += $"{card.Wild} + ";
                }
            }
            MessageBox.Show(messageFromDeck);
        }
    }
}
