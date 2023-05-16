using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml;


namespace ErrorHandling_EventViewer
{
    class Program
    {
        //ninterval in mins


        /// <summary>
        /// 1. Log Name
        /// 2. Interval in hours
        /// 3. Path where the file is stored. Include xml file name
        /// 4. Sources
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            List<String> sSources = new List<string>();
            for (int i = 2; i < args.Length; i++)
            {
                sSources.Add(args[i]);
            }
            string sLog = args[0];
            int nInterval = Convert.ToInt32(args[1]);
            EventLog myLog = new EventLog();
            myLog.Log = sLog == "" ? "Application" : sLog;
            string path = @args[2];

            using (XmlWriter writer = XmlWriter.Create(path))
            {
                writer.WriteStartElement("ErrorHandling");
                writer.WriteStartElement("Events");

                foreach (EventLogEntry entry in myLog.Entries)
                {
                    if (entry.TimeGenerated > DateTime.Now.AddHours(-(nInterval)))
                    {
                        if (sSources.Contains(entry.Source))
                        {

                            writer.WriteStartElement("Event");
                            writer.WriteElementString("MachineName", System.Environment.MachineName);
                            writer.WriteElementString("Level", entry.EntryType.ToString());
                            writer.WriteElementString("DateTime", entry.TimeGenerated.ToString());
                            writer.WriteElementString("Source", entry.Source.ToString());
                            writer.WriteElementString("Category", entry.Category.ToString());
                            writer.WriteElementString("Message", entry.Message.ToString());
                            writer.WriteEndElement();
                            
                        }
                    }
                }

                writer.WriteEndElement();
                
                writer.WriteEndElement();
            }

            
        }
    }
}
