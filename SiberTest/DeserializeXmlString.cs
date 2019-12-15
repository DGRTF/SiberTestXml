using SiberTest;
using System;
using System.Linq;

namespace DeserializeXmlString
{



    public class CreateElementListNode
    {
        public CreateElementListNode(string xmlString)
        {
            XmlString = xmlString;
        }

        string XmlString { get; }
        public ListNode[] ListNodes { get; private set; }

        public void Create()
        {
            // выделяем свойство Close головного элемента
            CloseListInXml closeListInXml = new CloseListInXml(XmlString);
            closeListInXml.CloseList();

            // Создаём двусвязный список в виде массива элементов
            CreateElementListfromXmlString createElementListfromXmlString = new CreateElementListfromXmlString(XmlString);
            createElementListfromXmlString.CreateElementListfromXml();
            ListNodes = createElementListfromXmlString.ListNodes;

            if (closeListInXml.StrClose == "1") // если вначале текста была еденица, то список замкнут.делаем связь между первым и последним элементом
            {
                ListNodes[0].Previous = ListNodes[^1];
                ListNodes[^1].Next = ListNodes[0];
                Console.WriteLine("Список замкнут");
            }
        }

    }

    public class CloseListInXml
    {
        string XmlString { get; set; }

        public string StrClose { get; private set; }

        public CloseListInXml(string xmlString)
        {
            XmlString = xmlString;
        }

        public void CloseList()
        {
            StrClose = XmlString.Remove(0, XmlString.IndexOf("\">") - 1);// выделяем свойство Close ,которое указывает, замкнут ли список
            StrClose = StrClose.Remove(StrClose.IndexOf("\""), StrClose.Length - StrClose.IndexOf("\""));
        }
    }

    public class CreateElementListfromXmlString
    {

        string XmlString { get; }
        public ListNode[] ListNodes { get; private set; }


        public CreateElementListfromXmlString(string xmlString)
        {
            XmlString = xmlString;
        }

        public void CreateElementListfromXml()
        {
            // выводим массив элементов в строковом виде Xml
            SearchElementToXml searchEl = new SearchElementToXml(XmlString);
            searchEl.StrElem();

            // создаём двусвязный список
            CreateElementsList createElementList = new CreateElementsList(searchEl.ObjXml);
            createElementList.Create();
            ListNodes = createElementList.Lis;
        }
    }

    public class SearchElementToXml
    {
        int Count { get; set; } // счётчик элементов

        string XmlString { get; set; } // строка объектов xml

        public string[] ObjXml { get; private set; } // массив объектов в xml разметке в виде строк


        public SearchElementToXml(string xmlString)
        {
            XmlString = xmlString;
        }


        public void StrElem() // возвращаем массив xml строк каждого элемента
        {
            ClearStringXml clearStringXml = new ClearStringXml(XmlString);
            clearStringXml.ClearString();
            XmlString = clearStringXml.XmlString;

            CountElementInXml();

            ObjXml = new string[Count];
            int count = 0;

            while (XmlString.IndexOf("<") != -1) // пока элементы не закончились
            {
                // находим элемент, добавляем в массив и удаляем
                ObjXml[count] = XmlString.Remove(XmlString.IndexOf(">") + 1, XmlString.Length - XmlString.IndexOf(">") - 1); // добавляем первый элемент в массив
                XmlString = XmlString.Remove(0, XmlString.IndexOf(">") + 1);// удаляем первый элемент
                count++;
            }

        }


        void CountElementInXml() // находим количество элементов в разметке
        {
            Count = 0;

            foreach (char i in XmlString.ToArray())
            {
                if (i == '<')
                    Count++;
            }
        }

    }

    public class ClearStringXml
    {
        public string XmlString { get; private set; }

        public ClearStringXml(string xmlString)
        {
            XmlString = xmlString;
        }

