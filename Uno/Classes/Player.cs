using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno.Classes
{
    public class Player
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public List<Card> playerInventory;

        public bool IsHost { get; set; }

        public Player(string id, string name)
        {
            this.Id = id;
            this.Name = name;
            playerInventory = new List<Card>();
        }

        public void DrawCard(Card card)
        {
            playerInventory.Add(card);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}