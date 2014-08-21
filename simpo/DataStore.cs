using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Simpo
{
    [Serializable()]
    public class DataStore : ISerializable
    {
        private List<Info> objList = new List<Info>();

        public List<Info> ListProp 
        { 
            get 
            {
                return objList;
            }
            set
            {
                this.objList = value;
            }
        }

        public DataStore() { }

        public DataStore(SerializationInfo info, StreamingContext contxt)
        {
            this.objList = (List<Info>)info.GetValue("Info", typeof(List<Info>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext contxt)
        {
            info.AddValue("Info", this.objList);
        }

        public void AddData(Info f_objInfo)
        {
            //Add information object to queue

            objList.Add(f_objInfo);
        }
    }
}
