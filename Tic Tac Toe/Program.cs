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
    /* array to hold marks of X or O 
    * this will be used to check if current location has been used or not.
    * 
    *This array is being initialized with filler values.
    **/
    static char[] usedSpaces = new char[] 
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i'
            };
    // Counter so that can tell what players turn it is 
    static int player = 1; //there will be 2 players starting with player 1
    // Mark for identifying if player one or player 2
    static char mark = ' ';
    static bool anotherGame = true;
    static bool draw = false;
    // this is used for identifying what value on the board is chosen.
    static char choice = ' ';
    static bool keepGoing = true;

    private static void Main(string[] args)
    {
        /* Loop to keep game going if at the end of the first game 
        * players want to play another game.
        **/
        while (anotherGame)
        {

            do
            {
                // Clears the console so not to keep redisplaying the board and answers.
                Console.Clear();
                // Method call to load the board and any alterations made with the X and O's
                BoardGame();

                Console.WriteLine("Player 1 is X and Player 2 is O\n");
                // Since using a counter called player this allows to identify if player 1 or 2
                if (player % 2 == 0)
                {
                    Console.WriteLine("Player 2's turn. Make your choice for spot.");
                    player++;
                    mark = 'O';
                }
                else
                {
                    Console.WriteLine("Player 1's turn. Make your choice for spot.");
                    player++;
                    mark = 'X';
                }

                // Check if the value entered is an acceptable choice.
                bool validateChoice = char.TryParse(Console.ReadLine(), out choice);

                // Pass the choice to the method and have the method return the actual integer 
                int actual = CharToInt(choice);

                // Check what value was given by the user so that can see if the spot is available or not
                if (choice == '1' || choice == '2' || choice == '3' || choice == '4' || choice == '5' ||
                    choice == '6' || choice == '7' || choice == '8' || choice == '9')
                {
                    // Can tell if it is player 1 or 2 based on the mark
                    if (mark == 'X')
                    {
                        // loop through the board starting with dimension 0 and then next dimension 1
                        for (int i = 0; i < gameBoard.GetLength(0); i++)
                        {
                            for (int j = 0; j < gameBoard.GetLength(1); j++)
                            {
                                /* This is where I decided to use another array to store the marks
                                 * based on the location chosen by the user. Starting with
                                 * index 0 and going to index 8 for a total of 9 spaces
                                 * If the index does not have an X or an O then put an X into the 
                                 * 2 dimensional array at that location and update the normal array
                                 * to show it has an X now
                                 * Also need to break out of the loop so added a goto
                                 * */
                                if (usedSpaces[actual - 1] != 'X' && usedSpaces[actual - 1] != 'O')
                                {
                                    if (choice == gameBoard[i, j])
                                    {
                                        gameBoard[i, j] = 'X';
                                        usedSpaces[actual - 1] = 'X';
                                        goto Next;
                                    }

                                }

                                /* This is a way to tell which player had an error due to
                                 * the location already having an X or an O
                                 * To let the player retry need to decrease player count and 
                                 * break out of loop again with goto
                                 * */
                                else
                                {
                                    Console.WriteLine("Error Message Player 1");
                                    Console.WriteLine("This spot is already taken. Press enter to try again.");
                                    Console.ReadKey();
                                    player--;
                                    goto Next;
                                }

                            }
                        }
                    }
                    /*mark = 'O'
                    * This is for Player 2
                    * */
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
                                    Console.WriteLine("Error message Player 2.");
                                    Console.WriteLine("This spot is already taken. Press enter to try again.");
                                    Console.ReadKey();
                                    player--;
                                    goto Next;
                                }
                            }
                        }
                    }
                }
                /* If entered a wrong key for example and not a 1 - 9 
                 * player will be able to try again without losing their turn.
                 */
                else
                {
                    Console.WriteLine("This was not a valid choice.");
                    Console.WriteLine("Press enter to try again.");
                    Console.ReadKey();
                    player--;
                }
                // This is where to go when breaking out of the loop
            Next:
                Console.WriteLine();

                /* Take the gameboard and send it to the method to see if there is a
                 * winner or a draw return true if there is a winner or false if not
                */
                keepGoing = WinOrDraw(gameBoard);

                /* Keepgoing is used to tell the app to continue to run using this and 
                * checking if the gameboard had a draw or not to determine what to do
                * If game was won and not a draw then clear the console call the winning board and call
                * Player method to announce who the winner is
                */
                if (keepGoing == true && draw == false)
                {
                    Console.Clear();
                    BoardGame();
                    Player();
                }

                /* If game is over and it was a draw then clear the console 
                 * call the boardgame to bring the board back and announce the game was a draw
                 * */
                else if (keepGoing == true && draw == true)
                {
                    Console.Clear();
                    BoardGame();
                    Console.WriteLine("This game was a DRAW. Better luck next time!");
                }


            } while (!keepGoing);

            bool waiting = false;

            // Check if the players want to play another game
            do 
            {
                Console.WriteLine("\nWould you like to play again? Press Y for yes or N for no.\n");
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
    

    // Method that determines what player has won using the last mark.
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
     /* This is the method that draws the board each time it is called 
     * It makes sure to update each time with the new mark X or O
     * */
    static void BoardGame()
    {
        Console.Write("{0} | {1} | {2}", gameBoard[0, 0], gameBoard[0, 1], gameBoard[0, 2]);
        Console.Write("\n---------\n");
        Console.Write("{0} | {1} | {2}", gameBoard[1, 0], gameBoard[1, 1], gameBoard[1, 2]);
        Console.Write("\n---------\n");
        Console.Write("{0} | {1} | {2}", gameBoard[2, 0], gameBoard[2, 1], gameBoard[2, 2]);
        Console.Write("\n---------\n");
    }
    // This is the method that takes the choice that the user selected and returns the actual int value
    // of that value.
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

    /* This is the method to check if there is a winning condition 
     * If there is 3 in a row of the same type X or O for example then there has been 
     * a win. If there are no winning conditions check to see if all the spaces are filled
     * If all spaces are filled then the game was a draw.
     * */
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

    /* If users want to play another game this method will reset all values for a fresh start.
     * */
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
