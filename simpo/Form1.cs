using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Simpo
{
    public delegate void EditDelegate(Info f_objInfo, Boolean bEdit, int f_nIndex);

    public partial class Form1 : Form
    {
        public EditDelegate EditDelegateCallback;
        private Boolean m_bUpdate = false;
        private frmAdd objAdd = new frmAdd();
        private int m_nCnt = 0;

        public Boolean UpdateProp 
        {
            get
            {
                return m_bUpdate;
            }

            set
            {
                m_bUpdate = value;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeListControl();

            if (File.Exists("dbinfo.dat"))
            {
                LoadListFromFile();
            }

            //Register delegates
            EditDelegateCallback = new EditDelegate(objAdd.EditDelegateFn);
            objAdd.RefreshDelegateCallback = new RefreshDelegate(this.RefreshDelegateFn);
            objAdd.UpdateDelegateCallback = new UpdateDelegate(this.UpdateDelegateFn);

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void InitializeListControl()
        {
            //Set up list control
            listView1.Columns.Add("No.");
            listView1.Columns.Add("Site");
            listView1.Columns.Add("Username");
            listView1.Columns.Add("Password");
            listView1.Columns.Add("Comment");

            //Adjust sizes
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            //Make visible
            objAdd.Show();
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            //Set update flag
            UpdateProp = false;

            //Write all entries to file
            Serializer objSer = new Serializer();
            objSer.Serialize(objAdd.PropData);

            MessageBox.Show("Save was successful");
        }

        public void RefreshDelegateFn(Info objInfo, bool f_bEdit)
        {
            //string []arrItems = {(++m_nCnt).ToString(), objInfo.siteprop, objInfo.usernameprop,
            //    objInfo.passprop, objInfo.commentprop};

            //ListViewItem item = new ListViewItem(arrItems);

            //listView1.Items.Add(item);

            //Clear listview and refresh with new content
            listView1.Items.Clear();

            //Reset index
            m_nCnt = 0;

            List<Info> pList = objAdd.PropData.ListProp;

            foreach(Info oInfo in pList)
            {
                string[] arrItems = {(++m_nCnt).ToString(), oInfo.siteprop, oInfo.usernameprop,
                oInfo.passprop, oInfo.commentprop};

                ListViewItem item = new ListViewItem(arrItems);

                listView1.Items.Add(item);
            }
        }

        public void UpdateDelegateFn(Boolean bUpdate)
        {
            UpdateProp = bUpdate;
        }

        private void LoadListFromFile()
        {
            List<Info> objList = new List<Info>();
            DataStore objSto = new DataStore();
            Serializer objSer = new Serializer();

            objSto = objSer.DeSerialize();
            objList = objSto.ListProp;

            //Reset
            m_nCnt = 0;

            objList.ForEach(delegate(Info objInfo)
            {
                string[] arrItems = {(++m_nCnt).ToString(), objInfo.siteprop, objInfo.usernameprop,
                objInfo.passprop, objInfo.commentprop};

                ListViewItem item = new ListViewItem(arrItems);

                listView1.Items.Add(item);
            });

            //Update internal list in memory
            objAdd.PropData.ListProp = objList;

            //Adjust sizes
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UpdateProp == true)
            {
                DialogResult result;
                result = MessageBox.Show("Do you want to save the changes?", "Data Changed",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);

                if(result == DialogResult.Yes)
                {
                    //Set update flag
                    UpdateProp = false;

                    //Write all entries to file
                    Serializer objSer = new Serializer();
                    objSer.Serialize(objAdd.PropData);

                    MessageBox.Show("Save was successful");
                }
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            var index = listView1.SelectedIndices;
            int nLen = listView1.Items.Count;

            List<Info> objList = objAdd.PropData.ListProp;
            
            while(nLen != 0)
            {
                if (index.Contains(nLen - 1))
                {
                    listView1.Items.RemoveAt(nLen - 1);

                    objList.Remove(objList.ElementAt(nLen - 1));

                    //Set change flag
                    UpdateProp = true;

                    return;
                }
                nLen--;
            }

            MessageBox.Show("Please select the item to delete from the list");
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            var index = listView1.SelectedIndices;
            int nLen = listView1.Items.Count;

            List<Info> objList = objAdd.PropData.ListProp;

            while (nLen != 0)
            {
                if (index.Contains(nLen - 1))
                {
                    Info objInfo = new Info();
                    objInfo = objList.ElementAt(nLen - 1);
                    
                    //Call delegate
                    //True indicates editing, false
                    //indicates adding
                    EditDelegateCallback(objInfo, true, (nLen - 1));

                    //Show the add/edit form
                    objAdd.Show();

                    return;
                }
                nLen--;
            }

            MessageBox.Show("Please select the item to edit from the list");
        }
    }
}