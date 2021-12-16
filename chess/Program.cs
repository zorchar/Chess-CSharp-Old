using System;
namespace chess
{
    class ChessGameLauncher
    {
        static void Main()
        {
            new ChessGame().PlayGame();
        }
    }
    class UtilityMethod
    {
        public static bool IsValidInput(string input)
        {
            input = input.Trim();
            if (input.Length != 4)
                return false;
            if (!((input[0] >= 'A' && input[0] <= 'H') || (input[0] >= 'a' && input[0] <= 'h')))
                return false;
            if (!((input[1] >= '1' && input[1] <= '8')))
                return false;
            if (!((input[2] >= 'A' && input[2] <= 'H') || (input[2] >= 'a' && input[2] <= 'h')))
                return false;
            if (!((input[3] >= '1' && input[3] <= '8')))
                return false;
            return true;
        }
        public static string ProcessInputIntoUsableFormat(string input)
        {
            input = input.Trim();
            string changedInput = "";
            changedInput += '8' - input[1];
            if (input[0] >= 'A' && input[0] <= 'H')
                changedInput += (input[0] - 'A');
            else if (input[0] >= 'a' && input[0] <= 'h')
                changedInput += (input[0] - 'a');
            changedInput += '8' - input[3];
            if (input[2] >= 'A' && input[2] <= 'H')
                changedInput += (input[2] - 'A');
            else if (input[2] >= 'a' && input[2] <= 'h')
                changedInput += (input[2] - 'a');
            return changedInput;
        }
    }
    class Piece
    {
        readonly bool isWhite;
        readonly ChessGame game;
        public Piece() { }
        public Piece(ChessGame game, bool isWhite)
        {
            this.game = game;
            this.isWhite = isWhite;
        }
        public virtual bool IsLegalMove(bool displayReason, int xOld, int yOld, int xNew, int yNew)
        { return true; }
        public bool GetIsWhite() { return isWhite; }
        public ChessGame GetGame() { return game; }
    }
    class Knight : Piece
    {
        public Knight(ChessGame game, bool isWhite) : base(game, isWhite) { }
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
            if (GetGame().IsSquareOpponent(xOld, yOld, xNew, yNew) || GetGame().IsSquareEmpty(xNew, yNew))
                return true;
            if (displayReason)
                Console.WriteLine("Illegal move. Can't take own piece.");
            return false;
        }
        public override string ToString()
        {
            if (GetIsWhite())
                return "N";
            else
                return "n";
        }
    }
    class Rook : Piece
    {
        public Rook(ChessGame game, bool isWhite) : base(game, isWhite) { }
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
                if (!GetGame().IsSquareEmpty(i, j))
                {
                    if (displayReason)
                        Console.WriteLine("Illegal move. A piece is blocking the way.");
                    return false;
                }
                i += xStep;
                j += yStep;
            }
            // check if square is empty or has rival
            if (GetGame().IsSquareOpponent(xOld, yOld, xNew, yNew) || GetGame().IsSquareEmpty(xNew, yNew))
                return true;
            if (displayReason)
                Console.WriteLine("Illegal move. Can't take own piece.");
            return false;
        }
        public override string ToString()
        {
            if (GetIsWhite())
                return "R";
            else
                return "r";
        }
    }
    class Bishop : Piece
    {
        public Bishop(ChessGame game, bool isWhite) : base(game, isWhite) { }
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
                if (!GetGame().IsSquareEmpty(i, j))
                {
                    if (displayReason)
                        Console.WriteLine("Illegal move. A piece is blocking the way.");
                    return false;
                }
                i += xStep;
                j += yStep;
            }
            // check if square is empty or has rival - make function in Piece
            if (GetGame().IsSquareOpponent(xOld, yOld, xNew, yNew) || GetGame().IsSquareEmpty(xNew, yNew))
                return true;
            if (displayReason)
                Console.WriteLine("Illegal move. Can't take own piece.");
            return false;
        }
        public override string ToString()
        {
            if (GetIsWhite())
                return "B";
            else
                return "b";
        }
    }
    class Queen : Piece
    {
        public Queen(ChessGame game, bool isWhite) : base(game, isWhite) { }
        public override bool IsLegalMove(bool displayReason, int xOld, int yOld, int xNew, int yNew)
        {
            Bishop queenAsBishop = new Bishop(GetGame(), GetIsWhite());
            Rook queenAsRook = new Rook(GetGame(), GetIsWhite());
            if (queenAsBishop.IsLegalMove(false, xOld, yOld, xNew, yNew) || queenAsRook.IsLegalMove(false, xOld, yOld, xNew, yNew))
                return true;
            if (displayReason)
                Console.WriteLine("Queen can't move there.");
            return false;
        }
        public override string ToString()
        {
            if (GetIsWhite())
                return "Q";
            else
                return "q";
        }
    }
    class King : Piece
    {
        public King(ChessGame game, bool isWhite, int x, int y) : base(game, isWhite)
        {
            if (isWhite)
            {
                game.SetXWhiteKing(x);
                game.SetYWhiteKing(y);
            }
            else
            {
                game.SetXBlackKing(x);
                game.SetYBlackKing(y);
            }
        }
        public override bool IsLegalMove(bool displayReason, int xOld, int yOld, int xNew, int yNew)
        {
            // if king wants to castle long (to move two squares left)
            if ((yOld - yNew == 2 && xOld == xNew))
            {
                // if can potentially castle long and not in check
                if (GetGame().GetBoard()[GetIsWhite() ? 7 : 0, 0] != null && !GetGame().IsKingInCheck(false, GetIsWhite()) && (GetIsWhite() ? GetGame().GetCanWhitePotentiallyCastleLong() : GetGame().GetCanBlackPotentiallyCastleLong()))
                {
                    //if something is on the way or square is threatened
                    if (GetGame().GetBoard()[xOld, yOld - 1] != null || GetGame().IsSquareThreatened(xOld, yOld - 1))
                    {
                        if (displayReason)
                            Console.WriteLine("Can't castle. A piece is in the way or square is threatened.");
                        return false;
                    }
                    if (GetGame().GetBoard()[xOld, yOld - 2] != null || GetGame().IsSquareThreatened(xOld, yOld - 2))
                    {
                        if (displayReason)
                            Console.WriteLine("Can't castle. A piece is in the way or square is threatened.");
                        return false;
                    }
                    if (GetGame().GetBoard()[xOld, yOld - 3] != null)
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
                if (GetGame().GetBoard()[GetIsWhite() ? 7 : 0, 7] != null && !GetGame().IsKingInCheck(false, GetIsWhite()) && (GetIsWhite() ? GetGame().GetCanWhitePotentiallyCastleShort() : GetGame().GetCanBlackPotentiallyCastleShort()))
                {
                    //if something is on the way or square is threatended
                    if (GetGame().GetBoard()[xOld, yOld + 1] != null || GetGame().IsSquareThreatened(xOld, yOld + 1))
                    {
                        if (displayReason)
                            Console.WriteLine("Can't castle. A piece is in the way or square is threatened.");
                        return false;
                    }
                    if (GetGame().GetBoard()[xOld, yOld + 2] != null || GetGame().IsSquareThreatened(xOld, yOld + 2))
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
            if (GetGame().IsSquareOpponent(xOld, yOld, xNew, yNew) || GetGame().IsSquareEmpty(xNew, yNew))
                return true;
            if (displayReason)
                Console.WriteLine("Illegal move. Can't take own piece.");
            return false;
        }
        public override string ToString()
        {
            if (GetIsWhite())
                return "K";
            else
                return "k";
        }
    }
    class Pawn : Piece
    {
        public Pawn(ChessGame game, bool isWhite) : base(game, isWhite) { }
        public override bool IsLegalMove(bool displayReason, int xOld, int yOld, int xNew, int yNew)
        {
            int xStep = 1;
            if (GetIsWhite())
                xStep = -1;
            // if walked one square up
            if (xNew - xOld == xStep)
            {
                // if sideways
                if (Math.Abs(yNew - yOld) == 1)
                {
                    if (GetGame().IsSquareOpponent(xOld, yOld, xNew, yNew))
                        return true;
                    //if attacking empty square
                    if (GetGame().IsSquareEmpty(xNew, yNew))
                    {
                        // if en passant availiable and pawn is attacking it
                        if (GetGame().GetIsEnPassantAvailiable() && xNew == GetGame().GetEnPassantSquareX() && yNew == GetGame().GetEnPassantSquareY())
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
                    if (GetGame().IsSquareEmpty(xNew, yNew))
                        return true;
                    if (GetGame().IsSquareOpponent(xOld, yOld, xNew, yNew))
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
                if (((GetIsWhite() ? 6 : 1) == xOld))
                {
                    //if square on the way is empty
                    if (GetGame().IsSquareEmpty(xOld + xStep, yOld))
                    {
                        // if final square is empty
                        if (GetGame().IsSquareEmpty(xNew, yNew))
                            return true;
                        // if final square has opponent
                        if (GetGame().IsSquareOpponent(xOld, yOld, xNew, yNew))
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
            if (GetIsWhite())
                return "P";
            else
                return "p";
        }
    }
    class ChessGame
    {
        bool canWhitePotentiallyCastleLong = true, canWhitePotentiallyCastleShort = true, canBlackPotentiallyCastleLong = true, canBlackPotentiallyCastleShort = true;
        string input;
        int enPassantSquareX, enPassantSquareY, lastRemovedPieceX, lastRemovedPieceY, xWhiteKing, yWhiteKing, xBlackKing, yBlackKing, xOld, yOld, xNew, yNew, positionsArrayIndex = 0;
        bool isEnPassantAvailiable = false, isCurrentTurnWhite = true;
        Piece lastRemovedPiece;
        string[] positionsArray = new string[101];
        readonly Piece[,] board = new Piece[8, 8];
        public ChessGame()
        {
            board[0, 0] = new Rook(this, false);
            board[0, 1] = new Knight(this, false);
            board[0, 2] = new Bishop(this, false);
            board[0, 3] = new Queen(this, false);
            board[0, 4] = new King(this, false, 0, 4);
            board[0, 5] = new Bishop(this, false);
            board[0, 6] = new Knight(this, false);
            board[0, 7] = new Rook(this, false);
            board[7, 0] = new Rook(this, true);
            board[7, 1] = new Knight(this, true);
            board[7, 2] = new Bishop(this, true);
            board[7, 3] = new Queen(this, true);
            board[7, 4] = new King(this, true, 7, 4);
            board[7, 5] = new Bishop(this, true);
            board[7, 6] = new Knight(this, true);
            board[7, 7] = new Rook(this, true);
            for (int j = 0; j < 8; j++)
                board[1, j] = new Pawn(this, false);
            for (int j = 0; j < 8; j++)
                board[6, j] = new Pawn(this, true);
        }
        public Piece[,] GetBoard() { return board; }
        public bool GetCanWhitePotentiallyCastleLong() { return canWhitePotentiallyCastleLong; }
        public bool GetCanWhitePotentiallyCastleShort() { return canWhitePotentiallyCastleShort; }
        public bool GetCanBlackPotentiallyCastleLong() { return canBlackPotentiallyCastleLong; }
        public bool GetCanBlackPotentiallyCastleShort() { return canBlackPotentiallyCastleShort; }
        public int GetEnPassantSquareX() { return enPassantSquareX; }
        public int GetEnPassantSquareY() { return enPassantSquareY; }
        public bool GetIsEnPassantAvailiable() { return isEnPassantAvailiable; }
        public void SetXWhiteKing(int x) { xWhiteKing = x; }
        public void SetYWhiteKing(int y) { yWhiteKing = y; }
        public void SetXBlackKing(int x) { xBlackKing = x; }
        public void SetYBlackKing(int y) { yBlackKing = y; }
        public void ParseInputIntoAppropriateVariables()
        {
            xOld = int.Parse("" + input[0]);
            yOld = int.Parse("" + input[1]);
            xNew = int.Parse("" + input[2]);
            yNew = int.Parse("" + input[3]);
        }
        public void MoveRookToCompleteOrReverseCastling(bool isReverse)
        {
            if (yOld - yNew == 2)
            {
                board[(isCurrentTurnWhite ? 7 : 0), (isReverse ? 0 : 3)] = board[(isCurrentTurnWhite ? 7 : 0), (isReverse ? 3 : 0)];
                board[(isCurrentTurnWhite ? 7 : 0), (isReverse ? 3 : 0)] = null;
            }
            else if (yOld - yNew == -2)
            {
                board[(isCurrentTurnWhite ? 7 : 0), (isReverse ? 7 : 5)] = board[(isCurrentTurnWhite ? 7 : 0), (isReverse ? 5 : 7)];
                board[(isCurrentTurnWhite ? 7 : 0), (isReverse ? 5 : 7)] = null;
            }
        }
        public void UpdateKingPosition(int x, int y)
        {
            if (isCurrentTurnWhite)
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
            lastRemovedPiece = board[x, y];
            lastRemovedPieceX = x;
            lastRemovedPieceY = y;
        }
        public void UpdatePotentialCastlingRights()
        {
            // check king move and change move status of other rook to prevent 3 fold errors
            if (board[xNew, yNew] is King)
            {
                if (isCurrentTurnWhite)
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
            if (board[xNew, yNew] is Rook)
            {
                if (isCurrentTurnWhite)
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
            // check if took rook while didn't move yet. for positions bugs
            if (xNew == 0 && yNew == 0)
                canBlackPotentiallyCastleLong = false;
            else if (xNew == 0 && yNew == 7)
                canBlackPotentiallyCastleShort = false;
            else if (xNew == 7 && yNew == 0)
                canWhitePotentiallyCastleLong = false;
            else if (xNew == 7 && yNew == 7)
                canWhitePotentiallyCastleShort = false;
        }
        public void PromptUntilInputIsLegal()
        {
            while (true)
            {
                Console.Write("Enter move: ");
                input = Console.ReadLine();
                Console.WriteLine();
                if (!UtilityMethod.IsValidInput(input))
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }
                input = UtilityMethod.ProcessInputIntoUsableFormat(input);
                ParseInputIntoAppropriateVariables();
                if (board[xOld, yOld] == null)
                {
                    Console.WriteLine("Invalid move. No piece on square.");
                    continue;
                }
                else if (board[xOld, yOld].GetIsWhite() != isCurrentTurnWhite)
                {
                    Console.WriteLine("Illegal move. Can't move opponent's pieces.");
                    continue;
                }
                return;
            }
        }
        public bool IsGameOver()
        {
            if (!AreThereAnyLegalMoves(isCurrentTurnWhite, isEnPassantAvailiable, lastRemovedPieceX, lastRemovedPieceY))
            {
                if (IsKingInCheck(false, isCurrentTurnWhite))
                {
                    Console.WriteLine("Checkmate. {0} wins!", isCurrentTurnWhite ? "black" : "white");
                    return true;
                }
                Console.WriteLine("Stalemate.");
                return true;
            }
            if (IsThreeFoldRepetition())
            {
                Console.WriteLine("Draw. Threefold repetition.");
                return true;
            }
            if (positionsArray[100] != null)
            {
                Console.WriteLine("Draw. Fifty moves without pawn move or taking a piece.");
                return true;
            }
            if (IsInsufficientMaterial())
            {
                Console.WriteLine("Draw. Too few pieces to win.");
                return true;
            }
            return false;
        }
        public void PlayGame()
        {
            PrintBoard();
            AddPositionStringToPositionsArray();
            while (true)
            {
                PromptUntilInputIsLegal();
                if (board[xOld, yOld].IsLegalMove(true, xOld, yOld, xNew, yNew))
                {
                    bool enPassantIsAvailableBeforeMove = isEnPassantAvailiable;
                    int enPassantSquareXBeforeMove = GetEnPassantSquareX(), enPassantSquareYBeforeMove = GetEnPassantSquareY();
                    MakeMove(xOld, yOld, xNew, yNew);
                    if (IsKingInCheck(false, isCurrentTurnWhite))
                    {
                        ReverseLastMove(xOld, yOld, xNew, yNew);

                        SetEnPassantProperties(enPassantSquareXBeforeMove,
                        enPassantSquareYBeforeMove, enPassantIsAvailableBeforeMove);
                        Console.WriteLine("Illegal move. Your king will be in check.");
                        continue;
                    }
                    UpdatePotentialCastlingRights();
                    if (board[xNew, yNew] is Pawn && (xNew == 7 || xNew == 0))
                    {
                        Promote(xNew, yNew);
                    }
                    isCurrentTurnWhite = !isCurrentTurnWhite;
                    PrintBoard();
                    if (lastRemovedPiece != null || board[xNew, yNew] is Pawn)
                    {
                        positionsArray = new string[101];
                        positionsArrayIndex = 0;
                    }
                    AddPositionStringToPositionsArray();
                    if (IsGameOver())
                        break;
                    IsKingInCheck(true, isCurrentTurnWhite);
                }
            }
            Console.ReadLine();
        }
        public bool IsInsufficientMaterial()
        {
            int knightCount = 0, bishopCount = 0, whiteBishopCountBlackSquare = 0, whiteBishopCountWhiteSquare = 0, blackBishopCountBlackSquare = 0, blackBishopCountWhiteSquare = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] is Pawn || board[i, j] is Rook || board[i, j] is Queen)
                        return false;
                    if (board[i, j] is Bishop)
                    {
                        bishopCount++;
                        if (board[i, j].GetIsWhite())
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
                    else if (board[i, j] is Knight)
                        knightCount++;
                }
            }
            if (knightCount == 1 && bishopCount == 0 || knightCount == 0 && bishopCount == 1)
                return true;
            if (knightCount == 0 && ((blackBishopCountBlackSquare == 1 && whiteBishopCountBlackSquare == 1) || (blackBishopCountWhiteSquare == 1 && whiteBishopCountWhiteSquare == 1)))
                return true;
            return false;
        }
        public void Promote(int xNew, int yNew)
        {
                Console.WriteLine("Which piece to promote to ?\n1.knight\n2.bishop\n3.rook\nelse.queen");
                string whichPiece = Console.ReadLine();
                if (whichPiece == "1")
                    board[xNew, yNew] = new Knight(this, isCurrentTurnWhite);
                else if (whichPiece == "2")
                    board[xNew, yNew] = new Bishop(this, isCurrentTurnWhite);
                else if (whichPiece == "3")
                    board[xNew, yNew] = new Rook(this, isCurrentTurnWhite);
                else
                    board[xNew, yNew] = new Queen(this, isCurrentTurnWhite);
        }
        public bool IsSquareOpponent(int xOld, int yOld, int xNew, int yNew)
        {
            if (board[xNew, yNew] != null && board[xNew, yNew].GetIsWhite() != board[xOld, yOld].GetIsWhite())
                return true;
            return false;
        }
        public void PrintBoard()
        {
            Console.WriteLine();
            int k = 8;
            for (int i = 0; i < 8; i++)
            {
                Console.Write(k + " ");
                k--;
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] != null)
                    {
                        Console.Write(" {0}", board[i, j].GetIsWhite() ? "W" : "B");
                        Console.Write((board[i, j].ToString() + " ").ToUpper());
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
            if (board[x, y] == null)
                return true;
            return false;
        }
        public void MakeMove(int xOld, int yOld, int xNew, int yNew)
        {
            isEnPassantAvailiable = false;
            UpdateLastRemovedPiece(xNew, yNew);
            board[xNew, yNew] = board[xOld, yOld];
            board[xOld, yOld] = null;
            if (board[xNew, yNew] is Pawn)
            {
                //if did a double move
                if (xOld - xNew == (board[xNew, yNew].GetIsWhite() ? 2 : -2))
                    SetEnPassantProperties(xNew + (isCurrentTurnWhite ? 1 : -1), yNew, true);
                //if made attacking move(sideways) and attacked empty square - means it's en passant
                else if (Math.Abs(yNew - yOld) == 1 && lastRemovedPiece == null)
                {
                    UpdateLastRemovedPiece(xNew + (isCurrentTurnWhite ? 1 : -1), yNew);
                    board[xNew + (isCurrentTurnWhite ? 1 : -1), yNew] = null;
                }
            }
            else if (board[xNew, yNew] is King)
            {
                UpdateKingPosition(xNew, yNew);
                if (board[xNew, yNew] is King && Math.Abs(yOld - yNew) == 2)
                    MoveRookToCompleteOrReverseCastling(false);
            }
        }
        public void ReverseLastMove(int xOld, int yOld, int xNew, int yNew)
        {
            board[xOld, yOld] = board[xNew, yNew];
            board[xNew, yNew] = lastRemovedPiece;
            //if is pawn and if made attacking move(sideways) and attacked different square from last piece location - means it's an passant
            if (board[xOld, yOld] is Pawn && Math.Abs(yNew - yOld) == 1 && !(xNew == lastRemovedPieceX && yNew == lastRemovedPieceY))
            {
                isEnPassantAvailiable = true;
                board[xNew + (board[xNew, yNew].GetIsWhite() ? -1 : 1), yNew] = lastRemovedPiece;
                lastRemovedPiece = null;
                board[xNew, yNew] = null;
            }
            else if (board[xOld, yOld] is King)
            {
                UpdateKingPosition(xOld, yOld);
                if (board[xNew, yNew] is King && Math.Abs(yOld - yNew) == 2)
                    MoveRookToCompleteOrReverseCastling(true);
            }
        }
        public void AddPositionStringToPositionsArray()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] != null)
                        positionsArray[positionsArrayIndex] += "" + board[i, j].ToString();
                    else
                        positionsArray[positionsArrayIndex] += " ";
                }
            positionsArray[positionsArrayIndex] += (isEnPassantAvailiable ? 'y' : 'n');
            positionsArray[positionsArrayIndex] += (canBlackPotentiallyCastleLong ? 'y' : 'n');
            positionsArray[positionsArrayIndex] += (canBlackPotentiallyCastleShort ? 'y' : 'n');
            positionsArray[positionsArrayIndex] += (canWhitePotentiallyCastleLong ? 'y' : 'n');
            positionsArray[positionsArrayIndex] += (canWhitePotentiallyCastleShort ? 'y' : 'n');
            positionsArray[positionsArrayIndex] += (isCurrentTurnWhite ? 'w' : 'b');
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
                    if (board[i, j] != null && board[i, j].GetIsWhite() != isCurrentTurnWhite && board[i, j].IsLegalMove(false, i, j, isWhite ? xWhiteKing : xBlackKing, isWhite ? yWhiteKing : yBlackKing))
                    {
                        if (displayReason)
                            Console.WriteLine("{0} is in check.", isCurrentTurnWhite ? "white" : "black");
                        return true;
                    }
            return false;
        }
        public bool IsSquareThreatened(int x, int y)
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (this.board[i, j] != null && board[i, j].GetIsWhite() != isCurrentTurnWhite && this.board[i, j].IsLegalMove(false, i, j, x, y))
                        return true;
            return false;
        }
        public bool AreThereAnyLegalMoves(bool isWhite, bool enPassantState, int lastPieceX, int lastPieceY)
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (board[i, j] != null && this.board[i, j].GetIsWhite() == isWhite)
                        for (int k = 0; k < 8; k++)
                            for (int l = 0; l < 8; l++)
                                if (board[i, j].IsLegalMove(false, i, j, k, l))
                                {
                                    MakeMove(i, j, k, l);
                                    if (!IsKingInCheck(false, isWhite))
                                    {
                                        ReverseLastMove(i, j, k, l);
                                        isEnPassantAvailiable = enPassantState;

                                        UpdateLastRemovedPiece(lastPieceX, lastPieceY);
                                        return true;
                                    }
                                    ReverseLastMove(i, j, k, l);
                                    isEnPassantAvailiable = enPassantState;
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
