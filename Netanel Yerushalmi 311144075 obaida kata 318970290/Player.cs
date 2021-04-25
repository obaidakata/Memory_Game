using System;

namespace Netanel_Yerushalmi_311144075_obaida_kata_318970290
{
    public enum ePlayerType
    {
        Human,
        Computer
    }

    public class Player
    {
        private string m_Name;
        private int m_NumberOfSuccessfulFlips = 0;
        private ePlayerType m_PlayerType;

        public Player(string i_PlayerName, ePlayerType i_PlayerType)
        {
            m_Name = i_PlayerName;
            m_PlayerType = i_PlayerType;
        }

        public string Name
        {
            get
            {
                return m_Name;
            }

            set
            {
                m_Name = value;
            }
        }

        public bool IsCoumputer
        {
            get
            {
                return m_PlayerType == ePlayerType.Computer;
            }
        }

        public void AddPoint()
        {
            m_NumberOfSuccessfulFlips++;
        }

        public int Score
        {
            get
            {
                return m_NumberOfSuccessfulFlips;
            }
        }
    }
}
