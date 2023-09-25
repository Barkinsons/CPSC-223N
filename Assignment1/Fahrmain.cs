using System;
using System.Windows.Forms;

public class Fahrmain {
    static void Main(string[] args) {
        System.Console.WriteLine("Welcome to the Main method of the fahrenheit conversion program");
        Fahruserinterface fahrapp = new Fahruserinterface();
        Application.Run(fahrapp);
        System.Console.WriteLine("Main method will new shutdown.");
    }
}