using System;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace TwoDGameEngine
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
        public static void SaveValues()
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
        public static async void AutoSaveValues()
        {
            while (true)
            {
                while (AutoSave)
                {
                    SaveValues();

                    Console.WriteLine("Config has been saved.");

                    await Task.Delay(AutoSaveInterval);
                }

                await Task.Delay(AutoSaveInterval);
            }
        }

        //Load values from config
        public static void LoadConfig()
        {
            if (File.Exists(ConfigPath))
            {

                //Sets players info to config values
                Values state = new Values();

                XmlSerializer serializerPlayer = new XmlSerializer(typeof(Values));
                using (FileStream fsEnemies = File.OpenRead(ConfigPath))
                {
                    state = (Values)serializerPlayer.Deserialize(fsEnemies);
                }

                int someInt = 1;
                string someString = "test";

                //Sets application info to config values
                someInt = state.var1;
                someString = state.var2;
            }
            else
            {
                //Create configs if they do not exist
                Directory.CreateDirectory($"{ Environment.SpecialFolder.ApplicationData }" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
                File.Create(ConfigPath);
            }
        }

        //Store values to config
        public static void WriteConfig(Values v)
        {
            if (File.Exists(ConfigPath))
            {
                //write player values to file in %appdata%
                XmlSerializer serializerPlayer = new XmlSerializer(typeof(Values));
                using (TextWriter twPlayer = new StreamWriter(ConfigPath))
                {
                    serializerPlayer.Serialize(twPlayer, v);
                }
            }
        }
    }
}