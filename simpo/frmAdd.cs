using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Simpo
{
    public delegate void RefreshDelegate(Info objInfo, Boolean f_bEdit);
    public delegate void UpdateDelegate(Boolean bUpdate);
    
    public partial class frmAdd : Form
    {
        private DataStore m_objDataStore = new DataStore();
        public RefreshDelegate RefreshDelegateCallback;
        public UpdateDelegate UpdateDelegateCallback;
        private Boolean m_bEdit = false;
        private Info m_objInfo;
        private int m_nIndex = -1;

        public DataStore PropData
        {
            get
            {
                return m_objDataStore;
            }
        }

        public frmAdd()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            //Clear all text boxes
            txtPass.ResetText();
            txtComment.ResetText();
            txtSite.ResetText();
            txtUsername.ResetText();

            //Hide form
            Hide();
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            if (AddEntry())
            {
                //Clear all text boxes
                txtPass.ResetText();
                txtComment.ResetText();
                txtSite.ResetText();
                txtUsername.ResetText();

                //Hide form
                Hide();
            }
        }

        private bool AddEntry()
        {
            if (txtSite.Text == "")
            {
                MessageBox.Show("Please enter a valid site name");
                txtSite.Focus();
            }
            else if (txtUsername.Text == "")
            {
                MessageBox.Show("Please enter a valid username");
                txtUsername.Focus();
            }
            else if (txtPass.Text == "")
            {
                MessageBox.Show("Please enter a valid username");
                txtPass.Focus();
            }
            else
            {
                Info objInfo = new Info();

                //Set up information object
                objInfo.siteprop = txtSite.Text;
                objInfo.usernameprop = txtUsername.Text;
                objInfo.passprop = txtPass.Text;
                objInfo.commentprop = txtComment.Text;

                if (false == m_bEdit)
                {
                    //Maintain list of information objects
                    m_objDataStore.AddData(objInfo);
                }
                else
                {
                    PropData.ListProp[m_nIndex] = objInfo;
                }

                //Fire the callback and send new data
                RefreshDelegateCallback(objInfo,m_bEdit);
                UpdateDelegateCallback(true);
                return true;
            }

            return false;
        }

        public void EditDelegateFn(Info f_objInfo, bool f_bEdit, int f_nIndex)
        {
            m_objInfo = f_objInfo;
            m_bEdit = f_bEdit;
            m_nIndex = f_nIndex;
        }
               
        private void frmAdd_Shown(object sender, EventArgs e)
        {
            if (true == m_bEdit)
            {
                txtSite.Text = m_objInfo.siteprop;
                txtUsername.Text = m_objInfo.usernameprop;
                txtPass.Text = m_objInfo.passprop;
                txtComment.Text = m_objInfo.commentprop;
            }
        }
    }
}
