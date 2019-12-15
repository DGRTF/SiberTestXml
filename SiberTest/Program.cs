using System;
using System.IO;
using System.Linq;
using SerializeXmlString;
using DeserializeXmlString;


namespace SiberTest
{

    /// <summary>
    /// Программа сериализует двусвязный список в xml разметку.
    /// Исходный файл сериализации стандартизирован под xml, поэтому его можно прочитать стандартными средствами десериализации .NET
    /// 
    /// Сериализация\Десериализация за O(n) за счёт добавления в ListNode поля 'int Number' содержащий номер элемента и последующем заданием номеров,
    /// что так же можно сделать с помощью класса имеющем те же поля и дополнительное поле int.
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


    public class ListRead
    {
        public ListRandom Random { get; private set; }
        public string Close { get; private set; }



        public ListRead(ListRandom random)
        {
            Random = random;
        }



        public void Read()//чтение списка
        {
            int count = Random.Count;

            while (count != 0)
            {
                Console.WriteLine("Element:" + Random.Head.Data);
                Console.Write("Родительский элемент:");
                if (Random.Head.Previous != null)
                    Console.WriteLine(Random.Head.Previous.Data);
                else
                {
                    Console.WriteLine();
                }

                Console.Write("Рандомный элемент:");
                if (Random.Head.Random != null)
                    Console.WriteLine(Random.Head.Random.Data);
                else
                {
                    Console.WriteLine();
                }

                Console.Write("Следующий элемент:");
                if (Random.Head.Next != null)
                    Console.WriteLine(Random.Head.Next.Data);
                else
                {
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine();

                Random.Head = Random.Head.Next;
                count--;
            }
        }


        public void ListClose() // возвращает true ,если список замкнут
        {
            int count = Random.Count;

            while (Random.Head.Previous != null)
            {
                if (count == 0)
                {
                    Close = "1";
                    return;
                }
                else
                {
                    Random.Head = Random.Head.Previous;
                    count--;
                }
            }
            Close = "0";
        }


        public void HeadEl()  // возвращает родительский элемент из незамкнутого списка
        {
            for (; ; )
            {
                if (Random.Head.Previous == null)
                    return;
                else
                    Random.Head = Random.Head.Previous;
            }
        }


        //public int Number(ListNode list) // номер объекта в списке отсчитывамый от нуля
        //{
        //    int number = 0;
        //    for (; ; )
        //    {
        //        if (list.Previous != null)
        //        {
        //            number++;
        //            list = list.Previous;
        //        }
        //        else
        //            return number;
        //    }

        //}

    }

    public class StringSymbol
    {

        public string Symbol { get; private set; }



        public void RepCharacters(string s) // меняем специальные символы на заменяющие их
        {
            s = s.Replace("&", "&amp;");
            s = s.Replace("<", "&lt;");
            s = s.Replace(">", "&gt;");
            s = s.Replace("\"", "&quot;");
            Symbol = s;
        }



        public void Characters(string s) // восттанавливаем специальные символы
        {
            s = s.Replace("&amp;", "&");
            s = s.Replace("&lt;", "<");
            s = s.Replace("&gt;", ">");
            s = s.Replace("&quot;", "\"");
            Symbol = s;
        }
    }


    public class ListNode
    {
        public ListNode Previous;
        public ListNode Next;
        public ListNode Random; // произвольный элемент внутри списка
        public string Data;
        public int Number; // номер элемента, отсчитываемый от нуля
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
                // подготовка списка
                PrepListNode prepListNode = new PrepListNode(this);
                prepListNode.Preparation();

                // созаём строку формата Xml
                CreateXmlDocument createXmlString = new CreateXmlDocument(this, prepListNode.ListRead.Close);
                createXmlString.CreateXml();

                // преобразуем текст в байты
                byte[] by = System.Text.Encoding.Default.GetBytes(createXmlString.XmlString);

                using (stream)// записываем в файл
                {
                    // запись массива байтов в файл
                    stream.Write(by, 0, by.Length);

                    Console.WriteLine("Текст записан в файл");
                    Console.WriteLine("-------------------------------------------------");
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
                        string xmlString = System.Text.Encoding.Default.GetString(by);

                        CreateElementListNode createElementListNode = new CreateElementListNode(xmlString);
                        createElementListNode.Create();

                        // 
                        Head = createElementListNode.ListNodes[0];
                        Tail = createElementListNode.ListNodes[^1];
                        Count = createElementListNode.ListNodes.Length;

                        Console.WriteLine("Список восстановлен!");
                        Console.WriteLine();

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

    class Program
    {
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
            ListRead lisr = new ListRead(lr);
            lisr.Read();
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

            ListRead lislr = new ListRead(lr1);
            lislr.Read();

        }
    }
}