        public void ClearString()
        {
            XmlString = XmlString.Remove(0, XmlString.IndexOf("?>") + 2); //удаляем объявление xml
            XmlString = XmlString.Remove(0, XmlString.IndexOf(">") + 1); // удаляем родительский элемент из разметки вначале
            XmlString = XmlString.Remove(XmlString.LastIndexOf("<"), XmlString.Length - XmlString.LastIndexOf("<")); // удаляем родительский элемент из разметки вконце
        }
    }



    public class CreateElementsList
    {

        string[] ObjXml { get; set; } // массив объектов в xml разметке в виде строк


        public ListNode[] Lis { get; private set; }


        public CreateElementsList(string[] objXml)
        {
            ObjXml = objXml;
        }



        public void Create()// Создаём список объектов из массива строк xml
        {
            Lis = new ListNode[ObjXml.Length];
            InitializeListNodeArr();

            InitializePropertiesListNode initializePropertiesListNode = new InitializePropertiesListNode(Lis, ObjXml);
            initializePropertiesListNode.InitializeProperties();
            Lis = initializePropertiesListNode.ListNode;
        }


        void InitializeListNodeArr() // инициализируем массив объектов listNode
        {

            for (int i = 0; i < Lis.Length; i++)
            {
                Lis[i] = new ListNode();
                if (i != 0) // создаём двусвязную связь элементов массива
                {
                    Lis[i].Previous = Lis[i - 1];
                    Lis[i - 1].Next = Lis[i];
                }
            }
        }
    }

    public class InitializePropertiesListNode
    {
        public InitializePropertiesListNode(ListNode[] listNode, string[] objXml)
        {
            ListNode = listNode;
            ObjXml = objXml;
        }

        public ListNode[] ListNode { get; private set; }

        string[] ObjXml { get; }

        InitializeListNodeData initializeListNodeData { get; } = new InitializeListNodeData();

        InitializeRandomListNode initializeRandomListNode { get; } = new InitializeRandomListNode();



        public void InitializeProperties()
        {
            for (int i = 0; i < ObjXml.Length; i++)
            {
                initializeListNodeData.InitializeData(ObjXml[i]);
                ListNode[i].Data = initializeListNodeData.Data;

                initializeRandomListNode.InitializeRandomNumber(ObjXml[i]);
                if (initializeRandomListNode.Number != -1)
                    ListNode[i].Random = ListNode[initializeRandomListNode.Number];

            }
        }
    }

    public class InitializeListNodeData
    {

        public string Data { get; private set; }


        public void InitializeData(string objXml) //задаём свойство Data
        {
            Data = null;
            if (objXml.IndexOf("Data=\"") != -1) // если задано свойство Data, задаём его в объекте
            {
                GetStringProperties getString = new GetStringProperties();
                getString.GetStringPropertiesData(objXml);

                StringSymbol sb = new StringSymbol();
                sb.Characters(getString.ObjectXml);

                Data = sb.Symbol;
            }
        }
    }

    public class GetStringProperties
    {

        public string ObjectXml { get; private set; }


        public void GetStringPropertiesData(string objectXml)
        {
            objectXml = objectXml.Remove(0, objectXml.IndexOf("a=\"") + 3); // удаляем всё от первого символа до открывающейся кавычки включительно
            ObjectXml = objectXml.Remove(objectXml.IndexOf("\""), objectXml.Length - objectXml.IndexOf("\"")); // удаляем всё от последней(она же в текущей строке первая) кавычки до конца
        }
    }


    public class InitializeRandomListNode
    {

        public int Number { get; private set; }



        public void InitializeRandomNumber(string objectXml) // инициализируем массив объектов с номерами элементов и произвольных объектов
        {
            Number = -1;
            if (objectXml.IndexOf("Random=\"") != -1) // если задано свойство Random
            {
                objectXml = objectXml.Remove(0, objectXml.IndexOf("m=\"") + 3); // удаляем всё от первого символа до открывающейся кавычки включительно
                objectXml = objectXml.Remove(objectXml.IndexOf("\""), objectXml.Length - objectXml.IndexOf("\"")); // удаляем всё от последней(она же в текущей строке первая) кавычки до конца

                Number = Convert.ToInt32(objectXml);
            }

        }
    }



}
