using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

// без возможности в одной строке одинаковую степень

namespace ex10
{
    class MyHashTable
    {
        private int _capacity = 10;
        public Variable[] table { get; set; }

        public MyHashTable()
        {
            table = new Variable[_capacity];
        }

        public void Add(Variable added)
        {
            var cur = table[added.GetHashCode()];

            if (cur == null) { 
                table[added.GetHashCode()] = added;
                return;
            }

            while (cur.next != null)
            {
                cur = cur.next;
            }
            
            cur.next = added;
        }
        public void Plus(Variable added)
        {
            var cur = table[added.GetHashCode()];
            while ((cur != null))
            {
                if (cur.Power == added.Power)
                {
                    cur.Coefficient += added.Coefficient;
                    return;
                }
                cur = cur.next;
            }

          
        }

        public static MyHashTable operator +(MyHashTable a, MyHashTable b)
        {
            var c = new MyHashTable();
            foreach (var point in a.table)
                if (point != null) c.Add(point);        // все точки из списка a добавить в ответ

            foreach (var point in b.table)
            {
                if (point != null) c.Plus(point);
            }

            return c;
        }
    }
    class Variable
    {
        public int Coefficient { get; set; }
        public int Power { get; set; }
        public Variable next { get; set; }

        public Variable(int coefficient, int power)
        {
            this.Coefficient = coefficient;
            this.Power = power;
        }

        public Variable(string arg)
        {
            var args = arg.Split(' ');
            Coefficient = Int32.Parse(args[0]);
            Power = Int32.Parse(args[1]);
        }

        public override int GetHashCode()
        {
            return Power % 10;
        }

        public override string ToString()
        {
            return Power + " " + Coefficient;
        }
    }

    class Program
    {
        static void Main()
        {
            var polynom1 = new MyHashTable();
            var polynom2 = new MyHashTable();

            ReadFile("poly1.txt", polynom1);
            ReadFile("poly2.txt", polynom2);

            var ans = polynom1 + polynom2;

            WriteFile("ans.txt", ans);
        }

        static void ReadFile(string path, MyHashTable table)
        {
            var reader = new StreamReader(path);
            var cur = reader.ReadLine();

            while (cur != String.Empty)
            {
                table.Add(new Variable(cur));
            }

            reader.Close();
        }
        static void WriteFile(string path, MyHashTable table)
        {
            var writer = new StreamWriter(path);

            foreach (var obj in table.table)
            {
                writer.WriteLine(obj);
            }
        }
        
    }
}
