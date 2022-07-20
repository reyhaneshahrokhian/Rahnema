using System;
using System.Collections.Generic;
using System.Linq;

namespace Rahnema
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("How many side do you want for your game board?");
                int n = int.Parse(Console.ReadLine());

                Console.WriteLine("How many player are in the game?");
                int k = int.Parse(Console.ReadLine());

                GameBoard gameBoard = new GameBoard(n, k);

                for (int i = 0; i < k; i++)
                {
                    Console.WriteLine("Write the name of player{0}", i + 1);
                    gameBoard.players[i] = new Player(Console.ReadLine());
                }

                int turn = 0;
                while (true)
                {
                    if(gameBoard.emptySquares == 0)
                    {
                        Console.WriteLine("The game is over!");
                        gameBoard.PrintScores();
                        break;
                    }
                    if (turn == k)
                        turn = 0;

                    Console.WriteLine("1 -> play  2 -> print the board  3 -> print the scores  4 -> Exit from the game");
                    string input = Console.ReadLine();

                    if (input == "1")
                    {
                        Console.WriteLine("It's {0} turn", gameBoard.players[turn].name);
                        Console.WriteLine("Choose the line in this format : for example 0 0 h(h or H stand for horizontal and v or V stand for vertical");
                        string[] choose = Console.ReadLine().Split();
                        gameBoard.FillLine(int.Parse(choose[0]), int.Parse(choose[1]), choose[2], turn);
                        turn++;
                    }
                    else if (input == "2")
                        gameBoard.PrintBoard();

                    else if (input == "3")
                        gameBoard.PrintScores();

                    else if (input == "4")
                        break;

                    else
                        Console.WriteLine("The input is in wrong format");
                        
                }
            }
            catch (Exception e)
            {
                
            }
        }
    }
    interface IGameBoard
    {
        void PrintBoard();
        void FillLine(int x, int y, string type, int playerIndex);
        void CheckSquare(int i, int j, string type, int playerIndex);
        void PrintScores();
    }
    class GameBoard : IGameBoard
    {
        //number of sides
        int n;

        //check if the line is on or not
        bool[,] horizontals;
        bool[,] verticals;

        //number of fulled line among each squares
        int[,] squares;

        //number of empty squares in the game board
        public int emptySquares { get; set; }

        //array of players
        public Player[] players { get; set; }

        public GameBoard(int n, int k)
        {
            this.n = n;
            horizontals = new bool[n + 1, n + 1];
            verticals = new bool[n + 1, n + 1];
            squares = new int[n, n];
            emptySquares = n * n;
            players = new Player[k];
        }
        //print the game board
        public void PrintBoard()
        {
            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    Console.Write(".");

                    if (horizontals[i, j] == true)
                        Console.Write("__");

                    else
                        Console.Write("  ");
                }

                Console.WriteLine();

                for (int j = 0; j <= n; j++)
                {
                    if (verticals[j, i] == true)
                        Console.Write("|  ");

                    else
                        Console.Write("   ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("----------------------------------");
        }
        //fill the lines
        public void FillLine(int x, int y, string type, int playerIndex)
        {
            if (type == "H" || type == "h")
            {
                if (horizontals[x, y] == false)
                {
                    horizontals[x, y] = true;
                    CheckSquare(x, y, type, playerIndex);
                }
                else
                    Console.WriteLine("This line has been filled before");
            }
            else if (type == "V" || type == "v")
            {
                if (verticals[x, y] == false)
                {
                    verticals[x, y] = true;
                    CheckSquare(x, y, type, playerIndex);
                }
                else
                    Console.WriteLine("This line has been filled before");
            }
            else
            {
                Console.WriteLine("The charactor should be H(h) or V(v)");
            }
        }
        //check if the player can get that square or not
        public void CheckSquare(int i, int j, string type, int playerIndex)
        {
            if (type == "h" || type == "H")
            {
                if(i < n)
                {
                    squares[i, j]++;
                    if (squares[i, j] == 4)
                    {
                        players[playerIndex].score++;
                        emptySquares--;
                    }
                }
                if(i > 0)
                {
                    squares[i - 1, j]++;
                    if (squares[i - 1, j] == 4)
                    {
                        players[playerIndex].score++;
                        emptySquares--;
                    }
                }
            }
            else if (type == "V" || type == "v")
            {
                if (i < n)
                {
                    squares[j, i]++;
                    if (squares[j, i] == 4)
                        players[playerIndex].score++;
                }
                if (i > 0)
                {
                    squares[j, i - 1]++;
                    if (squares[j, i - 1] == 4)
                        players[playerIndex].score++;

                }
            }
        }
        //print scores of players
        public void PrintScores()
        {
            List<Player> winners = new List<Player>();
            winners.Add(players[0]);
            for (int i = 0; i < players.Length; i++)
            {
                Console.WriteLine("{0}'s score is : {1}", players[i].name, players[i].score);

                if (players[i].score > winners[0].score)
                {
                    winners.Clear();
                    winners.Add(players[i]);
                }
                else if (players[i].score == winners[0].score && players[i].name != winners[0].name)
                {
                    winners.Add(players[i]);
                }
            }
            Console.Write("The winners are : ");
            foreach (var winner in winners)
            {
                Console.Write(winner.name);
            }
            Console.WriteLine();
            Console.WriteLine("----------------------------------");
        }
    }
    class Player
    {
        public int score { get; set; }
        public string name { get; set; }
        public Player(string name)
        {
            this.name = name;
            this.score = 0;
        }
    }

}
