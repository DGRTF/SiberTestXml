using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SiberTest
{


    /// <summary>
    /// Программа сериализует двусвязный список в бинарный текст без пробелов
    /// Стандартных средств сериализации (в том числе Encoding)  не использовалось
    /// Учитываются следующиие варианты:
    /// 1.Рандомный элемент и строковое представление объекта не заданы
    /// 2.Рандомный элемент не задан
    /// 3.Строковое представление не задано
    /// 4.Если свойство Data="" или = null ,то после десериализации Data=null
    /// 5.Последний и первый элемент связаны между собой(Список замкнут)
    /// 6.Последний и первый элемент не знают друг о друге 
    /// 7.В незамкнутом списке неправильно задан родительский элемент(Программа не будет выдавать ошибку и сама найдёт родительский элемент)
    /// Неверно заданый бинарный текст(Пустой текст или не содержащий пробелы, пустой и т.д. выдаст ошибку)
    /// </summary>
    class Program
    {

        public class BinSpacing
        {
            public string WithoutSpa (string s)// из бинарного текста в бинарный текст без пробелов
            {
                string outs="";
                foreach(char i in s.ToCharArray())
                {
                    if(i=='0')
                    {
                        outs += "00";
                    }
                    else
                    {
                        if(i=='1')
                        {
                            outs += "01";
                        }
                        else
                        {
                            if (i == ' ')
                            {
                                outs += "11";
                            }
                            else
                            {
                                Console.WriteLine("Некоректный битовый текст с пробелами");
                                return "";
                            }
                        }
                    }
                }
                return outs;
            }


            public string WithSpa(string s) // из бинарного текста в бинарный текст с пробелами
            {
                if (s != "")
                {
                    if (s != null)
                    {
                        if (s.Length % 2 == 0)
                        {
                            string outs="";
                            string modif="";



                            char[] chars = s.ToCharArray();
                            for (int i = 0; i < chars.Length; i++)
                            {
                                if (i == 0)
                                {

                                    modif = chars[i].ToString();
                                }
                                else
                                {
                                    if (i == 1)
                                    {
                                        modif += chars[i].ToString();
                                    }
                                    else
                                    {
                                        if (i%2 == 0)
                                        {
                                            if (modif == "00")
                                            {
                                                outs += "0";
                                                modif = chars[i].ToString();
                                            }
                                            else
                                            {
                                                if (modif == "01")
                                                { 
                                                    outs += "1";
                                                    modif = chars[i].ToString();
                                                }
                                                else
                                                {
                                                    if (modif == "11")
                                                    {
                                                        outs += " ";
                                                        modif = chars[i].ToString();
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Неправильный формат битового текста без пробелов");
                                                        return "";
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            modif += chars[i].ToString();
                                        }
                                    }
                                }
                                
                               
                            }
                            if (modif == "00")
                                outs += "0";
                            else
                            {
                                if (modif == "01")
                                    outs += "1";
                                else
                                {
                                    if (modif == "11")
                                        outs += " ";
                                    else
                                    {
                                        Console.WriteLine("Неправильный формат битового текста без пробелов");
                                        return "";
                                    }
                                }
                            }

                            return outs;
                        }
                        else
                        {
                            Console.WriteLine("Неправильная длина строки");
                            return "";
                        }
                    }
                    else
                    {
                        Console.WriteLine("строка не задана");
                        return "";
                    }
                }
                else
                {
                    return s;
                }
            }




        }



        public class ListRead
        {
            public ListRead()
            {

            }
            public ListRead(ListNode Head, int count)
            {
                Read(Head,count);
            }


            public void Read(ListNode Head, int count)
            {
                
                for(; ; )
                {
                    if (count == 0)
                    {
                        return;
                    }
                    else
                    {

                        if (Head != null)
                        {
                            Console.WriteLine("Element:" + Head.Data);
                            Console.Write("Родительский элемент:");
                            if (Head.Previous != null)
                                Console.WriteLine(Head.Previous.Data);
                            else
                            {
                                Console.WriteLine();
                            }

                            Console.Write("Рандомный элемент:");
                            if (Head.Random != null)
                                Console.WriteLine(Head.Random.Data);
                            else
                            {
                                Console.WriteLine();
                            }

                            Console.Write("Следующий элемент:");
                            if (Head.Next != null)
                                Console.WriteLine(Head.Next.Data);
                            else
                            {
                                Console.WriteLine();
                            }
                            Console.WriteLine();
                            Console.WriteLine();

                            if (Head.Next != null)
                            {
                                Head = Head.Next;
                                count--;
                            }
                        }
                    }
                }
            }


            public bool ListClose (ListNode Head, int count) // возвращает true ,если список замкнут
            {
                for (; ; )
                {
                    if (count == 0)
                    {
                        return true;
                    }
                    else
                    {
                        if (Head.Previous == null)
                            return false;
                        else
                        {
                            Head = Head.Previous;
                            count--;
                        }
                    }
                }
            }


            public ListNode HeadEl (ListNode Head)  // возвращает родительский элемент из незамкнутого списка
            {
                for (; ; )
                {
                    if (Head.Previous == null)
                        return Head;
                    else
                        Head = Head.Previous;
                }
            }



        }



        public class CreateList
        {
            public List<ListNode> Create(string[] s)// создаем объекты из массива строк
            {
                List<ListNode> list = new List<ListNode>();
                StringBit sb = new StringBit();
                StringForm sf = new StringForm();
                List<string> number = new List<string>();
                string numberob;
                string g;
                int n;
                foreach (string i in s)
                {

                    // если строка равна 1
                    if (i == sf.ToBitString("1"))
                    {
                        ListNode ln = new ListNode();// создаем объект без рандомного элемента и поля data
                        list.Add(ln);
                        if (list.IndexOf(ln) != 0) // если перед ним в списке есть объект, то задем текущий элемента свойству Next предыдущего объекта ,а полю Previous текущего элемента ссылку на предыдущий
                        {
                            list[list.IndexOf(ln) - 1].Next = ln;
                            ln.Previous = list[list.IndexOf(ln) - 1];
                        }
                    }

                    else
                    {
                        // если в строке индекс первого вхождения двух пробелов равен 0
                        if (i.IndexOf("  ") == 0)
                        {
                            string gg = i.Remove(0,2); // удаляем первые два пробела
                            int index = gg.IndexOf("  "); // находим индекс первого вхождения двух пробелов
                            g = gg.Remove(index, gg.Length - index);// извлекаем первый символ в бинарном виде  
                            numberob = sb.BitTextToString(g);// переводим из битового текста в строку

                            // извлекаем второй символ в бинарном виде
                            g = gg.Remove(0, index + 2);
                            numberob += " " + sb.BitTextToString(g);
                            number.Add(numberob);
                            ListNode ln = new ListNode();// создаём объект без поля Data
                            list.Add(ln);
                            if (list.IndexOf(ln) != 0)
                            {
                                list[list.IndexOf(ln) - 1].Next = ln;
                                ln.Previous = list[list.IndexOf(ln) - 1];
                            }
                        }


                        else
                        {


                            if (i.IndexOf("  ") != -1) // если строка содержит два пробела
                            {

                                n = i.IndexOf("  "); // находим индекс первого вхождения двух пробелов
                                g = i.Remove(0, n + 2); // удаляем все записи от нуля до конца первых двух пробелов
                                int index = g.IndexOf("  ");
                                g = g.Remove(index, g.Length - index);// извлекаем первый символ в бинарном виде  
                                numberob = sb.BitTextToString(g);// переводим из битового текста в строку
                                // извлекаем второй символ в бинарном виде
                                g = i.Remove(0, n + 2);
                                g = g.Remove(0, index + 2);
                                numberob += " " + sb.BitTextToString(g);
                                number.Add(numberob);
                                // создаём объект с полем Data и cсылкой на рандомный элемент
                                g = i.Remove(n, i.Length - n); // удаляем первые два пробела и всё после них
                                g = sb.BitTextToString(g);
                                // переводим из битового текста в строку
                                ListNode ln = new ListNode { Data = g };
                                list.Add(ln);
                                if (list.IndexOf(ln) != 0) // если перед ним в списке есть объект, то задем текущий элемента свойству Next предыдущего объекта ,а полю Previous текущего элемента ссылку на предыдущий
                                {
                                    list[list.IndexOf(ln) - 1].Next = ln;
                                    ln.Previous = list[list.IndexOf(ln) - 1];
                                }
                            }
                            else
                            {
                                if (i != " " && i != "  ")
                                {

                                    ListNode ln = new ListNode { Data = sb.BitTextToString(i) }; // создаём объект без cсылки на рандомный элемент
                                    list.Add(ln);
                                    if (list.IndexOf(ln) != 0) // если перед ним в списке есть объект, то задем текущий элемента свойству Next предыдущего объекта ,а полю Previous текущего элемента ссылку на предыдущий
                                    {
                                        list[list.IndexOf(ln) - 1].Next = ln;
                                        ln.Previous = list[list.IndexOf(ln) - 1];
                                    }
                                }
                            }
                        }
                    }
                }

                return list = AddRandom(list, number);
            }



            List<ListNode> AddRandom(List<ListNode> list, List<string> number) // добавляем рандомные объекты
            {
                List<Number> obj = new List<Number>();
                int index;
                foreach (string i in number) // переводим из строк в числа двух полей объекта Number
                {
                    index = i.IndexOf(" ");
                    Number num = new Number();
                    num.element = Convert.ToInt32(i.Remove(index, i.Length - index));
                    num.random = Convert.ToInt32(i.Remove(0, index));
                    obj.Add(num);
                }
                foreach (Number i in obj) // добавляем рандомные объекты элементам списка объекты
                {
                    list[i.element].Random = list[i.random];
                }
                return list;
            }



            class Number // набор чисел элемнта и рандомного объекта
            {
                public int element;
                public int random;
            }



        }



        public class StringBit
        {


            public string[] DivideText(string s)// делим битовый текст на строки
            {
                //string g = s;
                string [] outs= new string[0];
                string[] sub = new string[0];
                for (; ; )
                {
                    if (s != "")
                    {
                        if (s.IndexOf("   ") != -1) // если найдено три пробела подряд
                        {
                            int i = s.IndexOf("   "); // получаем индекс первого пробела первого вхождения
                            Array.Resize(ref sub, sub.Length + 1); // увеличиваем размер массива строк на 1
                            sub[^1] = s.Remove(i, s.Length - i); //удаляем начиная с первого пробела из трех подряд и до конца строки, присваеваем значение индексу массива
                            s = s.Remove(0, i + 3); // удаляем из строки все символы от нулевого до первго вхождения последнего пробела из 3 +3 элемента
                        }
                        else
                        {
                            if (s != " " && s != "  ")
                            {
                                Array.Resize(ref sub, sub.Length + 1); // увеличиваем размер массива строк на 1
                                sub[^1] = s;
                            }
                            return sub;
                        }
                    }
                }
               // Console.WriteLine("Поделили текст на строки, вывод:{0}", sub);
                return sub;
            }



            public string BitTextToString(string s) // преобразуем бинарную строку в алфавитную
            {
                string back = "";
                string outback = "";
                char cback;
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] != ' ')
                    {
                        back += Convert.ToString(s[i]);

                        if (i == s.Length - 1)
                        {
                            short sh = Convert.ToInt16(back, 2);
                            cback = Convert.ToChar(sh);
                            outback += cback.ToString();
                            back = "";
                        }
                    }
                    else
                    {
                        short sh = Convert.ToInt16(back, 2);
                        cback = Convert.ToChar(sh);
                        outback += cback.ToString();
                        back = "";
                    }
                }
                return outback;
            }
        }



        public class StringForm
        {



            public string AddString(ListNode list) // складываем строки
            {
                string s = "";
                for (; ; )
                {
                    if (list != null)
                    {
                        if (list.Data == "" || list.Data == null && list.Random == null) // если задана пустая строка объекта и рандомный элемент не задан
                        {
                            s += ToBitString("1");
                        }
                        else
                        {
                            s += ToBitString(list.Data);

                            if (list.Random != null) // если рандомный элемент задан
                            {

                                // номер текущего объекта в списке отсчитывамый от нуля
                                string g = Number(list).ToString();
                                s += "  " + ToBitString(g);
                                // номер рандомного объекта в списке отсчитывамый от нуля
                                g = Number(list.Random).ToString();
                                s += "  " + ToBitString(g);
                            }


                        }
                        if (list.Next != null)
                        {
                            s += "   ";
                            list = list.Next;
                        }
                        else
                        {
                            return s;
                        }
                    }
                }

            }



            public string ToBitString(string s) // преобразуем в биты
            {
                if (s == "" || s == null)
                {
                    return "";
                }
                else
                {
                    string g;
                    string bin = "";
                    char[] a = s.ToCharArray();
                    foreach (char i in a)
                    {
                        g = Convert.ToString(i, 2);
                        bin += g;
                        bin += " ";
                    }
                    // удаляем последний пробел в строке
                    int x1;
                    x1 = bin.Length - 1;
                    bin = bin.Substring(0, x1);

                    return bin;
                }
            }



            int Number(ListNode list) // номер объекта в списке отсчитывамый от нуля
            {
                int number = 0;
                for (; ; )
                {
                    if (list.Previous != null)
                    {
                        number++;
                        list = list.Previous;
                    }
                    else
                        return number;
                }
                
            }
        }




        public class ListNode
        {
            public ListNode Previous;
            public ListNode Next;
            public ListNode Random; // произвольный элемент внутри списка
            public string Data;
        }




        public class ListRandom
        {


            public ListNode Head;
            public ListNode Tail;
            public int Count;  // всего объектов в списке






            public void Serialize(Stream stream)
            {
                StringForm sf = new StringForm();
                if (Head != null)
                {
                    bool l;
                    //if (Head.Previous == null)
                    //{
                    //если список замкнут, то разрываем последний элемент списка с первым
                    ListRead lr = new ListRead();
                    if (lr.ListClose(Head, Count))
                    {
                        l = true;
                        Console.WriteLine("Список является замкнутым!");
                        Head.Previous.Next = null;
                        Head.Previous = null;
                    }
                    else // Если список не замкнут, то находим родительский элемент списка(Если случайно задан не родительский)
                    {
                        Console.WriteLine("Cписок не замкнут");
                        l = false;
                        Head = lr.HeadEl(Head);
                    }
                    
                        // преобразуем объекты в бинарную строку с пробелами
                        string bin = sf.AddString(Head);

                        // преобразуем бинарный текст с пробелами в бинарную строку
                        BinSpacing bs = new BinSpacing();
                        bin = bs.WithoutSpa(bin);
                    if (l) // если список замкнут, устанавливаем в битовой строке еденицу первым символом
                        bin = "1" + bin;
                    else // в протвном случае - 0
                        bin = "0" + bin;

                        // преобразуем битовый текст в байты
                        byte[] by = new byte[bin.Length];
                        int count = 0;
                        foreach (char i in bin)
                        {
                            by[count] = Convert.ToByte(i);
                            count++;
                        }
                        using (stream)// записываем в файл
                        {

                            // запись массива байтов в файл
                            stream.Write(by, 0, by.Length);
                            
                            Console.WriteLine("Бинарный текст записан в файл");
                            Console.WriteLine();
                            Console.WriteLine("-------------------------------------------------");
                            Console.WriteLine();
                        }
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Неправильно задан родительский элемент списка");
                    //}
                }
                else
                {
                    Console.WriteLine("Нет заданного списка");
                }
            }







            public void Deserialize(Stream stream)
            {
                Head = null;
                using (stream)
                {
                    // преобразуем строку в байты
                    byte[] by = new byte[stream.Length];
                    // считываем данные
                    stream.Read(by, 0, by.Length);

                    if (by.Length != 0) // Если файл не пуст
                    {
                        try
                        {
                            // преобразуем байты в битовый текст
                            char[] cha = new char[by.Length];
                            int coun = 0;
                            foreach (byte i in by) // преобразуем байты в символы
                            {
                                cha[coun] = Convert.ToChar(i);
                                coun++;
                            }

                            string strbyte = "";
                            foreach (char i in cha) //преобразуем символы в битовый текст
                            {
                                strbyte += i.ToString();
                            }

                            
                            string strclose = strbyte.Remove(1); // присваеваем строке первый элемент из битового текста без пробелов
                            strbyte = strbyte.Substring(1); // удаляем из текста первый элемент

                            // преобразуем битовую строку в битовую строку с пробелами
                            BinSpacing bs = new BinSpacing();
                            strbyte = bs.WithSpa(strbyte);
                            if (strbyte.IndexOf("1") != -1 && strbyte.IndexOf("0") != -1)
                            {

                                // делим битовый текст на битовые строки текста
                                CreateList create = new CreateList();
                                StringBit sb = new StringBit();

                                List<ListNode> list = create.Create(sb.DivideText(strbyte)); // создаём двусвязный список из строк
                                Console.WriteLine("Список восстановлен!");
                                Console.WriteLine();
                                Head = list[0];
                                Tail = list[list.Count - 1];
                                Count = list.Count;
                                if (strclose == "1") // если вначале текста была еденица, то список замкнут.делаем связь между первым и последним элементом
                                {
                                    Head.Previous = Tail;
                                    Tail.Next = Head;
                                    Console.WriteLine("Список замкнут");
                                    Console.WriteLine();
                                }
                                
                            }
                            else
                            {
                                Console.WriteLine("Битовый текст не содержит нужных символов");
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Некорректный текст файла1");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Файл пуст!");
                    }
                }
            }
        }




        static void Main(string[] args)
        {
            // создаем коллекцию двусвязного списка
            ListNode ln = new ListNode { Data = "Gena" };
            ListNode ln1 = new ListNode { Data = "Petya Rogov" };
            ListNode ln2 = new ListNode { Data = "Vasya" };
            ListNode ln3 = new ListNode();
            ListNode ln4 = new ListNode { Random = ln1 };
            ListNode ln5 = new ListNode { Previous = ln4, Data = "Goba", Random = ln2 };
            ln.Next = ln1;
            ln.Random = ln2;

            ln1.Previous = ln;
            ln1.Next = ln2;
            ln1.Random = ln2;

            ln2.Previous = ln1;
            ln2.Random = ln;
            ln2.Next = ln3;

            ln3.Previous = ln2;
            ln3.Next = ln4;

            ln4.Previous = ln3;
            ln4.Next = ln5;

            ln5.Next = ln;

            ln.Previous = ln5;




            ListRandom lr = new ListRandom
            {
                Head = ln,
                Tail = ln2,
                Count = 6
            };

            // выводим список
            Console.WriteLine("Созданный список:");
            Console.WriteLine();

            ListRead listr = new ListRead(lr.Head,lr.Count);
            // создаем каталог для файла
            string path = @"C:\New";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }


            ListRandom lr1 = new ListRandom();
            try
            {
                FileStream stream = new FileStream(@$"{path}\note.txt", FileMode.Create);
                lr.Serialize(stream);
                FileStream stream1 = File.OpenRead(@$"{path}\note.txt");
                lr1.Deserialize(stream1);
            }
            catch
            {
                Console.WriteLine("Проблема с потоком, файл невозможно создать/записать/прочитать");
            }
            
            // чтение списка

            ListRead lisR = new ListRead(lr1.Head,lr1.Count);

        }
    }
}
