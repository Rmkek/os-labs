using System;

using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;


//class Thread1
//{
//    // anonymous delegate
//    static void Main()
//    {
//        Thread newThread = new Thread(new ThreadStart(Work.DoWork));

//        newThread.Start();
//    }
//}

//class Thread2
//{
//    // anonymous delegate
//    static void Main()
//    {
//        Thread newThread = new Thread(new ThreadStart(Work.DoWork));

//        newThread.Start();
//    }
//}



//class Work
//{

//    Work() { }

//    public static void DoWork() { }

//}


// Каждый из вариантов требует создания трёх потоков.
// Первый поток должен быть создан с использованием статического метода класса, второй – с использованием метода экземпляра класса,
// а третий – с использованием анонимного делегата.

// Напишите программу, которая создаёт два потока, а затем дожидается их завершения.
// Первый поток строит таблицу из n1 значений функции sin(x), начиная с точки x1 с шагом s1 с задержкой каждого шага d1.
// Второй поток строит таблицу значений функции cos(x), начиная с точки x2 с шагом s2 с задержкой каждого шага d2 на протяжении времени t2.
// Таблицы выводятся в файлы поток1.dat и поток2.dat соответственно. Вывод осуществляется по формату  

// dd.mm.yyyy hh:nn: ss.zzz «значение x:8.4» «значение y:8.4» 

// Значения n1, x1, s1, d1, x2, s2, d2 и t2 запрашиваются до создания потоков и передаются им при создании. 

// static class method

//class Work
//{
//    public static void SinWorker(int n1, int x1, int s1, int d1)
//    {
//        // таблицу из n1 значений функции sin(x), начиная с точки x1 с шагом s1 с задержкой каждого шага d1.
//        int current_x = x1;
//        StringBuilder sb = new StringBuilder();
//        for (int i = 0; i < n1; i++)
//        {
//            sb.Append(DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss.zzz") + Math.Sin(current_x) + "\n");
//            current_x += s1;
//            Thread.Sleep(d1);
//        }
//        File.WriteAllText("thread1.dat", sb.ToString());
//        Console.WriteLine(sb);
//    }

//    public static void CosWorker(object data)
//    {
//        Thread.Sleep(1000);
//        string[] splitData = data.ToString().Split(" ");
//        // таблицу из n2 значений функции cos(x), начиная с точки x1 с шагом s1 с задержкой каждого шага d1.
//        int current_x = Int32.Parse(splitData[1]);
//        StringBuilder sb = new StringBuilder();
//        for (int i = 0; i < Int32.Parse(splitData[0]); i++)
//        {
//            sb.Append(DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss.zzz") + Math.Sin(current_x) + "\n");
//            current_x += Int32.Parse(splitData[2]);
//            Thread.Sleep(Int32.Parse(splitData[3]));
//        }
//        File.WriteAllText("thread2.dat", sb.ToString());
//        Console.WriteLine(sb);
//    }
//}

//public class MainClass
//{


//    public static void Main(string[] args)
//    {

//        //int age = Convert.ToInt32(Console.ReadLine());
//        //int x1, x2, x3, x4 = Console.ReadLine().Split(" ");
//        Thread t1 = new Thread(new ThreadStart(() => Work.SinWorker(20, 1, 1, 50)));
//        Thread t2 = new Thread(new ParameterizedThreadStart(Work.CosWorker));

//        t1.Start();
//        t2.Start("10 1 1 100");
//        t1.Join();
//        t2.Join();

//    }
//}


class Example
{

    public static bool Ready1 = false;
    public static bool Ready2 = false;
    public static int calculationsThread1 = 0;
    public static int calculationsThread2 = 0;
    public static double y = 0;
    public static double x = 0;
    public static Thread t1, t2;



    public static void ThreadProc1()
    {
        Console.WriteLine("Thread 1 Running");
        Thread.Sleep(40);
        if (calculationsThread1 == 100)
        {
            Console.WriteLine("Thread 1 Calculations finished");
            Thread.CurrentThread.Suspend();
            t2.Resume();
        }
        y = Math.Sin(x);
        Console.WriteLine("Thread 1 suspended");
        Thread.CurrentThread.Suspend();
        Console.WriteLine("Thread 2 Resumed");
        t2.Resume();

    }



    public static void ThreadProc2()
    {
        Console.WriteLine("Thread 2 Running");
        Thread.Sleep(40);
        if (calculationsThread2 == 100)
        {
            Console.WriteLine("Thread 2 Calculations finished");
            Thread.CurrentThread.Suspend();
            t1.Resume();
        }
        x = Math.Cos(y);
        Console.WriteLine("Thread 2 suspended");
        Thread.CurrentThread.Suspend();
        Console.WriteLine("Thread 1 Resumed");
        t1.Resume();
    }



    public static void Main()
    {
        t1 = new Thread(ThreadProc1);
        t2 = new Thread(ThreadProc2);



        t1.Start();
        t2.Start();

        while (!Ready1 || !Ready2) { };

        t1.Resume();
        Console.WriteLine(y);
        Console.ReadLine();

    }

}