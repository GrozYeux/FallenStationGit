using System.Xml;   //Use of XmlDocument
using System.Text;
using System.Collections.Generic; //Use of dictionary
using UnityEngine;

public class NoteFileParser
{
    private static string RemoveExtraSpaces(string input)
    {
        StringBuilder tmpbuilder = new StringBuilder();
        
        bool inspaces = false;
        tmpbuilder.Length = 0;

        for (int i = 0; i < input.Length; ++i)
        {
            char c = input[i];
            if (i < 1 && c == '\n') c = ' ';
            if (i <= 1 && c == '\r') c = ' ';

            if (inspaces)
            {
                if (c != ' ')
                {
                    inspaces = false;
                    tmpbuilder.Append(c);
                }
            }
            else if (c == ' ')
            {
                inspaces = true;
                tmpbuilder.Append(' ');
            }
            else
                tmpbuilder.Append(c);
        }
        return tmpbuilder.ToString();
    }

   public static Note Load(TextAsset textAsset, bool keepSpaces=true )
    {
        //This dictionnary will hold any text data contained in the xml.
        // -- ex <thing>hello</thing> will become: dict[thing] = "hello"
        Dictionary<string, string> dict = new Dictionary<string, string>();

        //Load the XML document
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);
        //Add each xml node and content as key,value in the dictionary
        // - our xml root node is <record>...</record>. We consider only its children nodes.
        foreach (XmlNode n in doc.SelectNodes("/record/*")){
            //Remove extra whitespaces
            string innerText =  RemoveExtraSpaces(n.InnerXml);

            //Check if you want to remove the linefeed
            if (keepSpaces == false){
                innerText = innerText.Replace("\n", "").Replace("\r", "");
            }
            //Append to the dictionary
            dict.Add(n.Name, innerText); 
        }

        string title, author, date, body;
        
        //Multiple tests to try and check if the title, author, date and body are within the data
        if (!dict.TryGetValue("title", out title))
        {   //In case of failure, we will return a null object. We could also return an empty note
            //In case of success, the value is written under the out variable (here to title)
            return null;
        }
        if (!dict.TryGetValue("author", out author))
        {
            return null;
        }
        if (!dict.TryGetValue("date", out date))
        {
            return null;
        }
        if (!dict.TryGetValue("body", out body))
        {
            return null;
        }
        
        //The Note is instanciated using valid parameters
        return new Note(title, author, date, body);
    }

}
