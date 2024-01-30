using System.Linq;
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
            deck.Shuffle();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Label[] labels = { label1, label2, label3, label4, label5, label6, label7};
            Card[] cards = new Card[7];

            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = deck.cardsDeckList.LastOrDefault();
                deck.cardsDeckList.Remove(cards[i]);
                Card card = cards[i];


                string color = card.Color != Card.ColorEnum.None ? $"{card.Color}" : string.Empty;
                string wild = card.Wild != Card.WildEnum.None ? $"{card.Wild}" : string.Empty;
                string action = card.Action != Card.ActionEnum.None ? $"{card.Action}" : string.Empty;
                string number = card.Number != -1 ? $"{card.Number}" : string.Empty;

                labels[i].Text = $"{color} {card.Type} {wild} {action} {number}";
            }
        }
    }
}
