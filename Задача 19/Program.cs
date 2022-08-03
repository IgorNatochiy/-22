using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Задача_19
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите число элементов линейного массива");
            object dimensionArray = (object)Convert.ToInt32(Console.ReadLine());

           
            Func<object, int[]> funcMakeArray = new Func<object, int[]>(GetRandomArray);
            Task<int[]> taskGetArray = new Task<int[]>(funcMakeArray, dimensionArray);


            Func<Task<int[]>, int> funcSummArray = new Func<Task<int[]>, int>(GetSumArray);
            Task<int> taskGetSummArray = taskGetArray.ContinueWith<int>(funcSummArray);

        
            Func<Task<int[]>, int> funcMaxArray = new Func<Task<int[]>, int>(GetMaxElementArray);
            Task<int> taskGetMaxArray = taskGetArray.ContinueWith<int>(funcMaxArray);


            Action<Task<int[]>> actionPrintarray = new Action<Task<int[]>>(PrintArray);
            Task taskPrintArray = taskGetArray.ContinueWith(actionPrintarray);


            Action<Task<int>> actionMax = new Action<Task<int>>(PrintMaxElement);
            Task taskPrintMax = taskGetMaxArray.ContinueWith(actionMax);


            Action<Task<int>> actionSumm = new Action<Task<int>>(PrintSummElements);
            Task taskPrintSumm = taskGetSummArray.ContinueWith(actionSumm);

            taskGetArray.Start();

            Console.ReadKey();
        }

        static void PrintMaxElement(Task<int> task)
        {
            Console.WriteLine("Мксимальный элемент массива имеет значение " + task.Result);
        }


        static void PrintSummElements(Task<int> task)
        {
            Console.WriteLine("Сумма элементов массива равна " + task.Result);
        }

        static void PrintArray(Task<int[]> task)
        {
            int[] array = task.Result;
            foreach (int i in array) Console.Write(i + " ");
            Console.WriteLine();
        }

        static int[] GetRandomArray(object NumberOfElementsObj)
        {
            int NumberOfElements = (int)NumberOfElementsObj;
            int[] arrayRandom = new int[NumberOfElements];
            Random random = new Random();
            for (int i = 0; i < NumberOfElements; i++) arrayRandom[i] = random.Next(0, 100);
            return arrayRandom;
        }

        static int GetSumArray(Task<int[]> task)
        {
            int[] array = task.Result;
            int sum = 0;
            foreach (int i in array) sum += i;
            return sum;
        }

        static int GetMaxElementArray(Task<int[]> task)
        {
            int[] array = task.Result;
            int max = array[0];
            for (int i = 0; i < array.Length; i++) if (array[i] > max) max = array[i];
            return max;
        }
    }
}
