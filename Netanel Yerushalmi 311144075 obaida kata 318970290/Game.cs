using System;
using System.Threading;

namespace MemoryGame
{
    public class Game
    {
        private const string k_ComputerName = "Computer";
        private Random m_RandomValue;
        private GameUI m_GameUI;
        private GameLogic m_GameLogic;
        private int m_NumberOfPairs;
        private int[,] m_CardsPairBoard;
        private Player m_Player1;
        private Player m_Player2;
        private Player m_CurrentPlayer;
        private Choice m_FirstCardChoice;
        private Choice m_SecondCardChoice;
        private int m_FirstCardIdentifier;
        private int m_SecondCardIdentifier;

        public Game()
        {
            initializeGame();
        }

        private void initializeGame()
        {
            m_RandomValue = new Random();
            m_GameUI = new GameUI();
            setPlayers();
            initializeBoard();
        }

        private void initializeBoard()
        {
            m_GameUI.SetBoardSize();
            m_NumberOfPairs = m_GameUI.NumberOfPairs;
            m_GameLogic = new GameLogic(m_NumberOfPairs);
            m_CardsPairBoard = new int[m_GameUI.Height, m_GameUI.Width];
            setCardsInfo();
        }

        public void Run()
        {
            m_CurrentPlayer = m_Player1;
            m_GameUI.CurrentPlayerName = m_CurrentPlayer.Name;
            bool successfulCoice = false;
            while (!m_GameLogic.IsGameEnd)
            {
                successfulCoice = playSingleStep();
                if (m_GameUI.PlayerWantToEndGame)
                {
                    break;
                }

                if (!successfulCoice)
                {
                    m_GameUI.PrintMessage(m_GameUI.BadChoiceMessage);
                    m_GameUI.SetCellEmpty(m_FirstCardChoice.Row, m_FirstCardChoice.Column);
                    m_GameUI.SetCellEmpty(m_SecondCardChoice.Row, m_SecondCardChoice.Column);
                    switchCurrentPlayer();
                }
                else
                {
                    m_GameUI.PrintMessage(m_GameUI.SuccessfulChoiceMessage);
                    m_CurrentPlayer.AddPoint();
                }
            }

            Player winner = (m_Player1.Score > m_Player2.Score) ? m_Player1 : m_Player2;
            if (m_GameUI.AskPlayerForNewGame(winner.Name, winner.Score))
            {
                initializeBoard();
                Run();
            }
            else
            {
                Console.WriteLine("Bye bye!");
                Environment.Exit(0);
            }
        }

        private bool playSingleStep()
        {
            if (m_CurrentPlayer.IsCoumputer)
            {
                computerMove();
            }
            else
            {
                humanMove();
            }

            bool areCardsMatch = m_GameLogic.CheckCardsMatch(m_FirstCardIdentifier, m_SecondCardIdentifier);
            if (!areCardsMatch)
            {
                m_GameLogic.SetBadChoiceToTable(m_FirstCardIdentifier, m_SecondCardChoice);
            }

            return areCardsMatch;
        }

        private bool humanMove()
        {
            bool isCardAlreadyMatch = true;
            bool isValidInput = true;
            bool isLegalChoice = false;
            do
            {
                isValidInput = m_GameUI.GetPlayerChoice(out m_FirstCardChoice, true);

                if (isValidInput)
                {
                    m_FirstCardIdentifier = getCardIdentifier(m_FirstCardChoice);
                    isCardAlreadyMatch = m_GameLogic.IsCardAlreadyMatch(m_FirstCardIdentifier);
                    if (isCardAlreadyMatch)
                    {
                        m_GameUI.PrintError(m_GameUI.ChosenExposedCard);
                        Thread.Sleep(1000);
                    }
                }

                isLegalChoice = !(!isValidInput || isCardAlreadyMatch);
            } 
            while (!isLegalChoice);

            m_GameUI.SetCellWithValue(m_FirstCardChoice.Row, m_FirstCardChoice.Column, m_FirstCardIdentifier);
            bool areChoicesEqual = false;
            do
            {
                isValidInput = m_GameUI.GetPlayerChoice(out m_SecondCardChoice, false);
                if (isValidInput)
                {
                    m_SecondCardIdentifier = getCardIdentifier(m_SecondCardChoice);
                    isCardAlreadyMatch = m_GameLogic.IsCardAlreadyMatch(m_SecondCardIdentifier);
                    areChoicesEqual = m_FirstCardChoice == m_SecondCardChoice;
                    if(areChoicesEqual || isCardAlreadyMatch)
                    {
                        m_GameUI.PrintError(m_GameUI.ChosenExposedCard);
                        Thread.Sleep(1000);
                    }
                }

                isLegalChoice = !(!isValidInput || isCardAlreadyMatch || areChoicesEqual);
            } 
            while (!isLegalChoice);

            m_GameUI.SetCellWithValue(m_SecondCardChoice.Row, m_SecondCardChoice.Column, m_SecondCardIdentifier);
            
            return true;
        }

