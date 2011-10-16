using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Daedalus.MOO
{
    public class Interop
    {
        public static List<object> ParseMOOstruct(string moolist)
        {
            List<object> csList = new List<object>();
            moolist = moolist.Trim();
            if (moolist == "{}")
                return new List<object>(); // Otherwise string.Split() gives us an empty string inside it.
            if (moolist[0] != '{' || moolist[moolist.Length - 1] != '}')
                throw new ArgumentException("Not a List!");
            string[] contents = moolist.Substring(1, moolist.Length - 2).Split(',');
            for (int i = 0; i < contents.Length; i++)
            {
                string arg = contents[i].Trim();
                if (arg.StartsWith("#"))
                    csList.Add(new MOOObject(arg));
                else if (arg.StartsWith("\""))  // Doesn't work.  \"\\\\"hello\\\\", he said.\"
                {                               // But the method I tried to fix that was equally useless.  Find a solution.
                    if (!arg.EndsWith("\""))
                        throw new ArgumentException("Not a valid string");
                    csList.Add(arg.Substring(1, arg.Length - 2));
                    //string str = arg + ",";
                    //while (!(str.EndsWith("\",") && !str.EndsWith("\\\",")))
                    //{
                    //    str += contents[++i] + ",";
                    //}
                    //str.Replace("\\\"", "\"");
                    //csList.Add(str.Substring(1, str.Length - 3));
                }
                else if (arg.StartsWith("{"))
                {
                    int OpenBrackets = 0;
                    StringBuilder args = new StringBuilder();
                    do
                    {
                        OpenBrackets += Regex.Matches(arg, "{").Count;
                        OpenBrackets -= Regex.Matches(arg, "}").Count;
                        args.Append(arg);
                        if (OpenBrackets == 0)
                        {
                            break;
                        }
                        args.Append(", ");
                        arg = contents[++i];
                    } while (i < contents.Length);
                    csList.Add(ParseMOOstruct(args.ToString()));
                }
                else if (arg.Contains("."))
                    csList.Add(float.Parse(arg));
                else if (arg.StartsWith("E_"))
                    throw new NotImplementedException();
                else
                    csList.Add(int.Parse(arg));
            }
            return csList;
        }

        public static string Escape(string p)
        {
            return "\"" + p.Replace("\"", "\\\"") + "\"";
        }
    }
}
