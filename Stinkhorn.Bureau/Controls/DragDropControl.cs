using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using static VirtualFileDataObject.VirtualFileDataObject;
using VFDO = VirtualFileDataObject.VirtualFileDataObject;

namespace Stinkhorn.Bureau.Controls
{
    public partial class DragDropControl : UserControl
    {
        readonly string DataFormatFileContent = "FileContents";
        readonly string DataFormatFileGroup = "FileGroupDescriptorW";

        public DragDropMode Mode { get; }

        public DragDropControl() : this("Test", DragDropMode.None)
        {
        }

        public DragDropControl(string title, DragDropMode mode)
        {
            InitializeComponent();
            Mode = mode;
            groupBox.Text = title;
            if (mode.HasFlag(DragDropMode.Input))
            {
                listView.AllowDrop = true;
                listView.DragEnter += ListView_DragEnter;
                listView.DragDrop += ListView_DragDrop;
            }
            if (mode.HasFlag(DragDropMode.Output))
            {
                listView.ItemDrag += ListView_ItemDrag;
            }
            listView.LargeImageList = new ImageList
            {
                Images = { Icons.FileUnknown }
            };
        }

        #region Output mode
        void ListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (!(e.Button == MouseButtons.Left))
                return;
            var item = (ListViewItem)e.Item;
            var file = item.Tag as FileDescriptor;
            if (file == null)
                return;
            var virtFile = new VFDO
            {
            };
            virtFile.SetData(new[] { file });
            DoNativeDragDrop(virtFile);
        }

        static void DoNativeDragDrop(object data)
        {
            var meth = typeof(VFDO).GetMethod("DoDragDrop");
            var effects = Enum.GetValues(meth.GetParameters()[2].ParameterType);
            var effect = effects.OfType<object>().Last();
            meth.Invoke(null, new[] { null, data, effect });
        }
        #endregion

        #region Input mode
        void ListView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            // if (e.Data.GetDataPresent(DataFormatFileContent))
            //     e.Effect = DragDropEffects.Copy;
        }

        void ListView_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var path in filePaths)
                    AddFile(path);
                return;
            }
            if (e.Data.GetDataPresent(DataFormatFileContent))
            {
                var desc = e.Data.GetData(DataFormatFileGroup, true);
                var cont = e.Data.GetData(DataFormatFileContent, true);
                // TODO: Handle that?!
                return;
            }
        }

        public void AddFile(string path)
        {
            var info = new FileInfo(path);
            var file = new FileDescriptor
            {
                ChangeTimeUtc = info.LastWriteTimeUtc,
                Length = info.Length,
                Name = info.Name,
                StreamContents = s =>
                {
                    using (var read = File.OpenRead(path))
                        read.CopyTo(s);
                }
            };
            listView.Items.Add(new ListViewItem
            {
                Name = info.FullName,
                ImageIndex = 0,
                Text = file.Name,
                Tag = file
            });
        }
        #endregion

        public IEnumerable<FileDescriptor> Files => listView.Items
            .OfType<ListViewItem>().Select(i => i.Tag).OfType<FileDescriptor>();
    }

    [Flags]
    public enum DragDropMode
    {
        None = 0, Input = 16, Output = 32
    }
}