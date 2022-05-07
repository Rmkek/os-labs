using System;
using System.Collections.Generic;
using System.IO;

using System.Threading;

class Example
{
    // Напишите программу, в которой два потока помещают произвольные числа в конец очереди,
    // а два других потока извлекают числа из очереди и выводят их на экран.
    // Очередь реализовать на основе массива.
    public static Random rnd = new Random();
    public static Thread t1, t2, t3, t4;
    public static bool appendingFinished1 = false;
    public static bool appendingFinished2 = false;
    public static Queue<int> numbers = new Queue<int>();

    public static void appendRandom(string name, int amount)
    {
        lock (numbers)
        {
            for (int i = 0; i < amount; i++)
            {
                int n = rnd.Next(100);
                //Console.WriteLine(name + ", Adding: " +  n);
                numbers.Enqueue(n);
            }
        }
    }

    public static void printLast()
    {
        while (!appendingFinished1 || !appendingFinished2 || numbers.Count > 0)
        {
            //Console.WriteLine("Finished1 " + appendingFinished1 + "Finsihed2 " + appendingFinished2 + " Count: " + numbers.Count);


            //typeof(Example)
            lock (numbers)
            {
                if (numbers.Count > 0)
                {
                    int number = numbers.Peek();
                    Console.WriteLine(number);
                    numbers.Dequeue();
                }
            }
        }
    }



    public static void Appender1()
    {
        appendRandom("Appender1", 4);
        appendingFinished1 = true;
    }



    public static void Appender2()
    {
        appendRandom("Appender2", 4);
        appendingFinished2 = true;
    }

    public static void Printer1()
    {
        printLast();
    }



    public static void Printer2()
    {
        printLast();
    }




    public static void Main()
    {
        t1 = new Thread(Appender1);
        t2 = new Thread(Appender2);
        t3 = new Thread(Printer1);
        t4 = new Thread(Printer2);



        t1.Start();
        t2.Start();
        t3.Start();
        t4.Start();

        t1.Join();
        t2.Join();
        t3.Join();
        t4.Join();


        Console.WriteLine("Threads done");
        //Console.ReadLine();

    }
}