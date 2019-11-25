using System;
using System.IO;
using System.Linq;

namespace SiberTest
{

    /// <summary>
    /// Программа сериализует двусвязный список в xml разметку.
    /// Исходный файл сериализации стандартизирован под xml, поэтому его можно прочитать стандартными средствами десериализации .NET
    /// 
    /// Сериализация\Десериализация за O(n^2), как сделать её за O(n) - не знаю, судя по всему нужно хранить рандомный элемент не по номеру в списке,
    /// но в таком случае ,при повторяющихся последовательностях объектов, возможна десериализация не того объекта в рандомный элемент.
    /// 
    /// Не используются: рекурсивные методы, списки, изменение размеров массива
    /// 
    /// Учитываются следующиие варианты:
    /// 1.Рандомный элемент и строковое представление объекта не заданы
    /// 2.Рандомный элемент не задан
    /// 3.Строковое представление не задано
    /// 4.Если свойство Data="",то вернётся "" .Если Data = null ,то после десериализации Data=null
    /// 5.Последний и первый элемент связаны между собой(Список замкнут)
    /// 6.Последний и первый элемент не знают друг о друге 
    /// 7.В незамкнутом списке неправильно задан родительский элемент(Программа не будет выдавать ошибку и сама найдёт родительский элемент)
    /// </summary> 




    class Program
    {

        public class StringSymbol
        {

            public string RepCharacters(string s) // меняем специальные символы на заменяющие их
            {
                s = s.Replace("&", "&amp;");
                s = s.Replace("<", "&lt;");
                s = s.Replace(">", "&gt;");
                s = s.Replace("\"", "&quot;");
                return s;
            }



            public string Characters(string s) // восттанавливаем специальные символы
            {
                s = s.Replace("&amp;", "&");
                s = s.Replace("&lt;", "<");
                s = s.Replace("&gt;", ">");
                s = s.Replace("&quot;", "\"");
                return s;
            }
        }



        public class ListRead
        {
            public ListRead()
            {

            }
            public ListRead(ListNode Head, int count)
            {
                Read(Head, count);
            }


            public void Read(ListNode Head, int count)
            {

                while (count != 0)
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

                    Head = Head.Next;
                    count--;
                }
            }


            public bool ListClose(ListNode Head, int count) // возвращает true ,если список замкнут
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


            public ListNode HeadEl(ListNode Head)  // возвращает родительский элемент из незамкнутого списка
            {
                for (; ; )
                {
                    if (Head.Previous == null)
                        return Head;
                    else
                        Head = Head.Previous;
                }
            }


            public int Number(ListNode list) // номер объекта в списке отсчитывамый от нуля
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


        public class RandomOb
        {
            public ListNode[] AddRandom(ListNode[] list, Number[] num) // добавляем рандомные объекты
            {
                foreach (Number i in num) // добавляем рандомные объекты элементам списка объекты
                {
                    if (i == null)
                        return list;
                    list[i.element].Random = list[i.random];
                }
                return list;
            }
        }


        public class Number // набор чисел элемнта и рандомного объекта
        {
            public int element;
            public int random;
        }



        public class ObjectToXml // преобразуем объекты в xml
        {
            public string[] XmlString(ListNode Head, int count) // преобразуем объекты в массив строк xml
            {
                string[] list = new string[count];
                string xml;
                count = 0;
                ListRead listR = new ListRead();
                StringSymbol sb = new StringSymbol();
                for (; ; )
                {
                    if (Head.Data == null && Head.Random == null) // если свойство дата и рандом не заданы
                    {
                        xml = "<Object/>";
                        list[count] = xml;
                        if (Head.Next != null)
                        {
                            count++;
                            Head = Head.Next;
                        }
                        else
                            return list;
                    }
                    else
                    {
                        xml = "<Object ";

                        if (Head.Data != null) // если свойство дата задано, то вписываем его
                            xml = "<Object Data=\"" + sb.RepCharacters(Head.Data) + "\" ";

                        if (Head.Random != null) //если рандомный элемент задан, то задаём его номер
                            xml += "Random=\"" + listR.Number(Head.Random).ToString() + "\"";

                        xml += "/>";
                        list[count] = xml;

                        if (Head.Next != null)
                        {
                            count++;
                            Head = Head.Next;
                        }
                        else
                            return list;
                    }
                }
            }
        }



        public class XmlToObject
        {
            public string[] StrElem(string s) // возвращаем массив xml строк каждого элемента
            {
                int count = 0;
                // находим количество элементов в разметке
                foreach (char i in s.ToArray())
                {
                    if (i == '<')
                        count++;
                }
                string[] obj = new string[count];
                count = 0;
                while (s.IndexOf("<") != -1) // пока элементы не закончились
                {
                    // находим элемент, добавляем в массив и удаляем
                    obj[count] = s.Remove(s.IndexOf(">") + 1, s.Length - s.IndexOf(">") - 1); // добавляем первый элемент в массив
                    s = s.Remove(0, s.IndexOf(">") + 1);// удаляем первый элемент
                    count++;
                }
                return obj;
            }


