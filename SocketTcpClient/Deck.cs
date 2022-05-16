﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTcpClient  
{
    class Deck : Card
    {
        const int NumOfCards = 52;//Номер всех карт
        private Card[] deck;//массив всех игральных карт

        public Deck()
        {
            deck = new Card[NumOfCards];
        }
        public Card[] getDeck { get { return deck; } }

        public void SetUpDeck()
        {
            int i = 0;
            foreach (SUIT s in Enum.GetValues(typeof(SUIT)))
            {
                foreach (VALUE v in Enum.GetValues(typeof(VALUE)))
                {
                    deck[i] = new Card { MySuit = s, MyValue = v };
                    ++i;
                }
            }
            ShuffleCards();
        }

        public void ShuffleCards()
        {
            Random rand = new Random();
            Card temp;

            for (int ShuffleTimes = 0; ShuffleTimes < 1000; ++ShuffleTimes)
            {
                for (int i = 0; i < NumOfCards; ++i)
                {
                    int SecondCardIndex = rand.Next(13);
                    temp = deck[i];
                    deck[i] = deck[SecondCardIndex];
                    deck[SecondCardIndex] = temp;
                }
            }
        }
    }
}
