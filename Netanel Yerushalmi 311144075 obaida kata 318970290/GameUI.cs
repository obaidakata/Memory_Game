using System;
using System.Text;
using System.Threading;

namespace MemoryGame
{
    public class GameUI
    {   
        private const int k_MaxSize = 6;
        private const int k_MinSize = 4;
        private const int k_NumberOfSpaces = 5;
        private const int k_Human = 0;
        private const int k_Computer = 1;
        private const char k_BlankCell = ' ';
        private int m_Height;
        private int m_Width;
        private char[,] m_Cells;
        private string m_Separator;
        private string m_Upperindices;
        private char[] m_LeftSideindices;
        private StringBuilder m_LineToPrint;
        private string m_CurrentPlayerName;
        private bool m_PlayerWantToEndGame = false;
        
        public GameUI()
        {
            m_LineToPrint = new StringBuilder();
        }

        public string OutOfRangeMessage
        {
            get
            {
                return "out of range";
            }
        }

        public string InvalidMessage
        {
            get
            {
                return "invalid input";
            }
        }

        public string ChosenExposedCard
        {
            get
            {
                return "card already exposed";
            }
        }

        public string BadChoiceMessage
        {
            get
            {
               return "Sorry, cards are not matched";
            }
        }

        public string SuccessfulChoiceMessage
        {
            get
            {
                return string.Format("Very good, {0} gets another move", m_CurrentPlayerName);
            }
        }

        public void PrintError(string i_ErrorMessageToPrint)
        {
            Console.WriteLine("Choice illegal:  {0}", i_ErrorMessageToPrint);
            Thread.Sleep(1000);
        }

        public void PrintMessage(string i_MessageToPrint)
        {
            Console.WriteLine(i_MessageToPrint);
            Thread.Sleep(1000);
        }

        public string CurrentPlayerName
        {
            set
            {
                m_CurrentPlayerName = value;
            }
        }

        public int NumberOfPairs
        {
            get
            {
                return (m_Height * m_Width) / 2;
            }
        }

        public int Height
        {
            get
            {
                return m_Height;
            }
        }

        public int Width
        {
            get
            {
                return m_Width;
            }
        }

        private bool IsInputInRange(int i_Row, int i_Column)
        {
            bool rowInRange = i_Row >= 0 && i_Row <= m_Height;
            bool columnInRange = i_Column >= 0 && i_Column <= m_Width;

            return rowInRange && columnInRange;
        }

        public void SetBoardSize()
        {
            GetSizeFromUser();
            m_Cells = new char[m_Height, m_Width];
            string space = new string(' ', 2);
            m_Separator = space + new string('=', (m_Width * k_NumberOfSpaces) + 1);
            initializeBoard();
            setUpperIndices();
            setLeftSideindices();
            printBoard();
        }

        private void initializeBoard()
        {
            for (int i = 0; i < m_Height; i++)
            {
                for (int j = 0; j < m_Width; j++)
                {
                    m_Cells[i, j] = ' ';
                }
            }
        }

        private void setUpperIndices()
        {
            StringBuilder upper = new StringBuilder();
            string spaces = "    ";
            for (int i = 0; i < m_Width; i++)
            {
                upper.Append(spaces);
                upper.Append((char)('A' + i));
            }

            m_Upperindices = upper.ToString();
        }

        private void setLeftSideindices()
        {
            m_LeftSideindices = new char[m_Height];
            for (int i = 0; i < m_Height; i++)
            {
                m_LeftSideindices[i] = (char)i;
            }
        }

        public void GetSizeFromUser()
        {
            Console.Write("Please type the board height: ");
            bool isValid = int.TryParse(Console.ReadLine(), out m_Height);
            while (!isValid || !(m_Height <= k_MaxSize && m_Height >= k_MinSize))
            {
                Console.Write("Invalid height, try again: ");
                int.TryParse(Console.ReadLine(), out m_Height);
            }

            Console.Write("Please type the board width: ");
            isValid = int.TryParse(Console.ReadLine(), out m_Width);
            while (!isValid || !(m_Width <= k_MaxSize && m_Width >= k_MinSize))
            {
                Console.Write("Invalid width, try again: ");
                int.TryParse(Console.ReadLine(), out m_Width);
            }

            if ((m_Width * m_Height) % 2 != 0)
            {
                Console.WriteLine("Height or width should be even number, try again");
                GetSizeFromUser();
            }
        }
       
        public void SetCellWithValue(int i_Row, int i_Column, int i_CardID)
        {
            setCell(i_Row, i_Column, (char)(i_CardID + 'A'));
        }

        private void setCell(int i_Row, int i_Column, char i_Value)
        {
            m_Cells[i_Row, i_Column] = i_Value;
            printBoard();
        }

        public void SetCellEmpty(int i_Row, int i_Column)
        {
            setCell(i_Row, i_Column, k_BlankCell);
        }

