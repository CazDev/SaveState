using System;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace SaveState
{
    public static class SaveState
    {
        public static string ConfigPath = $"{ Environment.SpecialFolder.ApplicationData }" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "config.xml";

        //Controls AutoSave is enabled or not
        public static bool AutoSave = true;
        //AutoSave interval
        public static int AutoSaveInterval = 10000;

        public class Values
        {
            //declare values to be saved
            public int var1 { get; set; }
            public string var2 { get; set; }
            //TODO: Other variables still need to be added here
        }

        //Saves values ready to be stored
        public static void SaveConfig(Values values)
        {
            //update values
            Values state = new Values();

            //Values
            state.var1 = 1;
            state.var2 = "test";

            //writes values to config
            WriteConfig(state);
        }

        //AutoSave values
        public static async void AutoSaveValues(Values values)
        {
            while (true)
            {
                while (AutoSave)
                {
                    SaveConfig(values);

                    Console.WriteLine("Config has been saved.");

                    await Task.Delay(AutoSaveInterval);
                }

                await Task.Delay(AutoSaveInterval);
            }
        }

        //Load values from config
        public static void LoadConfig(Values values)
        {
            if (File.Exists(ConfigPath))
            {

                //Sets players info to config values
                Values state = new Values();

                XmlSerializer serializer = new XmlSerializer(typeof(Values));
                using (FileStream fs = File.OpenRead(ConfigPath))
                {
                    state = (Values)serializer.Deserialize(fs);
                }
                //Sets application info to config values
                //TODO: change loaded values
                values.val1 = state.val1;
                values.val2 = state.val2
            }
            else
            {
                //Create configs if they do not exist
                Directory.CreateDirectory($"{ Environment.SpecialFolder.ApplicationData }" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
                File.Create(ConfigPath);
            }
        }

        //Store values to config
        public static void WriteConfig(Values values)
        {
            if (!File.Exists(ConfigPath))
            {
                File.Create(ConfigPath).Dispose();
            }

            //write player values to file in %appdata%
            XmlSerializer serializer = new XmlSerializer(typeof(Values));
            using (TextWriter tw = new StreamWriter(ConfigPath))
            {
                serializer.Serialize(tw, values);
            }
        }
    }
}
