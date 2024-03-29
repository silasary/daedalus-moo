﻿using System;
using System.Collections.Generic;
using System.Text;
using Chiroptera.Base;
using Chiroptera.Win.MOO;
using System.Text.RegularExpressions;

namespace Chiroptera.Win.MCP
{
    class MCPHandler
    {
        public static MCPHandler _mcpHandler;

        public string AuthenticationKey;

        public List<MCPPackage> Packages = new List<MCPPackage>();
        private Dictionary<string, MCPPackage> Commands = new Dictionary<string, MCPPackage>();
        private Dictionary<string, string> Multilines = new Dictionary<string, string>();

        public MCPHandler()
        {
            _mcpHandler = this;
            Packages.Add(new Packages.MCPNegotiate(this));
            Packages.Add(new Packages.AwnsStatus());
            Packages.Add(new Packages.VMooUserlist());
            Packages.Add(new Packages.VMooSmartComplete());
            Packages.Add(new Packages.SimpleEdit());
            Packages.Add(new Packages.KamahlIDE());
        }

        internal void ReceiveOOB(string s)
        {
            s = s.Remove(0, 3); // #$#
            string command =  s.Split(' ')[0];
            Dictionary<string, string> KeyVals = CreateKeyvals(s);
            if (command == "*") // Multiline
            {
                string dataTag = s.Split(' ')[1];
                if (Multilines.ContainsKey(dataTag))
                    command = Multilines[dataTag];
                else
                    return;
                KeyVals.Add("_data-tag", dataTag);
            }
            if (command == ":")
            {
                string dataTag = s.Split(' ')[1];
                if (Multilines.ContainsKey(dataTag))
                    command = Multilines[dataTag] + "-close";
                else
                    return;
                KeyVals.Add("_data-tag", dataTag);
                this.Multilines.Remove(dataTag);
            }
            if (command == "mcp")
            {
                HandleNegotiation(command, KeyVals);
            }
            else if (Commands.ContainsKey(command))
            {
                Commands[command].HandleMessage(command, KeyVals);
            }
            else
            {
                foreach (MCPPackage package in Packages)
                {
                    if (command.StartsWith(package.PackageName))
                    {
                        Commands.Add(command, package);
                        package.HandleMessage(command, KeyVals);
                        break;
                    }
                }
            }
        }
        public void SendOOB(string line)
        {
            if (!line.StartsWith("#$#"))
                throw new ArgumentException("Not an OOB line.");
            ClientCore.s_clientCore.SendLine(line);
        }
        public void SendOOB(string command, Dictionary<string, string> KeyVals)
        {
            if (command.Contains(" "))
                throw new ArgumentException("Command cannot contain whitespace");
            StringBuilder line = new StringBuilder("#$#").Append(command).Append(" ");
            if (AuthenticationKey != null)
                line.Append(AuthenticationKey).Append(" ");
            foreach (string key in KeyVals.Keys)
            {
                line.Append(key).Append(": ");
                line.Append(KeyVals[key]).Append(" ");
            }
            SendOOB(line.ToString());
        }

        public static Dictionary<string, string> CreateKeyvals(string s)
        {
            Dictionary<string,string> KeyVals = new Dictionary<string,string>();
            StringBuilder value = new StringBuilder();
            string name = null;
            string[] words = s.Split(' ');
            foreach (string word in words)
            {
                if (word.EndsWith(":"))
                {
                    if (name != null) // Finalize the previous keyval.
                    {
                        KeyVals.Add(name, value.ToString().Trim());
                    }
                    name = word.Substring(0, word.Length - 1);
                    value = new StringBuilder();
                    
                }
                else
                {
                    value.Append(word).Append(' ');
                }
            }
            if (name != null) // Finalize the final keyval.
            {
                KeyVals.Add(name, value.ToString().Trim());
            }
            return KeyVals;
        }
        public static Dictionary<string, string> CreateKeyvals(params string[] args)
        {
            Dictionary<string, string> KeyVals = new Dictionary<string, string>();
            for (int i = 0; i < args.Length; i++)
            {
                KeyVals.Add(args[i].Trim(' ', ':'), args[++i].Trim());
            }
            return KeyVals;
        }
        public static bool ContainsKeys(Dictionary<string, string> dict, params string[] Keys)
        {
            foreach (string key in Keys)
            {
                if (!dict.ContainsKey(key))
                    return false;
            }
            return true;
        }
        public MCPPackage FindPackage(string PackageName)
        {
            foreach (MCPPackage package in Packages)
            {
                if (PackageName.Trim() == package.PackageName)
                {
                    return package;
                }
            }
            return null;
        }

        public bool MCPEnabled { get; private set; }

        private void HandleNegotiation(string command, Dictionary<string, string> KeyVals)
        {
            if (command == "mcp")
            {
                if (ContainsKeys(KeyVals, "version", "to")) // Authentication
                { 
                    if (!VersionSupported(KeyVals["version"], KeyVals["to"], "2.1", "2.1"))
                        return; // Sorry, wrong version.
                    AuthenticationKey = new Random().NextDouble().GetHashCode().ToString();
                    SendOOB("#$#mcp authentication-key: " + AuthenticationKey + " version: 2.1 to: 2.1");
                    foreach (MCPPackage package in Packages)
                    {
                        try
                        {
                            SendOOB("mcp-negotiate-can", CreateKeyvals("package", package.PackageName, "min-version", package.minVer, "max-version", package.maxVer));
                        }
                        catch { Packages.Remove(package); }
                    }
                    SendOOB("mcp-negotiate-end",CreateKeyvals());
                }
            }
        }

        public static bool VersionSupported(string svrMin, string svrMax, string cltMin, string cltMax)
        {
            if (new Version(svrMax).CompareTo(new Version(cltMin)) < 0)
                return false; // They're using an older version
            else if (new Version(svrMin).CompareTo(new Version(cltMax)) > 0)
                return false; // They're using a newer version
            else
                return true; // They're using a version within our supported range
        }

        public static void RegisterMultilineHandler(string DataTag, string FauxCommand)
        {
            _mcpHandler.Multilines.Add(DataTag.Trim(), FauxCommand);
        }
    }
}
