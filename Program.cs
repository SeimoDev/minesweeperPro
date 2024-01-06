using System;
using System.Collections.Specialized;
using System.Data.Common;
using System.Numerics;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace 扫雷Pro
{
    class Program
    {
        int x;
        int y;
        int boom;
        public void getSize()
        {
            //从settings.conf文件中读取两个整数作为x,y的值，若不存在文件则自动新建并写入"10 10"
            if (File.Exists("settings.conf"))
            {
                // 读取文件中的值
                string[] values = File.ReadAllText("settings.conf").Split(' ');
                if (values.Length >= 3)
                {
                    int.TryParse(values[0], out x);
                    int.TryParse(values[1], out y);
                    int.TryParse(values[2], out boom);
                }
            }
            else
            {
                // 如果文件不存在，写入默认值
                using (StreamWriter sw = File.CreateText("settings.conf"))
                {
                    sw.Write("10 10 10");
                }
                x = 10;
                y = 10;
                boom = 10;
            }
        }

        public void setting()
        {
        start:
            Console.Clear();
            Console.WriteLine("请选择设置项\n\n");
            Console.WriteLine("1. 设置地图大小");
            Console.WriteLine("2. 设置炸弹数量");
            Console.WriteLine("3. 退出设置\n\n");
            Console.WriteLine("请输入序号：");
            int key = int.Parse(Console.ReadLine());
            if (key == 1)
            {
                setMap();
                if (boom > x * y)
                {
                    using (StreamWriter sw = File.CreateText("settings.conf"))
                    {
                        sw.Write(x + " " + y + " " + "10");
                    }
                }
                goto start;
            }
            else if (key == 2)
            {
                setBoom();
                goto start;
            }
            else if (key == 3) return;
            else
            {
                Console.WriteLine("输入错误，请重新输入");
                setting();
            }
        }
        public void setBoom()
        {
            Console.WriteLine("请输入炸弹数量：");
            int num;
            while (true)
            {
                string Get = Console.ReadLine();
                num = !int.TryParse(Get, out num) ? 0 : num;
                if (num > x * y)
                {
                    Console.WriteLine("炸弹数量过多，请重新输入");
                }
                else
                {
                    using (StreamWriter sw = File.CreateText("settings.conf"))
                    {
                        sw.Write(x + " " + y + " " + num);
                    }
                    return;
                }
            }
        }
        public void setMap()
        {
            Console.Clear();
            Console.WriteLine("请选择地图大小\n\n");
            Console.WriteLine("当前大小：" + x + "*" + y+"\n\n");
            Console.WriteLine("1. 10*10");
            Console.WriteLine("3. 20*20");
            Console.WriteLine("4. 30*30");
            Console.WriteLine("2. 40*40");
            Console.WriteLine("5. 自定义");
            Console.WriteLine("6. 退出\n\n");
            Console.WriteLine("请输入序号：");
            while (true)
            {
                string Get = Console.ReadLine();
                int input = !int.TryParse(Get, out input) ? 0 : input;
                if (input == 6) return;
                else if (input == 1)
                {
                    using (StreamWriter sw = File.CreateText("settings.conf"))
                    {
                        sw.Write("10 10 "+boom);
                    }
                    return;
                }
                else if (input == 2)
                {
                    using (StreamWriter sw = File.CreateText("settings.conf"))
                    {
                        sw.Write("20 20 " + boom);
                    }
                    return;
                }
                else if (input == 3)
                {
                    using (StreamWriter sw = File.CreateText("settings.conf"))
                    {
                        sw.Write("30 30 " + boom);
                    }
                    return;
                }
                else if (input == 4)
                {
                    using (StreamWriter sw = File.CreateText("settings.conf"))
                    {
                        sw.Write("40 40 " + boom);
                    }
                    return;
                }
                else if(input==5) {
                    int a = 0, b = 0;
                    string pattern = @"\d+\*\d+"; // 正则表达式模式
                    string get = Console.ReadLine();
                    string[] key = get.Split("*");
                    if (key.Length == 2)
                    {
                        a = int.Parse(key[0]);
                        b = int.Parse(key[1]);
                        using (StreamWriter sw = File.CreateText("settings.conf"))
                        {
                            sw.Write(a + " " + b + " " + boom);
                        }
                        return;
                    }
                    else
                    {
                        Console.WriteLine("输入错误，请重新输入");
                    }
                }
                else
                {
                    Console.WriteLine("输入错误，请重新输入");
                }
            }
        } 
        public void show(string[,] array)
        {
            Console.Clear();
            for (int i = 1; i <= x; i++)
            { 
                 for (int j = 0; j <= y; j++)
                {
                    if(j==0)Console.Write(i + "\t");
                    else Console.Write(array[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n\n请输入坐标(a b)，如 2 3 ");
        }
        public void startGame()
        {
            Console.Clear();
            int num = 0;
            int[,] booms = new int[x+2, y+2];
            string[,] map = new string[x + 2, y + 2];
            int tmp = boom;
            while(tmp > 0)
            {
                Random ran = new Random();
                int x1 = ran.Next(1, x+1);
                int y1 = ran.Next(1, y+1);
                if (booms[x1, y1] == 0)
                {
                    booms[x1, y1] = 1;
                    tmp--;
                }
            }
            for(int i=1;i<=x; i++)
            {
                for(int j=1;j<=y; j++)
                {
                    map[i, j] = "#";
                }
            }
            show(map);
            while (true)
            {
                int a=0, b=0;
                string pattern = @"^\d+\s\d+$"; // 正则表达式模式
                string get = Console.ReadLine();
                string[] key= get.Split(" ");
                if (key.Length == 2&&Regex.IsMatch(get,pattern))
                {
                    a = int.Parse(key[0]);
                    b = int.Parse(key[1]);
                }
                else
                {
                    Console.WriteLine("输入错误，请重新输入");
                    continue;
                }
                if(a>x||b>y||a<1||b<1)
                {
                    Console.WriteLine("输入错误，请重新输入");
                    continue;
                }
                if (booms[a, b] == 1)
                {
                    Console.Clear();
                    for(int i=1;i<=x;i++)
                    {
                        for(int j=0;j<=y;j++)
                        {   
                            if(j==0)Console.Write(i + "\t");
                            else if(booms[i, j] == 1) Console.Write("* ");
                            else Console.Write("0 ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine("你输了!\n\n按任意键重开...");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    num = 0;
                    for (int i = a - 1; i <= a + 1; i++)
                    {
                        for (int j = b - 1; j <= b + 1; j++)
                        {
                            if (booms[i, j] == 1) num++;
                        }
                    }
                    map[a, b] = num.ToString();
                    show(map);
                }
            }
            

        }
        public void about()
        {
            Console.Clear();
            Console.WriteLine("扫雷Pro v1.1\n\n");
            Console.WriteLine("作者：SeimoDev");
            Console.WriteLine("官网：https://seimo.cn/\n");
            Console.WriteLine("按任意键返回...");
            Console.ReadKey();
        }
        static void Main(string[] args)
        {
        start:
            Console.Clear();
            Program start = new Program();
            start.getSize();
            Console.WriteLine("欢迎游玩扫雷Pro\n\n");
            Console.WriteLine("当前设置：" + start.x + "*" + start.y + " 炸弹数量：" + start.boom + "\n");
            Console.WriteLine("1. 开始游戏");
            Console.WriteLine("2. 游戏设置");
            Console.WriteLine("3. 关于游戏");
            Console.WriteLine("4. 退出游戏\n\n");
            Console.WriteLine("输入序号：");
            string Get = Console.ReadLine();
            int input = !int.TryParse(Get, out input) ? 0 : input;
            if (input == 1)
            {
                start.startGame();
                goto start;
            }
            else if (input == 2)
            {
                start.setting();
                goto start;
            }
            else if (input == 3) 
            { start.about();
                goto start;
            }
            else if (input == 4) Environment.Exit(0);
            else
            {
                Console.WriteLine("输入错误，请重新输入");
                goto start;
            }
        }
    }
}
