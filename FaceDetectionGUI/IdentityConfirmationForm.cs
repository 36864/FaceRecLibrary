using FaceRecLibrary.Types;
using System;
using System.Windows.Forms;

namespace FaceDetectionGUI
{
    public partial class IdentityConfirmationForm : Form
    {
        public IdentityInfo chosenIdentity;

        private IdentityInfo[] identities;

        public IdentityConfirmationForm(IdentityInfo[] identities)
        {
            InitializeComponent();
            if (identities.Length < 1) {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            listViewIdentities.View = View.Details;
            this.identities = identities;
            foreach (IdentityInfo id in identities)
            {
                ListViewItem lvi = new ListViewItem(id.Name);
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, id.Label.ToString()));
                listViewIdentities.Items.Add(lvi);
            }
            ListViewItem lviNew = new ListViewItem(identities[0].Name);
            lviNew.SubItems.Add(new ListViewItem.ListViewSubItem(lviNew, "NEW"));
            listViewIdentities.Items.Add(lviNew);
        }

        private void listViewIdentities_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewIdentities.SelectedIndices.Count == 1 && listViewIdentities.SelectedIndices[0] > -1) {
                if (listViewIdentities.SelectedIndices[0] < identities.Length)
                {
                    this.chosenIdentity = identities[listViewIdentities.SelectedIndices[0]];
                }
                else
                    this.chosenIdentity = new IdentityInfo(identities[0].Name);
                }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
