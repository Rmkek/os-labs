using System;
using System.IO;
using System.Threading;

class Example
{
    // В парикмахерской работает три мастера и одновременно может находиться не более 10 клиентов, ожидающих своей очереди.
    // Через случайные промежутки времени в парикмахерскую пытаются попасть новые клиенты,
    // но зайти они могут лишь в том случае, если есть свободные места.
    // Если очередь не пуста, мастер вызывает из нее  клиента и в течение случайного промежутка времени занимается его стрижкой,
    // в противном случае мастер отдыхает.
    public static Random rnd = new Random();
    public static Thread m1, m2, m3, c1, c2, c3;

    public static Semaphore Sem1 = new Semaphore(3, 3);
    public static Semaphore Sem2 = new Semaphore(0, 10);


    public static int index = 0;
    public static int[] arr = new int[10];

    public static int Get()
    {
        lock (typeof(Example))
        {
            int result = arr[index - 1];
            index--;
            return result;
        }
    }

    public static void Put(int n)
    {
        lock (typeof(Example))
        {
            index++;
            arr[index - 1] = n;
        }
    }

    public static void Master()
    {
        while (true)
        {
            //  Если очередь пуста, мастер засыпает,
            //  до тех пор, пока в очереди не новый клиент 
            Sem2.WaitOne();
            int number = Get(); //извлекаем число из очереди
            Console.WriteLine(number);
            Thread.Sleep(number * 10);
            //  Увеличиваем счетчик текущего числа ресурсов семафора Sem1.  
            //  Это позволит проснуться потоку-писателю} 
            Sem1.Release();
        }
    }

    public static void Client()
    {
        while (true)
        {
            //  Если очередь полна, поток писатель засыпает,  
            //  до тех пор,пока не освободится, хотя бы одно свободное место 
            Sem1.WaitOne();
            //  помещаем в очередь случайное число 
            Put(rnd.Next(100));
            //  Увеличиваем счетчик текущего числа ресурсов семафора sem2.  
            //  Это позволит проснуться одному из ожидающих  
            //  потоков-читателей 
            Sem2.Release();
        }
    }





    public static void Main()
    {
        m1 = new Thread(Master);
        //m2 = new Thread(Master);
        //m3 = new Thread(Master);

        c1 = new Thread(Client);
        c2 = new Thread(Client);
        c3 = new Thread(Client);



        m1.Start();
        //m2.Start();
        //m3.Start();

        c1.Start();
        c2.Start();
        c3.Start();



        Console.ReadLine();

    }

}