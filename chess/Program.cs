﻿using System;
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
            public bool didMove = false;
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
                if (board.squareIsOpponent(xNew, yNew) || board.squareIsEmpty(xNew, yNew))
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
                    if (!board.squareIsEmpty(i, j))
                    {
                        if (displayReason)
                            Console.WriteLine("Illegal move. A piece is blocking the way.");
                        return false;
                    }
                    i += xStep;
                    j += yStep;
                }
                // check if square is empty or has rival
                if (board.squareIsOpponent(xNew, yNew) || board.squareIsEmpty(xNew, yNew))
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
                    if (!board.squareIsEmpty(i, j))
                    {
                        if (displayReason)
                            Console.WriteLine("Illegal move. A piece is blocking the way.");
                        return false;
                    }
                    i += xStep;
                    j += yStep;
                }
                // check if square is empty or has rival - make function in Piece
                if (board.squareIsOpponent(xNew, yNew) || board.squareIsEmpty(xNew, yNew))
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
                    if (!board.squareIsEmpty(i, j))
                    {
                        if (displayReason)
                            Console.WriteLine("Illegal move. A piece is blocking the way.");
                        return false;
                    }
                    i += xStep;
                    j += yStep;
                }
                // check if square is empty or has rival
                if (board.squareIsOpponent(xNew, yNew) || board.squareIsEmpty(xNew, yNew))
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
                    // if king didn't move and rook didnt move and not in check
                    if (!board.grid[xOld, yOld].didMove && board.grid[isWhite ? 7 : 0, 0] != null && !board.grid[isWhite ? 7 : 0, 0].didMove && !board.checkIfKingIsInCheck(false, isWhite))
                    {
                        //if something is on the way or square is threatened
                        if (board.grid[xOld, yOld - 1] != null || board.checkIfSquareIsThreatened(xOld, yOld - 1))
                        {
                            if (displayReason)
                                Console.WriteLine("Can't castle. A piece is in the way or square is threatened.");
                            return false;
                        }
                        if (board.grid[xOld, yOld - 2] != null || board.checkIfSquareIsThreatened(xOld, yOld - 2))
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
                    // if king didn't move and rook didnt move and king not in check
                    if (!board.grid[xOld, yOld].didMove && board.grid[isWhite ? 7 : 0, 7] != null && !board.grid[isWhite ? 7 : 0, 7].didMove && !board.checkIfKingIsInCheck(false, isWhite))
                    {
                        //if something is on the way or square is threatended
                        if (board.grid[xOld, yOld + 1] != null || board.checkIfSquareIsThreatened(xOld, yOld + 1))
                        {
                            if (displayReason)
                                Console.WriteLine("Can't castle. A piece is in the way or square is threatened.");
                            return false;
                        }
                        if (board.grid[xOld, yOld + 2] != null || board.checkIfSquareIsThreatened(xOld, yOld + 2))
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
                if (board.squareIsOpponent(xNew, yNew) || board.squareIsEmpty(xNew, yNew))
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
                        if (board.squareIsOpponent(xNew, yNew))
                            return true;
                        //if attacking empty square
                        if (board.squareIsEmpty(xNew, yNew))
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
                        if (board.squareIsEmpty(xNew, yNew))
                            return true;
                        if (board.squareIsOpponent(xNew, yNew))
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
                        if (board.squareIsEmpty(xOld + xStep, yOld))
                        {
                            // if final square is empty
                            if (board.squareIsEmpty(xNew, yNew))
                                return true;
                            // if final square has opponent
                            if (board.squareIsOpponent(xNew, yNew))
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
        }
        class Board
        {
            bool canWhitePotentiallyCastleLong=true, canWhitePotentiallyCastleShort=true, canBlackPotentiallyCastleLong=true, canBlackPotentiallyCastleShort=true;
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
            public void moveRookToCompleteCastling(bool isReverse)
            {
                if (yOld - yNew == 2)
                {
                    grid[(getCurrentTurn() ? 7 : 0), (isReverse ? 0 : 3)] = grid[(getCurrentTurn() ? 7 : 0), (isReverse?3:0)];
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
                        if (checkIfKingIsInCheck(false, getCurrentTurn()))
                        {
                            reverseLastMove(xOld, yOld, xNew, yNew);
                            SetEnPassantProperties(enPassantSquareXBeforeMove, enPassantSquareYBeforeMove, enPassantIsAvailableBeforeMove);
                            Console.WriteLine("Illegal move. Your king will be in check.");
                            continue;
                        }
                        // check king move and change move status of other rook to prevent 3 fold errors
                        if (grid[xNew, yNew] is King)
                        {
                            if (grid[(grid[xNew, yNew].isWhite ? 7 : 0), 0] != null)
                                grid[(grid[xNew, yNew].isWhite ? 7 : 0), 0].didMove = true;
                            if (grid[(grid[xNew, yNew].isWhite ? 7 : 0), 7] != null)
                                grid[(grid[xNew, yNew].isWhite ? 7 : 0), 7].didMove = true;
                        }
                        // check if both rooks moved, and change king move if both true
                        if (grid[xNew, yNew] is Rook)
                        {
                            if (((grid[(grid[xNew, yNew].isWhite ? 7 : 0), 0] != null && grid[(grid[xNew, yNew].isWhite ? 7 : 0), 0].didMove == true) || grid[(grid[xNew, yNew].isWhite ? 7 : 0), 0] == null) && ((grid[(grid[xNew, yNew].isWhite ? 7 : 0), 7] != null && grid[(grid[xNew, yNew].isWhite ? 7 : 0), 7].didMove == true) || grid[(grid[xNew, yNew].isWhite ? 7 : 0), 7] == null))
                                if (grid[(grid[xNew, yNew].isWhite ? 7 : 0), 4] != null)
                                    grid[(grid[xNew, yNew].isWhite ? 7 : 0), 4].didMove = true;
                        }
                        grid[xNew, yNew].didMove = true;
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
                            if (checkIfKingIsInCheck(false, getCurrentTurn()))
                            {
                                Console.WriteLine("Checkmate. {0} wins!", getCurrentTurn() ? "black" : "white");
                                break;
                            }
                            Console.WriteLine("Stalemate.");
                            break;
                        }
                        if (checkLastStringPostisionArray())
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
                        checkIfKingIsInCheck(true, getCurrentTurn());
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
            public bool squareIsOpponent(int x, int y)
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
            public bool squareIsEmpty(int x, int y)
            {
                if (grid[x, y] == null)
                    return true;
                return false;
            }
            public void MovePiece()
            {
                grid[xNew, yNew] = grid[xOld, yOld];
                grid[xOld, yOld] = null;
            }
            public void makeMove(int xOld, int yOld, int xNew, int yNew)
            {
                isEnPassantAvailiable = false;
                UpdateLastRemovedPiece(xNew, yNew);
                MovePiece();
                //added stuff for en passant
                //if is pawn
                if (grid[xOld, yOld] is Pawn)
                {
                    //if did a double move
                    if (xOld - xNew == (grid[xOld, yOld].isWhite ? 2 : -2))
                        SetEnPassantProperties(xNew + (grid[xOld, yOld].isWhite ? 1 : -1), yNew, true);
                    //if made attacking move(sideways) and attacked empty square - means it's en passant
                    else if (Math.Abs(yNew - yOld) == 1 && grid[xNew, yNew] == null)
                    {
                        UpdateLastRemovedPiece(xNew + (grid[xOld, yOld].isWhite ? 1 : -1), yNew);
                        grid[xNew + (grid[xOld, yOld].isWhite ? 1 : -1), yNew] = null;
                    }
                }
                else if (grid[xOld, yOld] is King)
                {
                    UpdateKingPosition(xNew,yNew);
                    if (grid[xNew, yNew] is King && Math.Abs(yOld - yNew) == 2)
                        moveRookToCompleteCastling(false);
                }
            }
            public void reverseLastMove(int xOld, int yOld, int xNew, int yNew)
            {
                //if is pawn and if made attacking move(sideways) and attacked different square from last piece location - means it's an passant
                if (grid[xNew, yNew] is Pawn && Math.Abs(yNew - yOld) == 1&& !(xNew == lastRemovedPieceX && yNew == lastRemovedPieceY))
                {
                        isEnPassantAvailiable = true;
                        grid[xNew + (grid[xNew, yNew].isWhite ? 1 : -1), yNew] = lastRemovedPiece;
                        grid[xOld, yOld] = grid[xNew, yNew];
                        grid[xNew, yNew] = null;
                        return;
                }
                else if (grid[xOld, yOld] is King)
                {
                    UpdateKingPosition(xOld, yOld);
                    if (grid[xNew, yNew] is King && Math.Abs(yOld - yNew) == 2)
                        moveRookToCompleteCastling(true);
                }
                grid[xOld, yOld] = grid[xNew, yNew];
                grid[xNew, yNew] = lastRemovedPiece;
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
                // add en passant state
                positionsArray[positionsArrayIndex] += "" + (this.isEnPassantAvailiable ? 'y' : 'n');
                // add kings states
                if (grid[7, 4] != null && grid[7, 4] is King && !grid[7, 4].didMove)
                    positionsArray[positionsArrayIndex] += "" + 'n';
                else
                    positionsArray[positionsArrayIndex] += "" + 'y';
                if (grid[0, 4] != null && grid[0, 4] is King && !grid[0, 4].didMove)
                    positionsArray[positionsArrayIndex] += "" + 'n';
                else
                    positionsArray[positionsArrayIndex] += "" + 'y';
                // add rooks states
                if (grid[7, 0] != null && grid[7, 0].isWhite && !grid[7, 0].didMove)
                    positionsArray[positionsArrayIndex] += "" + 'n';
                else
                    positionsArray[positionsArrayIndex] += "" + 'y';
                if (grid[0, 0] != null && !grid[0, 0].isWhite && !grid[0, 0].didMove)
                    positionsArray[positionsArrayIndex] += "" + 'n';
                else
                    positionsArray[positionsArrayIndex] += "" + 'y';
                if (grid[7, 7] != null && grid[7, 7].isWhite && !grid[7, 7].didMove)
                    positionsArray[positionsArrayIndex] += "" + 'n';
                else
                    positionsArray[positionsArrayIndex] += "" + 'y';
                if (grid[0, 7] != null && !grid[0, 7].isWhite && !grid[0, 7].didMove)
                    positionsArray[positionsArrayIndex] += "" + 'n';
                else
                    positionsArray[positionsArrayIndex] += "" + 'y';
                // add which turn
                positionsArray[positionsArrayIndex] += (getCurrentTurn() ? 'w' : 'b');
                // add to posision index
                positionsArrayIndex++;
            }
            public bool checkLastStringPostisionArray()
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
            public bool checkIfKingIsInCheck(bool displayReason, bool isWhite)
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
            public bool checkIfSquareIsThreatened(int x, int y)
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
                        // if there is a piece on square
                        if (grid[i, j] != null && this.grid[i, j].isWhite == isWhite)
                            // check every possible square
                            for (int k = 0; k < 8; k++)
                                for (int l = 0; l < 8; l++)
                                    // if legal move
                                    if (grid[i, j].IsLegalMove(false, i, j, k, l))
                                    {
                                        makeMove(i, j, k, l);
                                        // if king is not in check
                                        if (!checkIfKingIsInCheck(false, isWhite))
                                        {
                                            reverseLastMove(i, j, k, l);
                                            isEnPassantAvailiable = enPassantState; // not sure 100% - meanwhile no bugs found
                                            lastRemovedPieceX = lastPieceX;
                                            lastRemovedPieceY = lastPieceY;
                                            lastRemovedPiece = grid[lastPieceX, lastPieceY];
                                            return true;
                                        }
                                        reverseLastMove(i, j, k, l);
                                        isEnPassantAvailiable = enPassantState; // not sure 100 % -meanwhile no bugs found
                                        lastRemovedPieceX = lastPieceX;
                                        lastRemovedPieceY = lastPieceY;
                                        lastRemovedPiece = grid[lastPieceX, lastPieceY];
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