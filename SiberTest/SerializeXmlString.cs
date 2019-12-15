using SiberTest;
using System;

namespace SerializeXmlString
{
    public class PrepListNode
    {

        ListRandom ListRandom { get; }

        public ListRead ListRead { get; private set; }

        NumirateElementsListNode numirateElementsListNode { get; } = new NumirateElementsListNode();


        public PrepListNode(ListRandom listRandom)
        {
            ListRandom = listRandom;
        }

        public void Preparation()
        {
            ListRead = new ListRead(ListRandom);
            ListRead.ListClose();

            if (ListRead.Close == "1")
                UnCloseListNode();
            else
                SearchHead();

            // номеруем элементы списка
            numirateElementsListNode.NumirateElement(ListRandom.Head);
        }


        void UnCloseListNode() // разрываем список
        {
            Console.WriteLine("Список является замкнутым!");
            ListRandom.Head.Previous.Next = null;
            ListRandom.Head.Previous = null;
        }

        void SearchHead()
        {
            Console.WriteLine("Cписок не замкнут");
            ListRead.HeadEl();
            ListRandom.Head = ListRead.Random.Head;
        }
    }

    public class NumirateElementsListNode
    {
        public ListNode NumirateElement(ListNode listNode)
        {
            ListNode list = listNode;
            int count = 0;
            while (listNode != null)
            {
                listNode.Number = count;
                listNode = listNode.Next;
                count++;
            }
            return list;
        }
    }





    public class CreateXmlDocument
    {

        ListRandom ListRandom { get; set; }

        string Close { get; }

        public string XmlString { get; private set; }


        public CreateXmlDocument(ListRandom listrandom, string close)
        {
            ListRandom = listrandom;
            Close = close;
        }
        public void CreateXml()
        {
            XmlString = "<?xml version=\"1.0\" encoding=\"utf-16\" ?>";
            XmlString += "<List Close =\"" + Close + "\">";

            // добавляем дочерние элементы в строку xml
            CreateObjStringXml createObjStringXml = new CreateObjStringXml(ListRandom.Head);
            createObjStringXml.CreateObjInString();

            // добавляем закрывающий тег корневого элемента
            XmlString += createObjStringXml.ObjXml + "</List>";
        }
    }

    public class CreateObjStringXml
    {

        public string ObjXml { get; private set; }


        readonly InitializePropertiesInXml initializePropertiesInXml = new InitializePropertiesInXml();

        ListNode ListNode { get; set; }

        string LastTeg { get; } = "/>";



        public CreateObjStringXml(ListNode listNode)
        {
            ListNode = listNode;
        }


        public void CreateObjInString()
        {
            ObjXml = "";
            while (ListNode != null)
            {
                initializePropertiesInXml.InitializeProperties(ListNode);
                ObjXml += "<Object " + initializePropertiesInXml.StringProperties + LastTeg;
                ListNode = ListNode.Next;
            }
        }

    }

    public class InitializePropertiesInXml
    {
        public string StringProperties { get; private set; }

        InitialisePropertiesDataInXml initialisePropertiesDataInXml = new InitialisePropertiesDataInXml();

        InitialisePropertiesRandomInXml initialisePropertiesRandomInXml = new InitialisePropertiesRandomInXml();


        public void InitializeProperties(ListNode listNode)
        {
            StringProperties = "";

            initialisePropertiesDataInXml.InitialisePropertiesData(listNode.Data);
            StringProperties = initialisePropertiesDataInXml.Data;
            if (listNode.Random != null)
            {
                initialisePropertiesRandomInXml.InitialisePropertiesRandom(listNode.Random.Number);
                StringProperties += initialisePropertiesRandomInXml.RandomStringNumber;
            }
        }
    }

    public class InitialisePropertiesDataInXml
    {
        public string Data { get; private set; }

        StringSymbol StringSymbol { get; } = new StringSymbol();

        public void InitialisePropertiesData(string data)
        {
            Data = "";
            if (data != null)
            {
                Data = data;
                InitialiseProperties();
            }
        }
        void InitialiseProperties()
        {
            StringSymbol.RepCharacters(Data);
            Data = "Data=\"" + StringSymbol.Symbol + "\" ";
        }
    }

    public class InitialisePropertiesRandomInXml
    {
        public string RandomStringNumber { get; private set; }


        public void InitialisePropertiesRandom(int randomNamber)
        {
            RandomStringNumber = "Random=\"" + randomNamber.ToString() + "\"";
        }
    }


}
