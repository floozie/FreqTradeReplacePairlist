using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FreqTradeReplacePairlist
{
    class Program
    {
        private static string mgmconfig = @"./user_data/mgm-config.json";
        static void Main(string[] args)
        {
            // Step 1: test for null.
            if (args == null)
            {
                Console.WriteLine("Usage:\nFreqTradeReplacePairlist bigpl\n or\nFreqTradeReplacePairlist smallpl");
            }
            else
            {

                string argument = args[0];
                if (argument.Trim().ToUpper() == "SMALLPL"
                || argument.Trim().ToUpper() == "BIGPL")
                {
                    replace(argument);
                    Console.WriteLine("pairlist replaced in "+mgmconfig);

                }
                else
                {
                    Console.WriteLine("Usage:\nFreqTradeReplacePairlist bigpl\n or\nFreqTradeReplacePairlist smallpl");
                }

            }
        }

        
        private static void replace(string type)
        {
            string pairlistfile = "";
            string configcontent = "";
            
            if (type.Trim().ToUpper() == "SMALLPL")
            {
                pairlistfile = @"./user_data/smallpl.json";
            }
            else if(type.Trim().ToUpper() == "BIGPL")
            {
                pairlistfile = @"./user_data/bigpl.json";
            }

            string pairlist = "";
            using (StreamReader sr = new StreamReader(pairlistfile))
            {
                pairlist = sr.ReadToEnd();
                pairlist = pairlist.Trim();
            }
            
            using (StreamReader sr = new StreamReader(mgmconfig))
            {
                configcontent = sr.ReadToEnd();
                configcontent = configcontent.Trim();
            }

            
            int blacklistpos = configcontent.IndexOf("\"pair_whitelist\": [");
            int blacklistposclosingbracketpos = configcontent.IndexOf("]",blacklistpos);
            string oldwhitelist = configcontent.Substring(blacklistpos,blacklistposclosingbracketpos-blacklistpos+1);
            configcontent = configcontent.Replace(oldwhitelist,pairlist);
          

            using (StreamWriter sw = new StreamWriter(mgmconfig))
            {
                sw.WriteLine(configcontent);       
            }
        }
    }
}
