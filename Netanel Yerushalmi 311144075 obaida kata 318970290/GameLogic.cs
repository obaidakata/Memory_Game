using System;
using System.Dynamic;
using System.Collections.Generic;

namespace MemoryGame
{
    public class GameLogic
    {
        private Card[] m_CardsDataBase;
        private int m_AmountOfExposedCards;
        private List<LinkedList<Choice>> m_BadChoicecList;

        public GameLogic(int i_NumOfCards)
        {
            m_AmountOfExposedCards = 0;
            m_CardsDataBase = new Card[i_NumOfCards];
            m_BadChoicecList = new List<LinkedList<Choice>>(i_NumOfCards);
            for(int i = 0; i < i_NumOfCards; i++)
            {
                m_BadChoicecList.Add(new LinkedList<Choice>());
            }

            SetCards();
        }

        public void SetBadChoiceToTable(int i_FirstCardIdentifier, Choice i_SecondUserChoice)
        {
            m_BadChoicecList[i_FirstCardIdentifier].AddLast(i_SecondUserChoice);
        }

        public bool SearchChoiceInList(int i_FirstCardIdentifier, Choice i_SecondUserChoice)
        {
            foreach (Choice iterator in m_BadChoicecList[i_FirstCardIdentifier])
            {
                if(iterator == i_SecondUserChoice)
                {
                    return true;
                }
            }

            return false;
        }

        public void SetCards()
        {
            for (int i = 0; i < m_CardsDataBase.Length; i++)
            {
                m_CardsDataBase[i] = new Card();
            }
        }

        public bool CheckCardsMatch(int i_FirstCardIdentifier, int i_SecondCardIdentifier)
        {
            bool areCardsEquals = i_FirstCardIdentifier == i_SecondCardIdentifier;
            if (areCardsEquals)
            {
                m_AmountOfExposedCards++;
                m_CardsDataBase[i_FirstCardIdentifier].IsMatchFound = true;
            }

            return areCardsEquals;
        }

        public bool IsCardAlreadyMatch(int i_CardIdentifier)
        {
            return m_CardsDataBase[i_CardIdentifier].IsMatchFound;
        }

        public bool IsGameEnd
        {
            get
            {
                return m_CardsDataBase.Length == m_AmountOfExposedCards;
            }
        }
    }  
}