        private void printBoard()
        {
            Console.Clear();
            Console.WriteLine("\t MEMORY GAME");
            Console.WriteLine(m_Upperindices);
            Console.WriteLine(m_Separator);
            for (int i = 0; i < m_Height; i++)
            {
                m_LineToPrint.Append(i + 1);
                m_LineToPrint.Append(" |");
                for (int j = 0; j < m_Width; j++)
                {
                    m_LineToPrint.Append(" ");
                    m_LineToPrint.Append(m_Cells[i, j]);
                    m_LineToPrint.Append("  |");
                }

                Console.WriteLine(m_LineToPrint);
                m_LineToPrint.Clear();
                Console.WriteLine(m_Separator);
            }
        }

        public bool IsSecondPlayerHuman()
        {
            bool isValidInput = false;
            int typeOfUserToInt;
            string typeOfUser;
            Console.Write("Do you want to play against player ({0}) or computer ({1})? ", k_Human, k_Computer);
            typeOfUser = Console.ReadLine();
            isValidInput = checkValidInputOfPlayerMode(typeOfUser, out typeOfUserToInt);
            while (!isValidInput)
            {
                Console.Write("Wrong input, please try again: ");
                typeOfUser = Console.ReadLine();
                isValidInput = checkValidInputOfPlayerMode(typeOfUser, out typeOfUserToInt);
            }

            return typeOfUserToInt == k_Human;
        }

        private bool checkValidInputOfPlayerMode(string i_TypeOfUser, out int io_InputToInt)
        {
            bool isInputValid = int.TryParse(i_TypeOfUser, out io_InputToInt);
            if (isInputValid)
            {
                isInputValid = io_InputToInt == k_Human || io_InputToInt == k_Computer;
            }

            return isInputValid;
        }

        public bool GetPlayerChoice(out Choice io_ChoiceFromPlayer, bool i_IsFirstChoice)
        {
            printBoard();
            string choice = i_IsFirstChoice ? "First" : "Second";

            showPlayerName();
            m_LineToPrint.Append(choice);
            m_LineToPrint.Append(" choice:");
            m_LineToPrint.Append(" please type row and column: ");
            Console.Write(m_LineToPrint);
            m_LineToPrint.Clear();
            string userChoice;
            do
            {
                userChoice = Console.ReadLine();
            } 
            while (string.IsNullOrEmpty(userChoice));

            bool isInputValid = false;
            io_ChoiceFromPlayer = null;
            if (userChoice.Equals("Q"))
            {
                Environment.Exit(0);
            }
            else
            {
                int row;
                char column;

                isInputValid = ValidChoiceInput(userChoice, out row, out column);
                if (isInputValid)
                {
                    io_ChoiceFromPlayer = new Choice(row, column);
                }
            }

            return isInputValid;
        }

        public bool ValidChoiceInput(string i_UserInput, out int io_Row, out char io_Column)
        {
            bool isInputvalid = false;
            io_Row = (int)char.GetNumericValue(i_UserInput[0]);
            io_Column = (i_UserInput.Length < 2) ? ' ' : i_UserInput[1];  
            if (io_Row == -1 || !char.IsLetter(io_Column))
            {
                PrintError(InvalidMessage);
            }
            else if (!IsInputInRange(io_Row, (int)(io_Column - 'A')))
            {
                PrintError(OutOfRangeMessage);
            }
            else
            {
                isInputvalid = true;
            }

            return isInputvalid;
        }

        public bool PlayerWantToEndGame
        {
            get
            {
                return m_PlayerWantToEndGame;
            }
        }

        public void PrintPlayerChoice(ref Choice io_PlayerChoice, bool i_IsFirstChoice)
        {
            showPlayerName();
            string choice = i_IsFirstChoice ? "First" : "Second";
            m_LineToPrint.AppendFormat("{0} choice:{1}", choice, Environment.NewLine);
            m_LineToPrint.AppendFormat("Row and column: {0}{1}", io_PlayerChoice.bRow, io_PlayerChoice.bColumn);
            Console.WriteLine(m_LineToPrint);
            m_LineToPrint.Clear();
        }

        private void showPlayerName()
        {
            Console.WriteLine("\t{0} turn", m_CurrentPlayerName);
        }

        public string GetPlayerNameFromUser()
        {
            Console.Write("Please type your name: ");
            return Console.ReadLine();
        }

        public bool AskPlayerForNewGame(string i_WinnerName, int i_WinnerScore)
        {
            Console.Clear();
            Console.WriteLine("\t\t GAME OVER");
            Console.WriteLine("The winner is {0}, with score {1}. CONGRATIOLASTIONS!", i_WinnerName, i_WinnerScore);
            Console.WriteLine("Do you want to start a new game?");
            Console.WriteLine("Type 0 if no, type 1 if yes");
            string userInput = Console.ReadLine();

            return userInput[0].Equals('1');
        }
    }
} 