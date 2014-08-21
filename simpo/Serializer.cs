using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Simpo
{
    public class Serializer
    {
        public Serializer() { }

        //Serialize our list and write to file
        public void Serialize(DataStore objStore)
        {
            Stream oStream = File.Open("dbinfo.dat", FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            bformatter.Serialize(oStream, objStore);
            oStream.Close();
        }

        //Read list from file and deserialize
        public DataStore DeSerialize()
        {
            try
            {
                DataStore objStore;
                Stream iStream = File.Open("dbinfo.dat", FileMode.Open);
                BinaryFormatter objBF = new BinaryFormatter();
                objStore = (DataStore)objBF.Deserialize(iStream);
                iStream.Close();
                return objStore;
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return null;
        }
    }
}
