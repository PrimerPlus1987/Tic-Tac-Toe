using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;

internal class Program
{

   static char[,] gameBoard = new char[,]
        {
        /*     Column
               0    1    2      */            
            { '1', '2', '3' },   // row 0
            { '4', '5', '6' },   // row 1
            { '7' ,'8', '9' }    // row 2
        };

    static char[] usedSpaces = new char[] 
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i'
            };

    static int player = 1; //there will be 2 players starting with player 1
    static char mark = ' ';
    static bool anotherGame = true;
    static bool draw = false;
    static char choice = ' ';
    static bool keepGoing = true;

    private static void Main(string[] args)
    {
        while (anotherGame)
        {

            do
            {
                Console.Clear();
                BoardGame();

                Console.WriteLine("Player 1 is X and Player 2 is O\n");

                if (player % 2 == 0)
                {
                    Console.WriteLine("Player 2's turn. Make your choice for spot.");
                    player++;
                    mark = 'O';
                }
                else
                {
                    Console.WriteLine("PLayer 1's turn. Make your choice for spot.");
                    player++;
                    mark = 'X';
                }

                bool validateChoice = char.TryParse(Console.ReadLine(), out choice);

                int actual = CharToInt(choice);

                if (choice == '1' || choice == '2' || choice == '3' || choice == '4' || choice == '5' ||
                    choice == '6' || choice == '7' || choice == '8' || choice == '9')
                {
                    if (mark == 'X')
                    {
                        for (int i = 0; i < gameBoard.GetLength(0); i++)
                        {
                            for (int j = 0; j < gameBoard.GetLength(1); j++)
                            {
                                if (usedSpaces[actual - 1] != 'X' && usedSpaces[actual - 1] != 'O')
                                {
                                    if (choice == gameBoard[i, j])
                                    {
                                        gameBoard[i, j] = 'X';
                                        usedSpaces[actual - 1] = 'X';
                                        goto Next;
                                    }

                                }
                                else
                                {
                                    Console.WriteLine("Error Message PLayer 1");
                                    Console.WriteLine("This spot is already taken. Press enter to try again.");
                                    Console.ReadKey();
                                    player--;
                                    goto Next;
                                }

                            }
                        }
                    }
                    //mark = 'O'
                    else
                    {
                        for (int i = 0; i < gameBoard.GetLength(0); i++)
                        {
                            for (int j = 0; j < gameBoard.GetLength(1); j++)
                            {
                                if (usedSpaces[actual - 1] != 'X' && usedSpaces[actual - 1] != 'O')
                                {
                                    if (choice == gameBoard[i, j])
                                    {
                                        gameBoard[i, j] = 'O';
                                        usedSpaces[actual - 1] = 'O';
                                        goto Next;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Error message PLayer 2.");
                                    Console.WriteLine("This spot is already taken. Press enter to try again.");
                                    Console.ReadKey();
                                    player--;
                                    goto Next;
                                }
                            }
                        }
                    }
                }

                else
                {
                    Console.WriteLine("This was not a valid choice.");
                    Console.WriteLine("Press enter to try again.");
                    Console.ReadKey();
                    player--;
                }

            Next:
                Console.WriteLine();

                keepGoing = WinOrDraw(gameBoard);

                if (keepGoing == true && draw == false)
                {
                    Console.Clear();
                    BoardGame();
                    Player();
                }
                else if (keepGoing == true && draw == true)
                {
                    Console.Clear();
                    BoardGame();
                    Console.WriteLine("This game was a DRAW. Better luck next time!");
                }


            } while (!keepGoing);

            bool waiting = false;


            do 
            {
                Console.WriteLine("\nWould you like to play again?\n");
                char input = char.ToLower(Console.ReadKey().KeyChar);

                if (input == 'y')
                {
                    anotherGame = true;
                    waiting = false;
                    ResetGame();
                }
                else if (input == 'n')
                {
                    waiting = false;
                    anotherGame = false;

                }
                else
                {
                    Console.WriteLine("\nWhat was that, I did not understand your selection.");
                    
                    waiting = true;

                }
            }while(waiting);

        }
    }
    


    static void Player()
    {
        if(mark == 'X')
        {
            Console.WriteLine("Player 1 has WON!");
        }
        else if(mark == 'O')
        {
            Console.WriteLine("Player 2 has WON!");
        }
    }
     
    static void BoardGame()
    {
        Console.Write("{0} | {1} | {2}", gameBoard[0, 0], gameBoard[0, 1], gameBoard[0, 2]);
        Console.Write("\n---------\n");
        Console.Write("{0} | {1} | {2}", gameBoard[1, 0], gameBoard[1, 1], gameBoard[1, 2]);
        Console.Write("\n---------\n");
        Console.Write("{0} | {1} | {2}", gameBoard[2, 0], gameBoard[2, 1], gameBoard[2, 2]);
        Console.Write("\n---------\n");
    }

    static int CharToInt(char c)
    {
        int actualValue = -1;

        if(c == '1')
        {
            actualValue = 1;
        }
        else if (c == '2')
        {
            actualValue = 2;
        }
        else if (c == '3')
        {
            actualValue = 3;
        }
        else if (c == '4')
        {
            actualValue = 4;
        }
        else if (c == '5')
        {
            actualValue = 5;
        }
        else if (c == '6')
        {
            actualValue = 6;
        }
        else if (c == '7')
        {
            actualValue = 7;
        }
        else if (c == '8')
        {
            actualValue = 8;
        }
        else if (c == '9')
        {
            actualValue = 9;
        }

        return actualValue;
    }

    static bool WinOrDraw(char[,] gameboard)
    {

        //checking board Left to Right Horizontal
        if (gameboard[0,0] == gameboard[0,1] && gameboard[0,1] == gameboard[0,2])
        {
          
            return true;
        }
        else if (gameboard[1,0] == gameboard[1,1] && gameboard[1,1] == gameboard[1,2])
        {
            
            return true;
        }
        else if (gameboard[2,0] == gameboard[2,1] && gameboard[2,1] == gameboard[2,2])
        {
            
            return true;
        }
        //Checking board Top to Bottom Vertical
        else if (gameboard[0,0] == gameboard[1,0] && gameboard[1,0] == gameboard[2,0])
        {
            
            return true;
        }
        else if (gameboard[0,1] == gameboard[1,1] && gameboard[1,1] == gameboard[2,1])
        {
            
            return true;
        }
        else if (gameboard[0,2] == gameboard[1,2] && gameboard[1,2] == gameboard[2,2])
        {
            
            return  true; 
        }

        //checking diagonal
        else if (gameboard[0,0] == gameboard[1,1] && gameboard[1,1] == gameboard[2,2])
        {
            
            return true;
        }
        else if (gameboard[0,2] == gameboard[1,1] && gameboard[1,1] == gameboard[2,0])
        {
            
            return true;
        }

        //check for draw
        else if (gameboard[0,0] != '1' && gameboard[0,1] != '2' && gameboard[0,2] != '3'
              && gameboard[1,0] != '4' && gameboard[1,1] != '5' && gameboard[1,2] != '6'
              && gameboard[2,0] != '7' && gameboard[2,1] != '8' && gameboard[2,2] != '9')
        {
            draw = true;           
            
            return true;
            
        }

        return false;
    }
    static void ResetGame()
    {
         gameBoard = new char[,]
        {
        /*     Column
               0    1    2      */            
            { '1', '2', '3' },   // row 0
            { '4', '5', '6' },   // row 1
            { '7' ,'8', '9' }    // row 2
        };

        usedSpaces = new char[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i'
            };

        player = 1;
        mark = ' ';
        anotherGame = true;
        draw = false;
        choice = ' ';

        keepGoing = true;

    }
}
