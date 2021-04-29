using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame
{
    public class Choice
    {
        private int m_Row;
        private char m_Column;

        public static bool operator ==(Choice choice1, Choice choice2)
        {
            return choice1.m_Column == choice2.m_Column && choice1.Row == choice2.Row;
        }

        public static bool operator !=(Choice choice1, Choice choice2)
        {
            return !(choice1.m_Column == choice2.m_Column && choice1.Row == choice2.Row);
        }

        public Choice(int i_Row, char i_Column)
        {
            m_Row = i_Row;
            m_Column = i_Column;
        }

        public int Row
        {
            get
            {
                return m_Row - 1;
            }
        }

        public int Column
        {
            get
            {
                if(char.IsLetter(m_Column))
                {
                    return(int)(m_Column - 'A' );
                }
                else
                {
                    return -1;
                }
            }
        }

        public int bRow
        {
            get
            {
                return m_Row;
            }
        }

        public char bColumn
        {
            get 
            {
                return m_Column;
            }
        }

        public override bool Equals(object obj)
        {
            return this == ((Choice)obj);
        }
    }
}
