﻿using System;
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
        public bool IsHost { get; set; }
        public List<Card> playerInventory;

        public Player(string id, string name)
        {
            this.Id = id;
            this.Name = name;
            playerInventory = new List<Card>();
        }

        public void AddCardToInventory(Card card)
        {
            playerInventory.Add(card);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}