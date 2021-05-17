using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SharePoint.Client;

namespace AddingContentTypeToListandLib
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// To add content types to the list 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            using(ClientContext clientContext = new ClientContext("http://vm-021-tzc02/sites/CSOMSite/"))
            {
                Web web = clientContext.Web;
                List myLib =  web.Lists.GetByTitle("Documents");
                clientContext.Load(myLib, lib => lib.ContentTypesEnabled);
                clientContext.ExecuteQuery();
                // check if contenttype is enabled if not then enable and update the list or Library
                if (!myLib.ContentTypesEnabled)
                {
                    myLib.ContentTypesEnabled = true;
                    myLib.Update();
                    clientContext.ExecuteQuery();
                }
                // Load content types from the rootweb
                ContentTypeCollection contentTypes = clientContext.Site.RootWeb.ContentTypes;
                clientContext.Load(contentTypes);
                clientContext.ExecuteQuery();
                // Add the exixting content type to the list and then update the list
                ContentType ctype = contentTypes.Where(c => c.Name == "Wiki Page").First();
                myLib.ContentTypes.AddExistingContentType(ctype);
                myLib.Update();
                clientContext.ExecuteQuery();

                MessageBox.Show("Content type Enabled");
            }
        }
    }
}
