/* ------------------------------------------------------------------------------------------------
| Cat and Mouse Program: Demonstrates collision and simple cat ai.                                |
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
 * file: CatMouseMain.cs
 * purpose: Defines the main function for the cat and mouse program.
 * author: Jared Sevilla
 * email: jgsevilla@csu.fullerton.edu
 * course: CPSC223N
 * program: Cat and Mouse 
 * due: 10 December, 2023
**/

using System;
using System.Windows.Forms;

public class CatMouseMain {
    public static void Main() {
        Console.WriteLine("Welcome to Main()!\n");

        // Create and run CatMouseUserInterface
        Application.Run(new CatMouseUserInterface());

        Console.WriteLine("Welcome back to Main()!\nGoodbye!!!\n");
    }
}