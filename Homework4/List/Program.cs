using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericApplication;

namespace Homework4_1
{
    class Program
    {
        public class GenericList<T>
        {
            private Node<T> head;
            private Node<T> tail;

            public GenericList()
            {
                tail = head = null;
            }

            public Node<T> Head
            {
                get => head;
            }

            public void Add(T t)
            {
                Node<T> n = new Node<T>(t);
                if (tail == null)
                {
                    head = tail = n;
                }
                else
                {
                    tail.Next = n;
                    tail = n;
                }
            }
            public void Foreach(Action<T> act) {
                Node<T> temp;
                temp = head;
                while (temp != null)
                {
                    act(temp.Data);
                    temp = temp.Next;
                }
            }
        }
        static void Main(string[] args)
        {
            int sum=0, max=int.MinValue, min=int.MaxValue;
            GenericList<int> intlist = new GenericList<int>();
            Random rd = new Random();
            for (int x = 0; x < 10; x++)
            {
                intlist.Add(1+rd.Next(10));
            }
            intlist.Foreach(x => Console.Write(x+" "));
            Console.WriteLine();
            intlist.Foreach(x => sum += x);
            Console.WriteLine("sum:"+sum);
            intlist.Foreach(x => max = x > max ? x : max);
            intlist.Foreach(x => min = x < min ? x : min);
            Console.WriteLine("max&min:"+max+" "+min);
        }
    }
}
