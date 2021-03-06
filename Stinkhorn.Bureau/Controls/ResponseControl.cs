﻿using System.Windows.Forms;
using System.Linq;
using Stinkhorn.Bureau.Utils;
using System;

namespace Stinkhorn.Bureau.Controls
{
    public partial class ResponseControl : UserControl
    {
        object rsp;

        public ResponseControl() : this(null)
        {
        }

        public ResponseControl(object rsp, string title = null)
        {
            InitializeComponent();
            this.rsp = rsp;
            groupBox1.Text = $"({DateTime.Now.ToLongTimeString()}) {title}";
            propertyGrid1.SetSelectedObject(rsp, readOnly: true);
            ExtractDragDrop(rsp);
        }

        void ExtractDragDrop(object obj)
        {
            var paths = Reflections.GetTypes(obj);
            var binary = paths.FirstOrDefault(e => e.Value == typeof(byte[]));
            if (string.IsNullOrWhiteSpace(binary.Key))
            {
                splitContainer1.Panel2Collapsed = true;
                dragDropControl1.Mode = DragDropMode.None;
                dragDropControl1.Visible = false;
                dragDropControl1.Title = string.Empty;
            }
            else
            {
                splitContainer1.Panel2Collapsed = false;
                dragDropControl1.Mode = DragDropMode.Output;
                dragDropControl1.Visible = true;
                dragDropControl1.Title = string.Empty;
                var chunks = Reflections.Extract<byte[]>(obj, binary.Key);
                var i = 0;
                foreach (var bytes in chunks)
                {
                    var ext = MimeType.GetFileType(bytes);
                    var key = binary.Key.Replace("/Item/", $"/{++i}/");
                    var count = key.LastIndexOf('/') - 1;
                    if (count < 1)
                        count = key.Length - 1;
                    key = key.Substring(1, count);
                    var name = $"{key.Replace('/', '_')}.{ext}";
                    dragDropControl1.AddFile(name, bytes);
                }
            }
        }
    }
}