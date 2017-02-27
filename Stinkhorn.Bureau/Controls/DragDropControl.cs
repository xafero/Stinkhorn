using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static VirtualFileDataObject.VirtualFileDataObject;

namespace Stinkhorn.Bureau.Controls
{
    public partial class DragDropControl : UserControl
    {
        public DragDropMode Mode { get; }

        public DragDropControl() : this("Test", DragDropMode.Input)
        {
        }

        public DragDropControl(string title, DragDropMode mode)
        {
            InitializeComponent();
            Mode = mode;
            groupBox.Text = title;
            if (mode == DragDropMode.Input)
            {
                listView.AllowDrop = true;
                listView.DragEnter += ListView_DragEnter;
                listView.DragDrop += ListView_DragDrop;
            }
            listView.LargeImageList = new ImageList
            {
                Images = { Icons.FileUnknown }
            };
        }

        void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        void ListView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var path in filePaths)
                {
                    var info = new FileInfo(path);
                    var file = new FileDescriptor
                    {
                        ChangeTimeUtc = info.LastWriteTimeUtc,
                        Length = info.Length,
                        Name = info.Name,
                        StreamContents = s => File.OpenRead(path).CopyTo(s)
                    };
                    listView.Items.Add(new ListViewItem
                    {
                        Name = info.FullName,
                        ImageIndex = 0,
                        Text = file.Name,
                        Tag = file
                    });
                }
            }
        }

        public IEnumerable<FileDescriptor> Files => listView.Items
            .OfType<ListViewItem>().Select(i => i.Tag).OfType<FileDescriptor>();
    }

    public enum DragDropMode
    {
        Input, Output
    }
}