            public ListNode[] Create(string[] obj)// Создаём объекты из массива строк xml
            {
                StringSymbol sb = new StringSymbol();
                ListNode[] lis = new ListNode[obj.Length]; // массив объектов listNode
                Number[] run = new Number[obj.Length]; // массив объектов с номерами элементов и произвольных объектов
                int count = 0; // счётчик элементов
                int coun = 0;
                string ob; // изменяемая строка для выделения значений свойств
                foreach (string i in obj)
                {
                    ListNode ln = new ListNode();
                    if (count != 0)
                    {
                        lis[count - 1].Next = ln;
                        ln.Previous = lis[count - 1];
                    }

                    if (i.IndexOf("Data=\"") != -1) // если задано свойство Data
                    {
                        ob = i.Remove(0, i.IndexOf("a=\"") + 3); // удаляем всё от первого символа до открывающейся кавычки включительно
                        ob = ob.Remove(ob.IndexOf("\""), ob.Length - ob.IndexOf("\"")); // удаляем всё от последней(она же в текущей строке первая) кавычки до конца
                        ln.Data = sb.Characters(ob); // 
                    }
                    if (i.IndexOf("Random=\"") != -1) // если задано свойство Random
                    {
                        ob = i.Remove(0, i.IndexOf("m=\"") + 3); // удаляем всё от первого символа до первой кавычки после random включительно
                        ob = ob.Remove(ob.IndexOf("\""), ob.Length - ob.IndexOf("\"")); // удаляем всё от последней(она же в текущей строке первая) кавычки до конца
                        Number num = new Number
                        {
                            element = count,
                            random = Convert.ToInt32(ob)
                        };
                        run[coun] = num;
                        coun++;
                    }
                    lis[count] = ln;
                    count++;
                }

                RandomOb random = new RandomOb();
                return random.AddRandom(lis, run);

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
                if (Head != null)
                {
                    string bin = "<?xml version=\"1.0\" encoding=\"utf-16\" ?>";
                    //если список замкнут, то разрываем последний элемент списка с первым
                    ListRead lr = new ListRead();
                    if (lr.ListClose(Head, Count))
                    {
                        Console.WriteLine("Список является замкнутым!");
                        Head.Previous.Next = null;
                        Head.Previous = null;
                        bin += "<List Close =\"1\">";
                    }
                    else // Если список не замкнут, то находим родительский элемент списка(Если случайно задан не родительский)
                    {
                        Console.WriteLine("Cписок не замкнут");
                        Head = lr.HeadEl(Head);
                        bin += "<List Close =\"0\">";
                    }

                    // добавляем дочерние элементы в список xml
                    ObjectToXml o = new ObjectToXml();
                    foreach (string i in o.XmlString(Head, Count))
                    {
                        bin += i;
                    }

                    // добавляем закрывающий тег корневого элемента
                    bin += "</List>";

                    // преобразуем текст в байты
                    byte[] by = System.Text.Encoding.Default.GetBytes(bin);

                    using (stream)// записываем в файл
                    {
                        // запись массива байтов в файл
                        stream.Write(by, 0, by.Length);

                        Console.WriteLine("Текст записан в файл");
                        Console.WriteLine();
                        Console.WriteLine("-------------------------------------------------");
                        Console.WriteLine();
                    }

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
                            string strbyte = System.Text.Encoding.Default.GetString(by);

                            strbyte = strbyte.Remove(0, strbyte.IndexOf("?>") + 2); //удаляем объявление xml
                            string strclose = strbyte.Remove(0, strbyte.IndexOf("\"") + 1);// выделяем свойство Close ,которое указывает, замкнут ли список
                            strclose = strclose.Remove(strclose.IndexOf("\""), strclose.Length - strclose.IndexOf("\""));
                            strbyte = strbyte.Remove(0, strbyte.IndexOf(">") + 1); // удаляем родительский элемент из разметки вначале
                            strbyte = strbyte.Remove(strbyte.LastIndexOf("<"), strbyte.Length - strbyte.LastIndexOf("<")); // удаляем родительский элемент вконце разметки

                            // создаём двусвязный список из строк
                            XmlToObject xml = new XmlToObject();
                            ListNode[] list = xml.Create(xml.StrElem(strbyte));

                            Console.WriteLine("Список восстановлен!");
                            Console.WriteLine();

                            Head = list[0];
                            Tail = list[^1];
                            Count = list.Length;

                            if (strclose == "1") // если вначале текста была еденица, то список замкнут.делаем связь между первым и последним элементом
                            {
                                Head.Previous = Tail;
                                Tail.Next = Head;
                                Console.WriteLine("Список замкнут");
                                Console.WriteLine();
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




        static void Main()
        {
            // создаем коллекцию двусвязного списка
            ListNode ln = new ListNode { Data = "Gena" };
            ListNode ln1 = new ListNode { Data = "<Petya RogovЁ>" };
            ListNode ln2 = new ListNode { Data = "Vasya" };
            ListNode ln3 = new ListNode();
            ListNode ln4 = new ListNode { Random = ln1 };
            ListNode ln5 = new ListNode { Previous = ln4, Data = "Goba\"" };
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

            ListRead listr = new ListRead(lr.Head, lr.Count);
            // создаем каталог для файла
            string path = @"C:\New";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            ListRandom lr1 = new ListRandom();
            //try
            //{
            FileStream stream = new FileStream(@$"{path}\note.xml", FileMode.Create);
            lr.Serialize(stream);
            FileStream stream1 = File.OpenRead(@$"{path}\note.xml");
            lr1.Deserialize(stream1);
            //}
            //catch
            //{
            //    Console.WriteLine("Проблема с потоком, файл невозможно создать/записать/прочитать");
            //}

            // чтение списка

            _ = new ListRead(lr1.Head, lr1.Count);

        }
    }
}
