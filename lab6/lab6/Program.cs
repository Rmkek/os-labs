using System;
using System.IO;
using System.Text;
using System.Threading;

class Example
{

    public static int[] arr;
    public static StringBuilder sb = new StringBuilder();
    public static String upperString = "";
    public static bool readerFinished = false;
    public static bool firstRun = true;

    public static AutoResetEvent evWriter;
    public static AutoResetEvent evUpper;
    public static AutoResetEvent evReader;



    //Тело потока-писателя 

    public static void Upper()
    {
        while (!readerFinished)
        {
            evUpper.WaitOne();

            if (sb.Length != 0)
            {
                upperString = sb.ToString().ToUpper();
                sb.Remove(0, sb.Length);
            }
            evWriter.Set();
        }
    }

    public static void Writer()
    {
        while (!readerFinished)
        {
            evWriter.WaitOne();

            if (upperString.Length != 0)
            {
                Console.WriteLine(upperString);
            }
            evReader.Set();
        }
    }




    //Тело потока-читателя 
    public static void Reader()
    {
        // Display the file contents by using a foreach loop.
        foreach (string line in File.ReadAllLines(@"/Users/rmk/no_escape.txt"))
        {
            evReader.WaitOne();
            sb.Append(line);
            evUpper.Set();
        }
        readerFinished = true;
    }





    public static void Main()
    {

        evWriter = new AutoResetEvent(true);
        evReader = new AutoResetEvent(true);
        evUpper = new AutoResetEvent(true);



        Thread reader = new Thread(Reader);
        Thread upper = new Thread(Upper);
        Thread writer = new Thread(Writer);


        // третий поток выводит на консоль


        reader.Start();
        upper.Start();
        writer.Start();


        evReader.Set();



        // основной поток дожидается завершения работы  
        // читателя и писателя 
        reader.Join();
        upper.Join();
        writer.Join();

    }

}