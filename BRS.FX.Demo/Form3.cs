using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Toolkit;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BRS.FX.Demo
{
    public partial class Form3 : KryptonForm
    {
        public Form3()
        {
            InitializeComponent();
        }
        private int _count = 1;
        private byte[] _array1;
        private byte[] _array2;
        private byte[] _array3;

        private void kryptonRibbonGroupButton1_Click(object sender, System.EventArgs e)
        {
            kryptonDockingManager.AddDockspace("Control", DockingEdge.Left, new KryptonPage[] { NewPropertyGrid() });

        }
        private KryptonPage NewPage(string name, int image, Control content, Size? autoHiddenSizeHint = null)
        {
            // Create new page with title and image
            KryptonPage p = new KryptonPage
            {
                Text = name,
                TextTitle = name,
                TextDescription = name,
                //ImageSmall = (Bitmap)imageListSmall.Images[image]
            };

            // Add the control for display inside the page
            content.Dock = DockStyle.Fill;
            p.Controls.Add(content);
            //p.Size = new Size(300, 400);
            if (autoHiddenSizeHint.HasValue)
            {
                p.AutoHiddenSlideSize = autoHiddenSizeHint.Value;
            }
            return p;
        }
        private KryptonPage NewPage(string name, int image, Control content)
        {
            // Create new page with title and image
            KryptonPage p = new KryptonPage
            {
                Text = name + _count.ToString(),
                TextTitle = name + _count.ToString(),
                TextDescription = name + _count.ToString()
            };
            p.UniqueName = p.Text;
            //p.ImageSmall = (Bitmap)imageListSmall.Images[image];

            // Add the control for display inside the page
            content.Dock = DockStyle.Fill;
            p.Controls.Add(content);

            _count++;
            return p;
        }

        private KryptonPage NewDocument()
        {
            KryptonPage page = NewPage("Document ", 0, new ContentDocument());

            // Document pages cannot be docked or auto hidden
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);

            return page;
        }

        private KryptonPage NewInput()
        {
            return NewPage("Input ", 1, new ContentInput());
        }

        private KryptonPage NewPropertyGrid()
        {
            return NewPage("Properties ", 2, new ContentPropertyGrid());
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            // Setup docking functionality
            KryptonDockingWorkspace w = kryptonDockingManager.ManageWorkspace(kryptonDockableWorkspace);
            kryptonDockingManager.ManageControl(kryptonPanel, w);
            kryptonDockingManager.ManageFloating(this);

            // Add docking pages
            //kryptonDockingManager.AddDockspace("Control", DockingEdge.Left, new KryptonPage[] { NewPropertyGrid() });
            //kryptonDockingManager.AddDockspace("Control", DockingEdge.Bottom, new KryptonPage[] { NewInput(), NewInput() });
            //kryptonDockingManager.AddAutoHiddenGroup("Control", DockingEdge.Right, new KryptonPage[] { NewPropertyGrid() });
            //kryptonDockingManager.AddToWorkspace("Workspace", new KryptonPage[] { NewDocument(), NewDocument(), NewDocument() });

        }

        private void kryptonDockingManager_GlobalSaving(object sender, DockGlobalSavingEventArgs e)
        {
            // Example code showing how to save extra data into the global config
            e.XmlWriter.WriteStartElement("CustomGlobalData");
            e.XmlWriter.WriteAttributeString("SavedTime", DateTime.Now.ToString(@"u"));
            e.XmlWriter.WriteEndElement();
        }

        private void kryptonDockingManager_GlobalLoading(object sender, DockGlobalLoadingEventArgs e)
        {
            // Example code showing how to reload the extra data that was saved into the global config
            e.XmlReader.Read();
            Console.WriteLine("GlobalConfig was saved at {0}", e.XmlReader.GetAttribute("SavedTime"));
            e.XmlReader.Read();
        }

        private void kryptonDockingManager_PageSaving(object sender, DockPageSavingEventArgs e)
        {
            // Example code showing how to save extra data into the page config
            e.XmlWriter.WriteStartElement("CustomPageData");
            e.XmlWriter.WriteAttributeString("SavedMilliseconds", DateTime.Now.Millisecond.ToString());
            e.XmlWriter.WriteEndElement();
        }

        private void kryptonDockingManager_PageLoading(object sender, DockPageLoadingEventArgs e)
        {
            // Example code showing how to reload the extra data that was saved into the page config
            e.XmlReader.Read();
            Console.WriteLine("PageConfig was saved at {0}", e.XmlReader.GetAttribute("SavedMilliseconds"));
            e.XmlReader.Read();
        }

        private void kryptonContextMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void kryptonRibbonGroupButton3_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                kryptonDockingManager.LoadConfigFromFile(openFileDialog.FileName);
            }
        }

        private void kryptonRibbonGroupButton2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                kryptonDockingManager.SaveConfigToFile(saveFileDialog.FileName);
            }
        }
    }
}