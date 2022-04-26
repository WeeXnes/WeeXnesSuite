#pragma warning disable CS0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wx
{
    public class wxfile
    {
        public string path { get; set; }
        public wxfile(string _path)
        {
            this.path = _path;
        }
        public string GetName()
        {
            string returnval = null;
            string[] rawcontent = wxfilefuncs.readFile(this.path);

            if (wxfilefuncs.verify(rawcontent))
            {
                try
                {
                    returnval = rawcontent[1];
                }
                catch (Exception e)
                {
                    returnval = null;
                }
            }

            return returnval;
        }
        public string GetValue()
        {
            string returnval = null;
            string[] rawcontent = wxfilefuncs.readFile(this.path);

            if (wxfilefuncs.verify(rawcontent))
            {
                try
                {
                    returnval = rawcontent[2];
                }catch (Exception e)
                {
                    returnval = null;
                }
            }

            return returnval;
        }
    }
    public class wxfilefuncs
    {

        public static string[] readFile(string filepath)
        {
            string[] lines = System.IO.File.ReadAllLines(filepath);
            var listOfStrings = new List<string>();
            foreach (string line in lines)
            {
                listOfStrings.Add(line);
            }
            string[] arrayOfStrings = listOfStrings.ToArray();
            return arrayOfStrings;
        }
        public static bool verify(string[] content)
        {
            bool integ = false;
            if(content != null)
            {
                if(content[0] == "##WXfile##")
                {
                    integ = true;
                }
            }
            return integ;
        }
    }
}
