using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai2KPDL
{
    class Program
    {
        static void Main(string[] args)
        {
            QLDiem qLDiem = new QLDiem("input.txt");
            qLDiem.Cal();

            Console.ReadKey();
        }
    }

    class Diem
    {
        public float xp { get; set; }
        public float x
        {
            get { return xp; }
            set { xp = (float)Math.Round(value, 2); }
        }
        public float yp { get; set; }
        public float y
        {
            get { return yp; }
            set { yp = (float)Math.Round(value, 2); }
        }
        public string name { get; set; }
        public Cum cum { get; set; }

        public Diem(string name, float x, float y)
        {
            this.x = x;
            this.y = y;
            this.name = name;
            cum = null;
        }
    }
    class Cum
    {
        public float xp { get; set; }
        public float x
        {
            get { return xp; }
            set { xp = (float)Math.Round(value, 2); }
        }
        public float yp { get; set; }
        public float y
        {
            get { return yp; }
            set { yp = (float)Math.Round(value, 2); }
        }
        public List<Diem> ltDiem { get; set; }

        public Cum(float x, float y)
        {
            this.x = x;
            this.y = y;
            ltDiem = new List<Diem>();
        }

        public void Cal()
        {
            if (!ltDiem.Any()) return;
            x = ltDiem.Select(item => item.x).Average();
            y = ltDiem.Select(item => item.y).Average();
        }
    }

    class QLDiem
    {
        ConcurrentDictionary<string, Diem> dic { get; set; }
        List<Cum> tam { get; set; }

        public QLDiem(string filepath)
        {
            dic = new ConcurrentDictionary<string, Diem>();
            tam = new List<Cum>();
            string[] Tam = null;
            foreach (var s in File.ReadLines(filepath))
            {
                string[] str = s.Trim().Split(' ');

                if (str.Count() > 0)
                {
                    if (Tam is null) { Tam = str; continue; }
                    else
                        dic.TryAdd(str[0], new Diem(str[0], float.Parse(str[1]), float.Parse(str[2])));
                }
            }

            foreach (var item in Tam)
            {
                Diem diem = dic[item];
                tam.Add(new Cum(diem.x, diem.y));
            }
        }

        public void Cal()
        {
            int vong = 0;
            while (true)
            {
                Console.Write($"\n\nVong {vong++}");
                bool isBreak = true;
                foreach (var item in dic.OrderBy(x => x.Key))
                {
                    Cum cum = null;
                    float min = 9999;
                    Console.Write($"\nDiem {item.Key}   :");
                    foreach (var c in tam)
                    {
                        float kc = (float)Math.Sqrt((c.x - item.Value.x) * (c.x - item.Value.x) + (c.y - item.Value.y) * (c.y - item.Value.y));
                        //float kc = (float)Math.Abs(c.x - item.Value.x) + (float)Math.Abs(c.y - item.Value.y);
                        Console.Write(Math.Round(kc, 2).ToString().PadRight(10));

                        if (min > kc)
                        {
                            cum = c;
                            min = kc;
                        }
                    }

                    if (!cum.Equals(item.Value.cum)) isBreak = false;
                    item.Value.cum = cum;
                    cum.ltDiem.Add(item.Value);
                }

                int i = 0;
                foreach (var c in tam)
                {
                    c.Cal();
                    Console.Write($"\nC{i++} new:({c.x},{c.y}) Group{{{string.Join(", ", c.ltDiem.Select(x => x.name))}}}");
                    c.ltDiem.Clear();
                }
                if (isBreak) break;

            }
        }
    }
}