        private void computerMove()
        {
            bool isCardAlreadyMatch;
            do
            {
                computerChoice(out m_FirstCardChoice);
                m_FirstCardIdentifier = getCardIdentifier(m_FirstCardChoice);
                isCardAlreadyMatch = m_GameLogic.IsCardAlreadyMatch(m_FirstCardIdentifier);
            } 
            while (isCardAlreadyMatch);

            m_GameUI.PrintPlayerChoice(ref m_FirstCardChoice, true);
            Thread.Sleep(2000);
            m_GameUI.SetCellWithValue(m_FirstCardChoice.Row, m_FirstCardChoice.Column, m_FirstCardIdentifier);
            Thread.Sleep(1000);
            bool isLegalChoice;
            bool isSecondCardChosenBefore;
            bool areChoicesEqual;
            do
            {
                computerChoice(out m_SecondCardChoice);
                m_SecondCardIdentifier = getCardIdentifier(m_SecondCardChoice);
                isSecondCardChosenBefore = m_GameLogic.SearchChoiceInList
                                    (m_FirstCardIdentifier, m_SecondCardChoice);
                                                           
                isCardAlreadyMatch = m_GameLogic.IsCardAlreadyMatch(m_SecondCardIdentifier);
                areChoicesEqual = m_FirstCardChoice == m_SecondCardChoice;
                isLegalChoice = !(isSecondCardChosenBefore || isCardAlreadyMatch || areChoicesEqual);
            } 
            while (!isLegalChoice);

            m_GameUI.PrintPlayerChoice(ref m_SecondCardChoice, false);
            Thread.Sleep(2000);
            m_GameUI.SetCellWithValue(m_SecondCardChoice.Row, m_SecondCardChoice.Column, m_SecondCardIdentifier);
        }

        private void computerChoice(out Choice io_ChoiceFromComputer)
        {
            int randomRow = m_RandomValue.Next(1, m_GameUI.Height + 1);
            int randomColumn = m_RandomValue.Next(0, m_GameUI.Width);
            char columnToChar = (char)('A' + randomColumn);
            io_ChoiceFromComputer = new Choice(randomRow, columnToChar);
        }

        private void switchCurrentPlayer()
        {
            m_CurrentPlayer = (m_CurrentPlayer == m_Player2) ? m_Player1 : m_Player2;
            m_GameUI.CurrentPlayerName = m_CurrentPlayer.Name;
        }

        private int getCardIdentifier(Choice i_ChosenCrad)
        {
            int rowFromUser = i_ChosenCrad.Row;
            int columnFromUser = i_ChosenCrad.Column;
            return m_CardsPairBoard[rowFromUser, columnFromUser];
        }

        private void setCardsInfo()
        {
            int[] apper = new int[m_NumberOfPairs];
            for (int i = 0; i < m_NumberOfPairs; i++)
            {
                apper[i] = 2;
            }

            for (int i = 0; i < m_GameUI.Height; i++)
            {
                for (int j = 0; j < m_GameUI.Width; j++)
                {
                    while (true)
                    {
                        int index = m_RandomValue.Next(0, m_NumberOfPairs);
                        apper[index]--;
                        if (apper[index] >= 0)
                        {
                            m_CardsPairBoard[i, j] = index;
                            break;
                        }
                    }
                }
            }
        }

        private void setPlayers()
        {
            string player1Name = m_GameUI.GetPlayerNameFromUser();
            m_Player1 = new Player(player1Name, ePlayerType.Human);
            bool isPlayer2Human = m_GameUI.IsSecondPlayerHuman();
            ePlayerType playerType = isPlayer2Human ? ePlayerType.Human : ePlayerType.Computer;
            string player2Name;
            if (playerType == ePlayerType.Human)
            {
                player2Name = m_GameUI.GetPlayerNameFromUser();
            }
            else
            {
                player2Name = k_ComputerName;
            }

            m_Player2 = new Player(player2Name, playerType);
        }

        private bool isGameFinish()
        {
            return false;
        }
    }
}