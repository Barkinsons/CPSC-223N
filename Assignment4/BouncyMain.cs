/* ------------------------------------------------------------------------------------------------
| Bouncy Ball Program: Demonstrates ball collision by bouncing ball off of panel walls.           |
| Copyright (C) 2023  Jared Sevilla                                                               |
|                                                                                                 |
| This program is free software: you can redistribute it and/or modify                            |
| it under the terms of the GNU General Public License as published by                            |
| the Free Software Foundation, either version 3 of the License, or                               |
| (at your option) any later version.                                                             |
|                                                                                                 |
| This program is distributed in the hope that it will be useful,                                 |
| but WITHOUT ANY WARRANTY; without even the implied warranty of                                  |
| MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                                   |
| GNU General Public License for more details.                                                    |
|                                                                                                 |
| You should have received a copy of the GNU General Public License                               |
| along with this program.  If not, see <https://www.gnu.org/licenses/>.                          |
------------------------------------------------------------------------------------------------ */

/** 
 * file: BouncyMain.cs
 * purpose: Creates and runs Bouncy Ball Program User Interface.
 * author: Jared Sevilla
 * email: jgsevilla@csu.fullerton.edu
 * course: CPSC223N
 * program: Bouncy Ball
 * due: 6 November, 2023
**/

// ***** PROGRAM STARTS HERE **********************************************************************
using System;
using System.Windows.Forms;

public class BouncyMain {
    public static void Main() {

        Console.WriteLine("Welcome to Main()!\nRunning user interface...\n");

        // Create and display BouncyUserInterface form.
        Application.Run(new BouncyUserInterface());

        Console.WriteLine("Welcome back to Main().\nHave a good day!\n");
    }
}
// ***** END PROGRAM ******************************************************************************