using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Simpo
{
    [Serializable()]
    public class Info : ISerializable
    {
        private string site;
        private string username;
        private string pass;
        private string comment;

        public Info() { }

        public Info(SerializationInfo info, StreamingContext cont)
        {
            this.site = (string)info.GetValue("Site", typeof(string));
            this.username = (string)info.GetValue("Username", typeof(string));
            this.pass = (string)info.GetValue("Pass", typeof(string));
            this.comment = (string)info.GetValue("Comment", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext cont)
        {
            info.AddValue("Site", this.site);
            info.AddValue("Username", this.username);
            info.AddValue("Pass", this.pass);
            info.AddValue("Comment", this.comment);
        }

        public string siteprop 
        { 
            get
            {
               return site;
            } 
            set
            {
                site = value;
            }
        }

        public string usernameprop
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }

        public string passprop
        {
            get
            {
                return pass;
            }
            set
            {
                pass = value;
            }
        }

        public string commentprop
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value;
            }
        }
        
    }
}
