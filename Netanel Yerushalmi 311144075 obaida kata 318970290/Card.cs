using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame
{
    public class Card
    {
        private static int s_IdentifierFactory = 0;
        private bool m_MatchFound;
        private int m_CardIdentifier;

        public Card()
        {
            m_MatchFound = false;
            m_CardIdentifier = s_IdentifierFactory;
            s_IdentifierFactory++;
        }

        public static bool operator ==(Card card1, Card card2)
        {
            return card1.m_CardIdentifier == card2.m_CardIdentifier;
        }

        public static bool operator !=(Card card1, Card card2)
        {
            return !(card1.m_CardIdentifier == card2.m_CardIdentifier);
        }

        public int CardIdentifier
        {
            get
            {
                return m_CardIdentifier;
            }
        }

        public bool IsMatchFound
        {
            get
            {
                return m_MatchFound;
            }

            set
            {
                m_MatchFound = value;
            }
        }

        public override bool Equals(object obj)
        {
            return this == ((Card)obj);
        }
    }
}
