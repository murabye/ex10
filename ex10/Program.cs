using System;
using System.IO;
using MyLib;

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
        public void Fill()
        {
            var count = Ask.Num("Введите количество членов полинома: ");

            for (var i = 0; i < count; i++)
            {
                var pow = Ask.Num("Введите степень: ");
                var coef = Ask.Num("Введите коэффициент: ");

                Add(new Variable(coef, pow));
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
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
            while (cur != null)
            {
                if (cur.Power == added.Power)
                {
                    cur.Coefficient += added.Coefficient;
                    return;
                } 
                cur = cur.next;
            }
            Add(added);
        }
        public void Refresh()
        {
            for (var i=0; i < 10; i++)
            {
                var start = table[i];
                var prev = start;

                var cur = start != null ? start.next : null;

                while (cur != null)
                {
                    if (cur.Coefficient == 0)
                    {
                        prev.next = cur.next;
                        cur = cur.next;
                    }
                    else
                    {
                        prev = cur;
                        cur = cur.next;
                    }
                }

                if (start != null && start.Coefficient == 0)
                {
                    table[i] = start.next;
                }
            }
        }
        
        public static MyHashTable ppp(MyHashTable a, MyHashTable b)
        {
            var c = new MyHashTable();
            foreach (var point in a.table)
                if (point != null) c.Add(point);        // все точки из списка a добавить в ответ

            foreach (var point in b.table)
                if (point != null) c.Plus(point);

            c.Refresh();
            return c;
        }

        public override string ToString()
        {
            var ans = "";
            foreach (var obj in table)
            {
                if (obj != null)
                    ans += obj.ToString() + "\n";
            }

            return ans;
        }
    }




    class Variable
    {
        public int Coefficient { get; set; }
        public int Power { get; set; }
        public Variable next { get; set; }

        public Variable(int coefficient, int power)
        {
            Coefficient = coefficient;
            Power = power;
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
        public int GetHashCode(int capacity)
        {
            return Power % capacity;
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
            while (true)
            {
                var polynom1 = new MyHashTable();
                var polynom2 = new MyHashTable();
                polynom1.Fill();
                polynom2.Fill();

                var ans = MyHashTable.ppp(polynom1, polynom2);
                Console.WriteLine("Результат: ");
                Console.WriteLine(ans);
                
                OC.Stay();
            }


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
