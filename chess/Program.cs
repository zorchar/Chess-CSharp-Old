using System;
namespace chess
{
    class Program
    {
        static void Main(string[] args)
        {
            new Board().PlayGame();
        }
        static bool IsInputValid(string input)
        {
            string tempInput = input;
            tempInput = tempInput.Trim();
            if (tempInput.Length != 4)
                return false;
            if (!((tempInput[0] >= 65 && tempInput[0] <= 72) || (tempInput[0] >= 97 && tempInput[0] <= 104)))
                return false;
            if (!((tempInput[1] >= 49 && tempInput[1] <= 56)))
                return false;
            if (!((tempInput[2] >= 65 && tempInput[2] <= 72) || (tempInput[2] >= 97 && tempInput[2] <= 104)))
                return false;
            if (!((tempInput[3] >= 49 && tempInput[3] <= 56)))
                return false;
            return true;
        }
        static string TrimInputAndChangeToUsableFormat(string input)
        {
            string changedInput = "";
            changedInput += ('8') - (input[1]);
            if (input[0] >= 65 && input[0] <= 72)
                changedInput += (char)(input[0] - 17);
            else if (input[0] >= 97 && input[0] <= 104)
                changedInput += (char)(input[0] - 49);
            changedInput += ('8') - (input[3]);
            if (input[2] >= 65 && input[2] <= 72)
                changedInput += (char)(input[2] - 17);
            else if (input[2] >= 97 && input[2] <= 104)
                changedInput += (char)(input[2] - 49);
            return changedInput;
        }
        class Piece
        {
            public char sign; // 1 is black king, 2 is black queen...
            public bool isWhite;
            public Board board;
            public Piece() { }
            public Piece(Board board, char sign, bool isWhite)
            {
                this.board = board;
                this.sign = sign;
                this.isWhite = isWhite;
            }
            public virtual bool IsLegalMove(bool displayReason, int xOld, int yOld, int xNew, int yNew)
            { return true; }
        }
        class Knight : Piece
        {
            public Knight(Board board, bool isWhite) : base(board, isWhite ? 'N' : 'n', isWhite)
            { }
            public override bool IsLegalMove(bool displayReason, int xOld, int yOld, int xNew, int yNew)
            {
                // if knight can move that way
                if (!((Math.Abs(xOld - xNew) == 2 && Math.Abs(yOld - yNew) == 1) || (Math.Abs(xOld - xNew) == 1 && Math.Abs(yOld - yNew) == 2)))
                {
                    if (displayReason)
                        Console.WriteLine("Illegal move. Knights don't move that way.");
                    return false;
                }
                // check if square is empty or has rival
                if (board.IsSquareOpponent(xNew, yNew) || board.IsSquareEmpty(xNew, yNew))
                    return true;
                if (displayReason)
                    Console.WriteLine("Illegal move. Can't take own piece.");
                return false;
            }
        }
        class Rook : Piece
        {
            public Rook(Board board, bool isWhite) : base(board, isWhite ? 'R' : 'r', isWhite)
            { }
            public override bool IsLegalMove(bool displayReason, int xOld, int yOld, int xNew, int yNew)
            {
                // if rook can move that way
                if (!((xOld - xNew == 0 || yOld - yNew == 0) && (xOld - xNew != 0 || yOld - yNew != 0)))
                {
                    if (displayReason)
                        Console.WriteLine("Illegal move. Rooks don't move that way.");
                    return false;
                }
                //check if nothing on the way
                int xStep = 0, yStep = 0;
                if (xNew - xOld > 0)
                    xStep = 1;
                if (yNew - yOld > 0)
                    yStep = 1;
                if (xNew - xOld < 0)
                    xStep = -1;
                if (yNew - yOld < 0)
                    yStep = -1;
                int i = xOld + xStep, j = yOld + yStep;
                while (i < 8 && j < 8 && (i != xNew || j != yNew))
                {
                    if (!board.IsSquareEmpty(i, j))
                    {
                        if (displayReason)
                            Console.WriteLine("Illegal move. A piece is blocking the way.");
                        return false;
                    }
                    i += xStep;
                    j += yStep;
                }
                // check if square is empty or has rival
                if (board.IsSquareOpponent(xNew, yNew) || board.IsSquareEmpty(xNew, yNew))
                    return true;
                if (displayReason)
                    Console.WriteLine("Illegal move. Can't take own piece.");
                return false;
            }
        }
        class Bishop : Piece
        {
            public Bishop(Board board, bool isWhite) : base(board, isWhite ? 'B' : 'b', isWhite)
            { }
            public override bool IsLegalMove(bool displayReason, int xOld, int yOld, int xNew, int yNew)
            {
                // if bishop can move that way
                if (Math.Abs(xOld - xNew) != Math.Abs(yOld - yNew))
                {
                    if (displayReason)
                        Console.WriteLine("Illegal move. Bishops don't move that way.");
                    return false;
                }
                //check if nothing on the way
                int xStep = 1, yStep = 1;
                if (xNew - xOld < 0)
                    xStep = -1;
                if (yNew - yOld < 0)
                    yStep = -1;
                int i = xOld + xStep, j = yOld + yStep;
                while (i < 8 && j < 8 && i != xNew)
                {
                    if (!board.IsSquareEmpty(i, j))
                    {
                        if (displayReason)
                            Console.WriteLine("Illegal move. A piece is blocking the way.");
                        return false;
                    }
                    i += xStep;
                    j += yStep;
                }
                // check if square is empty or has rival - make function in Piece
                if (board.IsSquareOpponent(xNew, yNew) || board.IsSquareEmpty(xNew, yNew))
                    return true;
                if (displayReason)
                    Console.WriteLine("Illegal move. Can't take own piece.");
                return false;
            }
        }
        class Queen : Piece
        {
            public Queen(Board board, bool isWhite) : base(board, isWhite ? 'Q' : 'q', isWhite)
            { }
            public override bool IsLegalMove(bool displayReason, int xOld, int yOld, int xNew, int yNew)
            {
                // if queen can move that way
                if ((Math.Abs(xOld - xNew) != Math.Abs(yOld - yNew)) && !((xOld - xNew == 0 || yOld - yNew == 0) && (xOld - xNew != 0 || yOld - yNew != 0)))
                {
                    if (displayReason)
                        Console.WriteLine("Illegal move. Queens don't move that way.");
                    return false;
                }
                //check if nothing on the way
                int xStep = 0, yStep = 0;
                if (xNew - xOld > 0)
                    xStep = 1;
                if (yNew - yOld > 0)
                    yStep = 1;
                if (xNew - xOld < 0)
                    xStep = -1;
                if (yNew - yOld < 0)
                    yStep = -1;
                int i = xOld + xStep, j = yOld + yStep;
                while (i < 8 && j < 8 && (i != xNew || j != yNew))
                {
                    if (!board.IsSquareEmpty(i, j))
                    {
                        if (displayReason)
                            Console.WriteLine("Illegal move. A piece is blocking the way.");
                        return false;
                    }
                    i += xStep;
                    j += yStep;
                }
                // check if square is empty or has rival
                if (board.IsSquareOpponent(xNew, yNew) || board.IsSquareEmpty(xNew, yNew))
                    return true;
                if (displayReason)
                    Console.WriteLine("Illegal move. Can't take own piece.");
                return false;
            }
        }
        class King : Piece
        {
            public King(Board board, bool isWhite, int x, int y) : base(board, isWhite ? 'K' : 'k', isWhite)
            {
                if (isWhite)
                {
                    board.setXWhiteKing(x);
                    board.setYWhiteKing(y);
                }
                else
                {
                    board.setXBlackKing(x);
                    board.setYBlackKing(y);
                }
            }
            public override bool IsLegalMove(bool displayReason, int xOld, int yOld, int xNew, int yNew)
            {
                // if king wants to castle long (to move two squares left)
                if ((yOld - yNew == 2 && xOld == xNew))
                {
                    // if can potentially castle long and not in check
                    if (board.grid[isWhite ? 7 : 0, 0] != null && !board.IsKingInCheck(false, isWhite) && (isWhite ? board.canWhitePotentiallyCastleLong : board.canBlackPotentiallyCastleLong))
                    {
                        //if something is on the way or square is threatened
                        if (board.grid[xOld, yOld - 1] != null || board.IsSquareThreatened(xOld, yOld - 1))
                        {
                            if (displayReason)
                                Console.WriteLine("Can't castle. A piece is in the way or square is threatened.");
                            return false;
                        }
                        if (board.grid[xOld, yOld - 2] != null || board.IsSquareThreatened(xOld, yOld - 2))
                        {
                            if (displayReason)
                                Console.WriteLine("Can't castle. A piece is in the way or square is threatened.");
                            return false;
                        }
                        if (board.grid[xOld, yOld - 3] != null)
                        {
                            if (displayReason)
                                Console.WriteLine("Can't castle. A piece is in the way.");
                            return false;
                        }
                        return true;
                    }
                    if (displayReason)
                        Console.WriteLine("Can't castle.Check/King or rook already moved / are out of position.");
                    return false;
                }
                // if king wants to castle short (to move two squares right)
                else if ((yOld - yNew == -2 && xOld == xNew))
                {
                    // if potentially can castle short and king not in check
                    if (board.grid[isWhite ? 7 : 0, 7] != null && !board.IsKingInCheck(false, isWhite) && (isWhite ? board.canWhitePotentiallyCastleShort : board.canBlackPotentiallyCastleShort))
                    {
                        //if something is on the way or square is threatended
                        if (board.grid[xOld, yOld + 1] != null || board.IsSquareThreatened(xOld, yOld + 1))
                        {
                            if (displayReason)
                                Console.WriteLine("Can't castle. A piece is in the way or square is threatened.");
                            return false;
                        }
                        if (board.grid[xOld, yOld + 2] != null || board.IsSquareThreatened(xOld, yOld + 2))
                        {
                            if (displayReason)
                                Console.WriteLine("Can't castle. A piece is in the way or square is threatened.");
                            return false;
                        }
                        return true;
                    }
                    if (displayReason)
                        Console.WriteLine("Can't castle.Check/King or rook already moved / are out of position.");
                    return false;
                }
                // if king can move that way
                if (!(!(xNew == xOld && yNew == yOld) && (Math.Abs(xOld - xNew) == 1 || Math.Abs(xOld - xNew) == 0) && (Math.Abs(yOld - yNew) == 1 || Math.Abs(yOld - yNew) == 0)))
                {
                    if (displayReason)
                        Console.WriteLine("Illegal move. Kings don't move that way.");
                    return false;
                }
                // check if square is empty or has rival
                if (board.IsSquareOpponent(xNew, yNew) || board.IsSquareEmpty(xNew, yNew))
                    return true;
                if (displayReason)
                    Console.WriteLine("Illegal move. Can't take own piece.");
                return false;
            }
        }
        class Pawn : Piece
        {
            public Pawn(Board board, bool isWhite) : base(board, isWhite ? 'P' : 'p', isWhite)
            { }
            public override bool IsLegalMove(bool displayReason, int xOld, int yOld, int xNew, int yNew)
            {
                // if walked one square up
                int xStep = 1;
                if (isWhite)
                    xStep = -1;
                if (xNew - xOld == xStep)
                {
                    // if sideways
                    if (Math.Abs(yNew - yOld) == 1)
                    {
                        if (board.IsSquareOpponent(xNew, yNew))
                            return true;
                        //if attacking empty square
                        if (board.IsSquareEmpty(xNew, yNew))
                        {
                            // if en passant availiable and pawn is attacking it
                            if (board.getIsEnPassantAvailiable() && xNew == board.getEnPassantSquareX() && yNew == board.getEnPassantSquareY())
                                return true;
                            else if (displayReason)
                                Console.WriteLine("Illegal move. Can't attack empty square.");
                            return false;
                        }
                        if (displayReason)
                            Console.WriteLine("Illegal move. Can't take own piece.");
                        return false;
                    }
                    // if up
                    else if (yNew - yOld == 0)
                    {
                        if (board.IsSquareEmpty(xNew, yNew))
                            return true;
                        if (board.IsSquareOpponent(xNew, yNew))
                        {
                            if (displayReason)
                                Console.WriteLine("Illegal move. Pawns can't attack that way. Only sideways.");
                            return false;
                        }
                        if (displayReason)
                            Console.WriteLine("Illegal move. Can't take own piece.");
                        return false;
                    }
                }
                // if walked two squares up
                if (xNew - xOld == xStep * 2 && yOld - yNew == 0)
                {
                    // if is on initial square
                    if ((isWhite && xOld == 6) || (!isWhite && xOld == 1))
                    {
                        //if square on the way is empty
                        if (board.IsSquareEmpty(xOld + xStep, yOld))
                        {
                            // if final square is empty
                            if (board.IsSquareEmpty(xNew, yNew))
                                return true;
                            // if final square has opponent
                            if (board.IsSquareOpponent(xNew, yNew))
                            {
                                if (displayReason)
                                    Console.WriteLine("Illegal move. Can't move pawn to opponents square.");
                                return false;
                            }
                            if (displayReason)
                                Console.WriteLine("Illegal move. Can't move pawn to your own piece square.");
                            return false;
                        }
                        if (displayReason)
                            Console.WriteLine("Illegal move. A piece is blocking the way.");
                        return false;
                    }
                }
                if (displayReason)
                    Console.WriteLine("Illegal move. Pawns don't move that way.");
                return false;
            }
            public override string ToString()
            {
                if (isWhite)
                    return "P";
                else
                    return "p";
            }
        }
        class Board
        {
            public bool canWhitePotentiallyCastleLong = true, canWhitePotentiallyCastleShort = true, canBlackPotentiallyCastleLong = true, canBlackPotentiallyCastleShort = true;
            string input;
            int xOld, yOld, xNew, yNew;
            int enPassantSquareX, enPassantSquareY, lastRemovedPieceX, lastRemovedPieceY, xWhiteKing, yWhiteKing, xBlackKing, yBlackKing;
            bool isEnPassantAvailiable = false;
            Piece lastRemovedPiece;
            bool currentTurnIsWhite = true;
            public string[] positionsArray = new string[101];
            int positionsArrayIndex = 0;
            public Piece[,] grid = new Piece[8, 8];
            public Board()
            {
                grid[0, 0] = new Rook(this, false);
                grid[0, 1] = new Knight(this, false);
                grid[0, 2] = new Bishop(this, false);
                grid[0, 3] = new Queen(this, false);
                grid[0, 4] = new King(this, false, 0, 4);
                grid[0, 5] = new Bishop(this, false);
                grid[0, 6] = new Knight(this, false);
                grid[0, 7] = new Rook(this, false);
                grid[7, 0] = new Rook(this, true);
                grid[7, 1] = new Knight(this, true);
                grid[7, 2] = new Bishop(this, true);
                grid[7, 3] = new Queen(this, true);
                grid[7, 4] = new King(this, true, 7, 4);
                grid[7, 5] = new Bishop(this, true);
                grid[7, 6] = new Knight(this, true);
                grid[7, 7] = new Rook(this, true);
                for (int j = 0; j < 8; j++)
                    grid[1, j] = new Pawn(this, false);
                for (int j = 0; j < 8; j++)
                    grid[6, j] = new Pawn(this, true);
            }
            public int getEnPassantSquareX() { return enPassantSquareX; }
            public void setEnPassantSquareX(int x) { enPassantSquareX = x; }
            public int getEnPassantSquareY() { return enPassantSquareY; }
            public void setEnPassantSquareY(int y) { enPassantSquareY = y; }
            public bool getIsEnPassantAvailiable() { return isEnPassantAvailiable; }
            public void setEnPassantAvailiable(bool isAvailable) { isEnPassantAvailiable = isAvailable; }
            public Piece getLastRemovedPiece() { return lastRemovedPiece; }
            public int getLastRemovedPieceX() { return lastRemovedPieceX; }
            public int getLastRemovedPieceY() { return lastRemovedPieceY; }
            public void setXWhiteKing(int x) { xWhiteKing = x; }
            public void setYWhiteKing(int y) { yWhiteKing = y; }
            public void setXBlackKing(int x) { xBlackKing = x; }
            public void setYBlackKing(int y) { yBlackKing = y; }
            public void ParseInputIntoAppropriateVariables()
            {
                xOld = int.Parse("" + input[0]);
                yOld = int.Parse("" + input[1]);
                xNew = int.Parse("" + input[2]);
                yNew = int.Parse("" + input[3]);
            }
            public void moveRookToCompleteOrReverseCastling(bool isReverse)
            {
                if (yOld - yNew == 2)
                {
                    grid[(getCurrentTurn() ? 7 : 0), (isReverse ? 0 : 3)] = grid[(getCurrentTurn() ? 7 : 0), (isReverse ? 3 : 0)];
                    grid[(getCurrentTurn() ? 7 : 0), (isReverse ? 3 : 0)] = null;
                }
                else if (yOld - yNew == -2)
                {
                    grid[(getCurrentTurn() ? 7 : 0), (isReverse ? 7 : 5)] = grid[(getCurrentTurn() ? 7 : 0), (isReverse ? 5 : 7)];
                    grid[(getCurrentTurn() ? 7 : 0), (isReverse ? 5 : 7)] = null;
                }
            }
            public void UpdateKingPosition(int x, int y)
            {
                if (getCurrentTurn())
                {
                    xWhiteKing = x;
                    yWhiteKing = y;
                }
                else
                {
                    xBlackKing = x;
                    yBlackKing = y;
                }
            }
            public void UpdateLastRemovedPiece(int x, int y)
            {
                lastRemovedPiece = grid[x, y];
                lastRemovedPieceX = x;
                lastRemovedPieceY = y;
            }
            public void PlayGame()
            {
                printGrid();
                addPositionStringToArray();
                while (true)
                {
                    Console.Write("Enter move: ");
                    input = Console.ReadLine();
                    Console.WriteLine();
                    if (IsInputValid(input))
                        input = TrimInputAndChangeToUsableFormat(input);
                    else
                    {
                        Console.WriteLine("Invalid input.");
                        continue;
                    }
                    ParseInputIntoAppropriateVariables();
                    if (grid[xOld, yOld] == null)
                    {
                        Console.WriteLine("Invalid move. No piece on square.");
                        continue;
                    }
                    if (grid[xOld, yOld].isWhite != getCurrentTurn())
                    {
                        Console.WriteLine("Illegal move. Can't move opponent's pieces.");
                        continue;
                    }
                    // if move is legal
                    if (grid[xOld, yOld].IsLegalMove(true, xOld, yOld, xNew, yNew))
                    {
                        bool enPassantIsAvailableBeforeMove = getIsEnPassantAvailiable();
                        int enPassantSquareXBeforeMove = getEnPassantSquareX(), enPassantSquareYBeforeMove = getEnPassantSquareY();
                        makeMove(xOld, yOld, xNew, yNew);
                        // if king is in check - turn back
                        if (IsKingInCheck(false, getCurrentTurn()))
                        {
                            reverseLastMove(xOld, yOld, xNew, yNew);
                            SetEnPassantProperties(enPassantSquareXBeforeMove, enPassantSquareYBeforeMove, enPassantIsAvailableBeforeMove);
                            Console.WriteLine("Illegal move. Your king will be in check.");
                            continue;
                        }
                        // check king move and change move status of other rook to prevent 3 fold errors
                        if (grid[xNew, yNew] is King)
                        {
                            if (getCurrentTurn())
                            {
                                canWhitePotentiallyCastleLong = false;
                                canWhitePotentiallyCastleShort = false;
                            }
                            else
                            {
                                canBlackPotentiallyCastleLong = false;
                                canBlackPotentiallyCastleShort = false;
                            }
                        }
                        // check if a rook moved, potential castling
                        if (grid[xNew, yNew] is Rook)
                        {
                            if (getCurrentTurn())
                            {
                                if (yOld == 0 && xOld == 7)
                                    canWhitePotentiallyCastleLong = false;
                                else if (yOld == 7 && xOld == 7)
                                    canWhitePotentiallyCastleShort = false;
                            }
                            else
                            {
                                if (yOld == 0 && xOld == 0)
                                    canBlackPotentiallyCastleLong = false;
                                else if (yOld == 7 && xOld == 0)
                                    canBlackPotentiallyCastleShort = false;
                            }
                        }
                        IsPawnNeedsPromotion(xNew, yNew);
                        switchCurrentTurn();
                        printGrid();
                        // if pawn move or piece taken
                        if (getLastRemovedPiece() != null || grid[xNew, yNew] is Pawn)
                        {
                            // clear stringslist
                            positionsArray = new string[101];
                            resetPositionArrayIndex();
                        }
                        addPositionStringToArray();
                        //check if have no legal moves
                        if (!areThereAnyLegalMoves(getCurrentTurn(), getIsEnPassantAvailiable(), getLastRemovedPieceX(), getLastRemovedPieceY()))
                        {
                            // check if king is in check after recent move
                            if (IsKingInCheck(false, getCurrentTurn()))
                            {
                                Console.WriteLine("Checkmate. {0} wins!", getCurrentTurn() ? "black" : "white");
                                break;
                            }
                            Console.WriteLine("Stalemate.");
                            break;
                        }
                        if (IsThreeFoldRepetition())
                        {
                            Console.WriteLine("Draw. Threefold repetition.");
                            break;
                        }
                        if (positionsArray[100] != null)
                        {
                            Console.WriteLine("Draw. Fifty moves without pawn move or taking a piece.");
                            break;
                        }
                        if (countObjectsForDraw())
                        {
                            Console.WriteLine("Draw. Too few pieces to win.");
                            break;
                        }
                        IsKingInCheck(true, getCurrentTurn());
                    }
                }
                Console.ReadLine();
            }
            public void resetPositionArrayIndex() { positionsArrayIndex = 0; }
            public bool countObjectsForDraw()
            {
                int pawnCount = 0, rookCount = 0, queenCount = 0, knightCount = 0, bishopCount = 0;
                int whiteBishopCountBlackSquare = 0, whiteBishopCountWhiteSquare = 0;
                int blackBishopCountBlackSquare = 0, blackBishopCountWhiteSquare = 0;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (grid[i, j] is Pawn)
                            pawnCount++;
                        else if (grid[i, j] is Rook)
                            rookCount++;
                        else if (grid[i, j] is Bishop)
                        {
                            bishopCount++;
                            if (grid[i, j].isWhite)
                            {
                                if ((i + j) % 2 == 0)
                                    whiteBishopCountBlackSquare++;
                                else
                                    whiteBishopCountWhiteSquare++;
                            }
                            else
                            {
                                if ((i + j) % 2 == 0)
                                    blackBishopCountBlackSquare++;
                                else
                                    blackBishopCountWhiteSquare++;
                            }
                        }
                        else if (grid[i, j] is Knight)
                            knightCount++;
                        else if (grid[i, j] is Queen)
                            queenCount++;
                    }
                }
                if (queenCount == 0 && rookCount == 0 && pawnCount == 0)
                {
                    if (knightCount == 1 && bishopCount == 0 || knightCount == 0 && bishopCount == 1)
                        return true;
                    if (knightCount == 0 && ((blackBishopCountBlackSquare == 1 && whiteBishopCountBlackSquare == 1) || (blackBishopCountWhiteSquare == 1 && whiteBishopCountWhiteSquare == 1)))
                        return true;
                }
                return false;
            }
            public bool IsPawnNeedsPromotion(int xNew, int yNew)
            {
                if (grid[xNew, yNew] is Pawn && (xNew == 7 || xNew == 0))
                {
                    Console.WriteLine("Which piece to promote to ?\n1.knight\n2.bishop\n3.rook\nelse.queen");
                    string askPromotion = Console.ReadLine();
                    if (askPromotion == "1")
                        grid[xNew, yNew] = new Knight(this, getCurrentTurn());
                    else if (askPromotion == "2")
                        grid[xNew, yNew] = new Bishop(this, getCurrentTurn());
                    else if (askPromotion == "3")
                        grid[xNew, yNew] = new Rook(this, getCurrentTurn());
                    else
                        grid[xNew, yNew] = new Queen(this, getCurrentTurn());
                    return true;
                }
                return false;
            }
            public bool getCurrentTurn() { return currentTurnIsWhite; }
            public void switchCurrentTurn() { currentTurnIsWhite = !currentTurnIsWhite; }
            public bool IsSquareOpponent(int x, int y)
            {
                if (grid[x, y] != null && grid[x, y].isWhite != getCurrentTurn())
                    return true;
                return false;
            }
            public void printGrid()
            {
                Console.WriteLine();
                int k = 8;
                for (int i = 0; i < 8; i++)
                {
                    Console.Write(k + " ");
                    k--;
                    for (int j = 0; j < 8; j++)
                    {
                        if (grid[i, j] != null)
                        {
                            Console.Write(" {0}", grid[i, j].isWhite ? "W" : "B");
                            Console.Write((grid[i, j].sign + " ").ToUpper());
                        }
                        else
                            Console.Write(" __ ");
                    }
                    Console.WriteLine("\n");
                }
                Console.Write("/");
                k = 65;
                for (int j = 0; j < 8; j++, k++)
                    Console.Write("  " + (char)k + " ");
                Console.WriteLine("\n\n");
            }
            public bool IsSquareEmpty(int x, int y)
            {
                if (grid[x, y] == null)
                    return true;
                return false;
            }
            public void makeMove(int xOld, int yOld, int xNew, int yNew)
            {
                isEnPassantAvailiable = false;
                UpdateLastRemovedPiece(xNew, yNew);
                grid[xNew, yNew] = grid[xOld, yOld];
                grid[xOld, yOld] = null;
                //if is pawn
                if (grid[xNew, yNew] is Pawn)
                {
                    //if did a double move
                    if (xOld - xNew == (grid[xNew, yNew].isWhite ? 2 : -2))
                        SetEnPassantProperties(xNew + (getCurrentTurn() ? 1 : -1), yNew, true);
                    //if made attacking move(sideways) and attacked empty square - means it's en passant
                    else if (Math.Abs(yNew - yOld) == 1 && lastRemovedPiece == null)
                    {
                        UpdateLastRemovedPiece(xNew + (getCurrentTurn() ? 1 : -1), yNew);
                        grid[xNew + (getCurrentTurn() ? 1 : -1), yNew] = null;
                    }
                }
                else if (grid[xNew, yNew] is King)
                {
                    UpdateKingPosition(xNew, yNew);
                    if (grid[xNew, yNew] is King && Math.Abs(yOld - yNew) == 2)
                        moveRookToCompleteOrReverseCastling(false);
                }
            }
            public void reverseLastMove(int xOld, int yOld, int xNew, int yNew)
            {
                grid[xOld, yOld] = grid[xNew, yNew];
                grid[xNew, yNew] = lastRemovedPiece;
                //if is pawn and if made attacking move(sideways) and attacked different square from last piece location - means it's an passant
                if (grid[xNew, yNew] is Pawn && Math.Abs(yNew - yOld) == 1 && !(xNew == lastRemovedPieceX && yNew == lastRemovedPieceY))
                {
                    isEnPassantAvailiable = true;
                    grid[xNew + (grid[xNew, yNew].isWhite ? -1 : 1), yNew] = lastRemovedPiece;
                    lastRemovedPiece = null;
                    grid[xNew, yNew] = null;
                }
                else if (grid[xNew, yNew] is King)
                {
                    UpdateKingPosition(xOld, yOld);
                    if (grid[xNew, yNew] is King && Math.Abs(yOld - yNew) == 2)
                        moveRookToCompleteOrReverseCastling(true);
                }
            }
            public void addPositionStringToArray()
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {
                        if (grid[i, j] != null)
                            positionsArray[positionsArrayIndex] += "" + grid[i, j].sign;
                        else
                            positionsArray[positionsArrayIndex] += " ";
                    }
                positionsArray[positionsArrayIndex] += (this.isEnPassantAvailiable ? 'y' : 'n');
                positionsArray[positionsArrayIndex] += (canBlackPotentiallyCastleLong ? 'y' : 'n');
                positionsArray[positionsArrayIndex] += (canBlackPotentiallyCastleShort ? 'y' : 'n');
                positionsArray[positionsArrayIndex] += (canWhitePotentiallyCastleLong ? 'y' : 'n');
                positionsArray[positionsArrayIndex] += (canWhitePotentiallyCastleShort ? 'y' : 'n');
                positionsArray[positionsArrayIndex] += (getCurrentTurn() ? 'w' : 'b');
                // add to posision index
                positionsArrayIndex++;
            }
            public bool IsThreeFoldRepetition()
            {
                string temp = "";
                int numOfRecursions = 0;
                for (int i = 0; i < positionsArray.Length; i++)
                {
                    if (positionsArray[i] != null)
                        temp = positionsArray[i];
                    else
                        break;
                }
                for (int i = 0; i < positionsArray.Length && positionsArray[i] != null; i++)
                {
                    if (positionsArray[i] == temp)
                        numOfRecursions++;
                    if (numOfRecursions == 3)
                        return true;
                }
                return false;
            }
            public bool IsKingInCheck(bool displayReason, bool isWhite)
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (grid[i, j] != null && grid[i, j].isWhite != getCurrentTurn() && grid[i, j].IsLegalMove(false, i, j, isWhite ? xWhiteKing : xBlackKing, isWhite ? yWhiteKing : yBlackKing))
                        {
                            if (displayReason)
                                Console.WriteLine("{0} is in check.", getCurrentTurn() ? "white" : "black");
                            return true;
                        }
                return false;
            }
            public bool IsSquareThreatened(int x, int y)
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (this.grid[i, j] != null && grid[i, j].isWhite != getCurrentTurn() && this.grid[i, j].IsLegalMove(false, i, j, x, y))
                            return true;
                return false;
            }
            public bool areThereAnyLegalMoves(bool isWhite, bool enPassantState, int lastPieceX, int lastPieceY)
            {
                // check every square
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        // if there is an opponent's piece on square
                        if (grid[i, j] != null && this.grid[i, j].isWhite == isWhite)
                            // check every possible square
                            for (int k = 0; k < 8; k++)
                                for (int l = 0; l < 8; l++)
                                    // if legal move
                                    if (grid[i, j].IsLegalMove(false, i, j, k, l))
                                    {
                                        makeMove(i, j, k, l);
                                        // if king is not in check
                                        if (!IsKingInCheck(false, isWhite))
                                        {
                                            reverseLastMove(i, j, k, l);
                                            isEnPassantAvailiable = enPassantState; // not sure 100% - meanwhile no bugs found ---- later update - check if needed or take out of reverse and make move
                                            UpdateLastRemovedPiece(lastPieceX, lastPieceY);
                                            return true;
                                        }
                                        reverseLastMove(i, j, k, l);
                                        isEnPassantAvailiable = enPassantState; // not sure 100 % -meanwhile no bugs found
                                        UpdateLastRemovedPiece(lastPieceX, lastPieceY);
                                    }
                return false;
            }
            public void SetEnPassantProperties(int enPassantSquareX, int enPassantSquareY, bool isEnPassantAvailiable)
            {
                this.isEnPassantAvailiable = isEnPassantAvailiable;
                this.enPassantSquareX = enPassantSquareX;
                this.enPassantSquareY = enPassantSquareY;
            }
        }
    }
}