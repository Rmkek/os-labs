using System;
using System.Collections.Generic;
using System.IO;

using System.Threading;

class Example
{

    public static Random rnd = new Random();
    public static Mutex m = new Mutex();
    public static Thread t1, t2, t3;
    public static LinkedList<int> numbers = new LinkedList<int>();

    // Напишите программу, в которой два потока работая параллельно, генерируют в цикле(100 итераций)
    // некоторую последовательность целых чисел: первый поток – четные, второй – нечетные числа.
    // Каждое новое число должно быть добавлено в конец общего связанного списка.
    // После завершения работы первого и второго  потоков, третий поток должен вывести содержимое списка на экран.



    public static void generateEvenNumbers()
    {
        for (int i = 1; i <= 100; i++)
        {
            //Thread.Sleep(rnd.Next(100));
            m.WaitOne();
            if (i % 2 == 0)
            {
                numbers.AddLast(i);
            }
            //Thread.Sleep(rnd.Next(100));
            m.ReleaseMutex();
        }
    }

    public static void generateOddNumbers()
    {
        for (int i = 1; i <= 100; i++)
        {
            //Thread.Sleep(rnd.Next(100));
            m.WaitOne();
            if (i % 2 != 0)
            {
                numbers.AddLast(i);
            }
            //Thread.Sleep(rnd.Next(100));
            m.ReleaseMutex();
        }
    }

    public static void printNumbers()
    {
        while (true)
        {
            m.WaitOne();
            if (numbers.Count == 100)
            {
                Console.Write("[");
                foreach (int num in numbers)
                {
                    Console.Write(num + ", ");
                }
                Console.Write("]");
                //Console.WriteLine("Numbers: ", numbers);
                m.ReleaseMutex();
                break;
            }
            m.ReleaseMutex();

        }
    }


    public static void thread1()
    {
        generateEvenNumbers();
    }



    public static void thread2()
    {
        generateOddNumbers();
    }

    public static void thread3()
    {
        printNumbers();
    }



    public static void Main()
    {
        t1 = new Thread(thread1);
        t2 = new Thread(thread2);
        t3 = new Thread(thread3);



        t1.Start();
        t2.Start();
        t3.Start();
        // рандомно принтует их, является ли это проблемой? хз
        //Console.ReadLine();
    }

}