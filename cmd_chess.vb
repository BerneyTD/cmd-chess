Module Module1
    Dim board(11, 11), answer As String
    Dim currHor, currVer As Integer
    Dim destHor, destVer As Integer
    Dim turn As String
    Dim currHorStr, destHorStr As String
    Dim moveable, colour, inCheck, inCheckmate As Boolean 'Colour: False = White, True = Black
    Dim UpGrave, LoGrave As String
    Dim Wcastle, Bcastle As String
    Dim castleW, castleB, Castling, castleRightMaybe, castleLeftMaybe As Boolean

    'When it works, do FEN thing (ask Dungles)

    Sub Main()
        castleW = True
        castleB = True
        Console.WriteLine("How to play:")
        Console.WriteLine("Type in the coordinate of the piece you want to move and where you want to move it.")
        Console.WriteLine("e.g:")
        Console.WriteLine("b2 b3")
        Console.WriteLine("This would (at the start of the game) move the pawn at b,2 to b,3")
        Console.WriteLine("Upper case goes first (they're white) lower case second (they're black)")
        Console.WriteLine("")
        Console.WriteLine("At any point, type 'FEN' to bring up the FEN menu.")
        Console.WriteLine("Press enter to continue.")
        Console.ReadKey()
        Console.WriteLine("GAMEMODE                    | NUMBER")
        Console.WriteLine("-------------------------------------")
        Console.WriteLine("Standard Chess              |   1")
        Console.WriteLine("Charge of the Light Brigade |   2")
        Console.WriteLine("Dunsay's Chess              |   3")
        Console.WriteLine("Legal's Game                |   4")
        Console.WriteLine("Peasant's Revolt            |   5")
        Console.WriteLine("Weak!                       |   6")
        Console.WriteLine("")
        Console.WriteLine("Choose a gamemode by entering its corresponding number.")
        answer = Console.ReadLine().Chars(0)
        Select Case answer
            Case "1" : FEN_load("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -")
            Case "2" : FEN_load("nnnnknnn/pppppppp/8/8/8/8/PPPPPPPP/1Q1QK1Q1 w ---- -")
            Case "3" : FEN_load("rnbqkbnr/pppppppp/8/8/PPPPPPPP/PPPPPPPP/PPPPPPPP/PPPPPPPP w ---- -")
            Case "4" : FEN_load("rnbqkbnr/pppppppp/8/8/2PPPP2/1PP2PP1/PPPPPPPP/RNB1KBNR w KQkq -")
            Case "5" : FEN_load("1nn1knn1/4p3/8/8/8/8/PPPPPPPP/4K3 w ---- -")
            Case "6" : FEN_load("nnnnknnn/pppppppp/2p2p2/1pppppp1/8/8/PPPPPPPP/RNBQKBNR w KQkq -")
        End Select
        colour = False
        turn = "White"
        While True
            If castleW = True Then : Wcastle = "can" : Else : Wcastle = "can't" : End If
            If castleB = True Then : Bcastle = "can" : Else : Bcastle = "can't" : End If
            displayBoard()
            answer = Console.ReadLine()
            If answer = "FEN" Then
                Console.WriteLine("Please enter one of the following:")
                Console.WriteLine("  LOAD:         Load a FEN string for a game.")
                Console.WriteLine("  SAVE:         Recieve the FEN string for the current game.")
                Console.WriteLine(" CLOSE:         Close this menu.")
                answer = Console.ReadLine()
                If answer.ToLower() = "load" Then
                    Console.WriteLine("Please enter the FEN string you wish to load:")
                    FEN_load(Console.ReadLine())
                End If
                If answer.ToLower() = "save" Then
                    FEN_save()
                End If
            Else
                processAnswer()
                move()
                If board(destVer, destHor) Is "P " And destVer = 9 Then
                    Console.WriteLine("Please input which piece you would like to promote to:")
                    Console.WriteLine("Q for queen; R for rook; N for knight; B for bishop")
                    answer = Console.ReadLine()
                    Select Case answer.Chars(0)
                        Case "Q" : board(destVer, destHor) = "Q "
                        Case "R" : board(destVer, destHor) = "R "
                        Case "N" : board(destVer, destHor) = "N "
                        Case "B" : board(destVer, destHor) = "B "
                    End Select
                End If
                If board(destVer, destHor) Is "p " And destVer = 2 Then
                    Console.WriteLine("Please input which piece you would like to promote to:")
                    Console.WriteLine("q for queen; r for rook; n for knight; b for bishop")
                    answer = Console.ReadLine()
                    Select Case answer.Chars(1)
                        Case "q" : board(destVer, destHor) = "q "
                        Case "r" : board(destVer, destHor) = "r "
                        Case "n" : board(destVer, destHor) = "n "
                        Case "b" : board(destVer, destHor) = "b "
                    End Select
                End If
            End If
        End While
    End Sub

    Sub displayBoard()
        Console.WriteLine("8|| " & board(9, 2) & " " & board(9, 3) & " " & board(9, 4) & " " & board(9, 5) & " " & board(9, 6) & " " & board(9, 7) & " " & board(9, 8) & " " & board(9, 9) & " ")
        Console.WriteLine("7|| " & board(8, 2) & " " & board(8, 3) & " " & board(8, 4) & " " & board(8, 5) & " " & board(8, 6) & " " & board(8, 7) & " " & board(8, 8) & " " & board(8, 9) & " ")
        Console.WriteLine("6|| " & board(7, 2) & " " & board(7, 3) & " " & board(7, 4) & " " & board(7, 5) & " " & board(7, 6) & " " & board(7, 7) & " " & board(7, 8) & " " & board(7, 9) & " " & "     Turn: " & turn)
        Console.WriteLine("5|| " & board(6, 2) & " " & board(6, 3) & " " & board(6, 4) & " " & board(6, 5) & " " & board(6, 6) & " " & board(6, 7) & " " & board(6, 8) & " " & board(6, 9) & " " & " black's Graveyard: " & LoGrave)
        Console.WriteLine("4|| " & board(5, 2) & " " & board(5, 3) & " " & board(5, 4) & " " & board(5, 5) & " " & board(5, 6) & " " & board(5, 7) & " " & board(5, 8) & " " & board(5, 9) & " " & " WHITE's Graveyard: " & UpGrave)
        Console.WriteLine("3|| " & board(4, 2) & " " & board(4, 3) & " " & board(4, 4) & " " & board(4, 5) & " " & board(4, 6) & " " & board(4, 7) & " " & board(4, 8) & " " & board(4, 9) & " ")
        Console.WriteLine("2|| " & board(3, 2) & " " & board(3, 3) & " " & board(3, 4) & " " & board(3, 5) & " " & board(3, 6) & " " & board(3, 7) & " " & board(3, 8) & " " & board(3, 9) & " ")
        Console.WriteLine("1|| " & board(2, 2) & " " & board(2, 3) & " " & board(2, 4) & " " & board(2, 5) & " " & board(2, 6) & " " & board(2, 7) & " " & board(2, 8) & " " & board(2, 9) & " ")
        Console.WriteLine(" \\========================")
        Console.WriteLine("    a  b  c  d  e  f  g  h")
    End Sub

    Sub processAnswer()

        currHorStr = answer.Chars(0)
        currVer = Val(answer.Chars(1)) + 1
        destHorStr = answer.Chars(3)
        destVer = Val(answer.Chars(4)) + 1

        'Converting the horizontal letters into numbers
        currHor = Asc(currHorStr) - 95
        destHor = Asc(destHorStr) - 95

        Console.WriteLine(currHor & "," & currVer & " -> " & destHor & "," & destVer)

        If currHor > 9 OrElse currHor < 2 OrElse currVer > 9 OrElse currVer < 2 OrElse destHor > 9 OrElse destHor < 2 OrElse destVer > 9 OrElse destVer < 2 Then
            Console.WriteLine("Invalid coordinate/input")
        End If
        Console.WriteLine(board(currVer, currHor))

    End Sub

    Sub move()
        moveable = False
        Select Case board(currVer, currHor)
            Case ".."
                Console.WriteLine("There is no piece here!")
            Case "P "
                If (destHor = currHor) And (destVer = currVer + 1) Then
                    moveable = True
                ElseIf currVer = 3 And (destHor = currHor) And (destVer = currVer + 2) Then
                    moveable = True
                ElseIf board(destVer, destHor) Is board(currVer + 1, currHor + 1) OrElse board(destVer, destHor) Is board(currVer + 1, currHor - 1) Then
                    Select Case board(destVer, destHor)
                        Case "..", "P ", "K ", "R ", "N ", "B " : moveable = False
                        Case Else : moveable = True
                    End Select
                Else
                    moveable = False
                End If
            Case "p "
                If (destHor = currHor) And (destVer = currVer - 1) Then
                    moveable = True
                ElseIf currVer = 8 And (destHor = currHor) And (destVer = currVer - 2) Then
                    moveable = True
                ElseIf board(destVer, destHor) Is board(currVer - 1, currHor + 1) OrElse board(destVer, destHor) Is board(currVer - 1, currHor - 1) Then
                    Select Case board(destVer, destHor)
                        Case "..", "p ", "r ", "n ", "b ", "q ", "k " : moveable = False
                        Case Else : moveable = True
                    End Select
                Else
                    moveable = False
                End If
            Case "R ", "r "
                'Vertical Movement
                If destHor = currHor And destVer <> currVer Then
                    'Up
                    If destVer > currVer Then
                        For i = currVer + 1 To destVer - 1
                            If board(i, currHor) IsNot ".." Then
                                moveable = False
                                Exit For
                            Else
                                moveable = True
                            End If
                        Next
                        If destVer - currVer = 1 Then
                            moveable = True
                        End If
                        'Down
                    ElseIf destVer < currVer Then
                        For i = currVer - 1 To destVer + 1
                            If board(i, currHor) IsNot ".." Then
                                moveable = False
                                Exit For
                            Else
                                moveable = True
                            End If
                        Next
                        If currVer - destVer = 1 Then
                            moveable = True
                        End If
                    End If
                    'Horizontal Movement
                ElseIf destVer = currVer And destHor <> currHor Then
                    'Right
                    If destHor > currHor Then
                        castleRightMaybe = True
                        For i = currHor + 1 To destHor - 1
                            If board(currVer, i) IsNot ".." Then
                                moveable = False
                                Exit For
                            Else
                                moveable = True
                            End If
                        Next
                        If destHor - currHor = 1 Then
                            moveable = True
                        End If
                        'Left
                    ElseIf destHor < currHor Then
                        castleLeftMaybe = True
                        For i = currHor - 1 To destHor + 1
                            If board(currVer, i) IsNot ".." Then
                                moveable = False
                                Exit For
                            Else
                                moveable = True
                            End If
                        Next
                        If currHor - destHor = 1 Then
                            moveable = True
                        End If
                    End If
                    'Castling
                    If moveable = True Then
                        If colour = False Then
                            If currVer = 2 And destVer = 2 Then
                                If (destHor = 5 Or destHor = 7) And castleW = True Then : Castling = True : End If
                            End If
                        End If
                        If colour = True Then
                            If currVer = 9 And destVer = 9 Then
                                If (destHor = 5 Or destHor = 7) And castleW = True Then : Castling = True : End If
                            End If
                        End If
                    End If
                End If
                If moveable = True Then
                    If Castling = False Then
                        If colour = False Then
                            castleW = False
                        Else
                            castleB = False
                        End If
                    End If
                End If
            Case "N ", "n "
                If (board(destVer, destHor) Is board(currVer + 1, currHor - 2)) OrElse (board(destVer, destHor) Is board(currVer + 2, currHor - 1)) OrElse (board(destVer, destHor) Is board(currVer + 2, currHor + 1)) OrElse (board(destVer, destHor) Is board(currVer + 1, currHor + 2)) OrElse (board(destVer, destHor) Is board(currVer - 1, currHor + 2)) OrElse (board(destVer, destHor) Is board(currVer - 2, currHor + 1)) OrElse (board(destVer, destHor) Is board(currVer - 2, currHor - 1)) OrElse (board(destVer, destHor) Is board(currVer - 1, currHor - 2)) Then
                    moveable = True
                Else
                    moveable = False
                End If
            Case "B ", "b "
                If currHor < destHor And currVer < destVer Then
                    For i = 1 To (destHor - currHor)
                        If board(currVer + i, currHor + i) IsNot ".." Then
                            moveable = False
                            Exit For
                        Else
                            moveable = True
                        End If
                    Next
                    If board(currVer + 1, currVer + 1) Is board(destVer, destHor) Then
                        moveable = True
                    End If
                ElseIf currHor < destHor And currVer > destVer Then
                    For i = 1 To (destHor - currHor)
                        If board(currVer - i, currHor + i) IsNot ".." Then
                            moveable = False
                            Exit For
                        Else
                            moveable = True
                        End If
                    Next
                    If board(currVer - 1, currVer + 1) Is board(destVer, destHor) Then
                        moveable = True
                    End If
                ElseIf currHor > destHor And currVer < destVer Then
                    For i = 1 To (currHor - destHor)
                        If board(currVer + i, currHor - i) IsNot ".." Then
                            moveable = False
                            Exit For
                        Else
                            moveable = True
                        End If
                    Next
                    If board(currVer + 1, currVer - 1) Is board(destVer, destHor) Then
                        moveable = True
                    End If
                ElseIf currHor > destHor And currVer > destVer Then
                    For i = 1 To (currHor - destHor)
                        If board(currVer - i, currHor - i) IsNot ".." Then
                            moveable = False
                            Exit For
                        Else
                            moveable = True
                        End If
                    Next
                    If board(currVer - 1, currVer - 1) Is board(destVer, destHor) Then
                        moveable = True
                    End If
                End If
            Case "Q ", "q "
                'Vertical Movement
                If destHor = currHor And destVer <> currVer Then
                    'Up
                    If destVer > currVer Then
                        For i = currVer + 1 To destVer - 1
                            If board(i, currHor) IsNot ".." Then
                                moveable = False
                                Exit For
                            Else
                                moveable = True
                            End If
                        Next
                        If destVer - currVer = 1 Then
                            moveable = True
                        End If
                        'Down
                    ElseIf destVer < currVer Then
                        For i = currVer - 1 To destVer + 1
                            If board(i, currHor) IsNot ".." Then
                                moveable = False
                                Exit For
                            Else
                                moveable = True
                            End If
                        Next
                        If currVer - destVer = 1 Then
                            moveable = True
                        End If
                    End If
                    'Horizontal Movement
                ElseIf destVer = currVer And destHor <> currHor Then
                    'Right
                    If destHor > currHor Then
                        For i = currHor + 1 To destHor - 1
                            If board(currVer, i) IsNot ".." Then
                                moveable = False
                                Exit For
                            Else
                                moveable = True
                            End If
                        Next
                        If destHor - currHor = 1 Then
                            moveable = True
                        End If
                    End If
                    'Left
                ElseIf destHor < currHor Then
                    For i = currHor - 1 To destHor + 1
                        If board(currVer, i) IsNot ".." Then
                            moveable = False
                            Exit For
                        Else
                            moveable = True
                        End If
                    Next
                    If currHor - destHor = 1 Then
                        moveable = True
                    End If
                End If
                If currHor < destHor And currVer < destVer Then
                    For i = 1 To (destHor - currHor)
                        If board(currVer + i, currHor + i) IsNot ".." Then
                            moveable = False
                            Exit For
                        Else
                            moveable = True
                        End If
                    Next
                    If board(currVer + 1, currVer + 1) Is board(destVer, destHor) Then
                        moveable = True
                    End If
                ElseIf currHor < destHor And currVer > destVer Then
                    For i = 1 To (destHor - currHor)
                        If board(currVer - i, currHor + i) IsNot ".." Then
                            moveable = False
                            Exit For
                        Else
                            moveable = True
                        End If
                    Next
                    If board(currVer - 1, currVer + 1) Is board(destVer, destHor) Then
                        moveable = True
                    End If
                ElseIf currHor > destHor And currVer < destVer Then
                    For i = 1 To (currHor - destHor)
                        If board(currVer + i, currHor - i) IsNot ".." Then
                            moveable = False
                            Exit For
                        Else
                            moveable = True
                        End If
                    Next
                    If board(currVer + 1, currVer - 1) Is board(destVer, destHor) Then
                        moveable = True
                    End If
                ElseIf currHor > destHor And currVer > destVer Then
                    For i = 1 To (currHor - destHor)
                        If board(currVer - i, currHor - i) IsNot ".." Then
                            moveable = False
                            Exit For
                        Else
                            moveable = True
                        End If
                    Next
                    If board(currVer - 1, currVer - 1) Is board(destVer, destHor) Then
                        moveable = True
                    End If
                End If
            Case "K ", "k "
                'Vertical Movement
                If destHor = currHor And destVer <> currVer Then
                    'Up
                    If destVer > currVer Then
                        If destVer - currVer = 1 Then
                            moveable = True
                        End If
                        'Down
                    ElseIf destVer < currVer Then
                        If currVer - destVer = 1 Then
                            moveable = True
                        End If
                    End If
                    'Horizontal Movement
                ElseIf destVer = currVer And destHor <> currHor Then
                    'Right
                    If destHor > currHor Then
                        If destHor - currHor = 1 Then
                            moveable = True
                        End If
                    End If
                    'Left
                ElseIf destHor < currHor Then
                    If currHor - destHor = 1 Then
                        moveable = True
                    End If
                End If
                If currHor < destHor And currVer < destVer Then
                    If board(currVer + 1, currVer + 1) Is board(destVer, destHor) Then
                        moveable = True
                    End If
                ElseIf currHor < destHor And currVer > destVer Then
                    If board(currVer - 1, currVer + 1) Is board(destVer, destHor) Then
                        moveable = True
                    End If
                ElseIf currHor > destHor And currVer < destVer Then
                    If board(currVer + 1, currVer - 1) Is board(destVer, destHor) Then
                        moveable = True
                    End If
                ElseIf currHor > destHor And currVer > destVer Then
                    If board(currVer - 1, currVer - 1) Is board(destVer, destHor) Then
                        moveable = True
                    End If
                End If
                If moveable = True Then
                    If colour = False Then
                        castleW = False
                    Else
                        castleB = False
                    End If
                End If
            Case Else
                Console.WriteLine("Invalid input")
        End Select
        'Moving pieces
        If moveable = True Then
            If colour = False Then ' white
                Select Case board(destVer, destHor)
                    Case "R ", "K ", "B ", "P ", "N ", "Q "
                        Console.WriteLine("You can't take one of your own pieces!")
                    Case Else
                        Select Case board(currVer, currHor)
                            Case "R ", "K ", "B ", "P ", "N ", "Q "
                                If board(destVer, destHor) IsNot ".." Then
                                    UpGrave += board(destVer, destHor)
                                End If
                                board(destVer, destHor) = board(currVer, currHor)
                                board(currVer, currHor) = ".."
                                colour = True
                                turn = "Black"
                                If Castling = True Then
                                    board(2, 6) = ".."
                                    If castleRightMaybe = True Then
                                        board(2, 4) = "K "
                                    End If
                                    If castleLeftMaybe = True Then
                                        board(2, 8) = "K "
                                    End If
                                End If
                            Case Else
                                Console.WriteLine("Invalid")
                        End Select
                End Select
            Else ' black
                Select Case board(destVer, destHor)
                    Case "r ", "k ", "b ", "p ", "n ", "q "
                        Console.WriteLine("You can't take one of your own pieces!")
                    Case Else
                        Select Case board(currVer, currHor)
                            Case "r ", "k ", "b ", "p ", "n ", "q "
                                If board(destVer, destHor) IsNot ".." Then
                                    LoGrave += board(destVer, destHor)
                                End If
                                board(destVer, destHor) = board(currVer, currHor)
                                board(currVer, currHor) = ".."
                                turn = "White"
                                colour = False
                                If Castling = True Then
                                    board(9, 6) = ".."
                                    If castleRightMaybe = True Then
                                        board(9, 4) = "k "
                                    End If
                                    If castleLeftMaybe = True Then
                                        board(9, 8) = "k "
                                    End If
                                End If
                            Case Else
                                Console.WriteLine("Invalid")
                        End Select
                End Select
            End If

        Else
            Console.WriteLine("Can't move there")
        End If
        Castling = False : castleLeftMaybe = False : castleRightMaybe = False
    End Sub


    Sub Check()
        ' What to check:
        ' pawns (check diagonally)
        ' knights (check L positions)
        ' rooks (use for statement)
        ' bishops (use for statement)
        ' queen (use rooks & bishops for statements)
        ' check all 9 tiles around the king (for checkmate)
        ' If all of these are true
        '     Prevent player from not moving king
        '     Prevent player from moving into check
        ' If all 8 tiles around king are moves into check
        '     checkmate!
        If colour = False Then
            Select Case board(destVer, destHor)
                Case "P "
                    If board(destVer + 1, destHor + 1) Is "k " OrElse board(destVer + 1, destHor - 1) Is "k " Then
                        inCheck = True
                    End If
                Case "R "
                    For i = destVer To 8
                        If board(i, destHor) Is "k " Then
                            inCheck = True
                        End If
                    Next
                    For i = destVer To 1
                        If board(i, destHor) Is "k " Then
                            inCheck = True
                        End If
                    Next
                    For i = destHor To 8
                        If board(destVer, i) Is "k " Then
                            inCheck = True
                        End If
                    Next
                    For i = destHor To 1
                        If board(destVer, i) Is "k " Then
                            inCheck = True
                        End If
                    Next
                Case "N "
                    If board(destVer + 1, destHor - 2) Is "k " OrElse board(destVer + 2, destHor - 1) Is "k " OrElse board(destVer + 2, destHor + 1) Is "k " OrElse board(destVer + 1, destHor + 2) Is "k " OrElse board(destVer - 1, destHor + 2) Is "k " OrElse board(destVer - 2, destHor + 1) Is "k " OrElse board(destVer - 2, destHor - 1) Is "k " OrElse board(destVer - 1, destHor - 2) Is "k " Then
                        inCheck = True
                    End If
                Case "B "
                    For i = 1 To 8 - destHor
                        If board(destVer + i, destHor + i) Is "k " Then
                            inCheck = True
                        End If
                    Next
                    For i = 1 To destHor
                        If board(destVer + i, destHor + i) Is "k " Then
                            inCheck = True
                        End If
                    Next
            End Select
        End If
    End Sub

    Sub FEN_load(ByRef Fstr As String)
        'K7/4r1q1/2Q5/5Rp1/1PN2p2/3P4/1k2p1n1/8
        Dim row, column, checking As Integer
        row = 9 'Y coordinate
        column = 2 'X coordinate
        checking = 1 'Which part of the FEN string you are checking (First is the board, then the Active colour, then Castling)
        Dim part As String

        'Clearing the board:
        For i = 2 To 9
            For j = 2 To 9
                board(i, j) = ".."
            Next
        Next
        'Placing pieces:
        For i = 0 To Fstr.Length() - 1
            part = Fstr.Chars(i)
            Select Case part
                Case "/" : row -= 1 : column = 1
                Case "1", "2", "3", "4", "5", "6", "7", "8" : column += Val(part) - 1
                Case " " : checking += 1
                Case "w" : colour = False
                Case Else
                    If checking > 1 Then
                        Select Case part
                            Case "Q", "K" : castleW = True
                            Case "q", "k" : castleB = True
                            Case "b" : colour = True
                        End Select
                    Else : board(row, column) = part + " "
                    End If
            End Select

            If checking = 4 Then : Exit For : End If
            If column = 9 Then : column = 1 : End If

            column += 1
        Next
    End Sub

    Sub FEN_save()
        Dim FENstr As String = ""
        Dim spaces As Integer = 0
        Dim i As Integer = 9
        While i > 1

            For j = 2 To 9
                If board(i, j) IsNot ".." Then
                    If spaces <> 0 Then
                        FENstr += Str(spaces).Trim()
                    End If
                    FENstr += board(i, j).Chars(0)
                    spaces = 0
                Else : spaces += 1
                End If
            Next

            If spaces > 0 Then
                FENstr += Str(spaces).Trim()
                spaces = 0
            End If

            If i > 2 Then : FENstr += "/" : End If
            i -= 1
        End While
        If turn = "Black" Then : FENstr += " b"
        Else : FENstr += " w" : End If
        Console.WriteLine(FENstr)
        Console.ReadKey()
    End Sub
End Module
