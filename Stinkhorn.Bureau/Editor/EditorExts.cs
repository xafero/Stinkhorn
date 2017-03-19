using System.IO;
using System.Reflection;
using AdamsLair.WinForms.PropertyEditing;
using AdamsLair.WinForms.PropertyEditing.Templates;
using AdamsLair.WinForms.PropertyEditing.Editors;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using DialogResult = System.Windows.Forms.DialogResult;

namespace Stinkhorn.Bureau.Editor
{
    public static class EditorExts
    {
        const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

        public static void EnableAddons(this PropertyGrid grid)
        {
            grid.MouseDoubleClick += OnPropGridDoubleClick;
        }

        static void OnPropGridDoubleClick(object sender, MouseEventArgs e)
        {
            var grid = (PropertyGrid)sender;
            var editor = grid.FocusEditor;
            var strPropEd = editor as StringPropertyEditor;
            if (strPropEd == null)
                return;
            var ed = strPropEd.GetType().GetField("stringEditor", flags);
            var sedt = ed.GetValue(strPropEd) as StringEditorTemplate;
            if (sedt == null)
                return;
            var txtf = sedt.GetType().GetField("text", flags);
            if (txtf == null)
                return;
            var text = string.Empty;
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Script files|*.cs" + "|"
                    + "Text files|*.txt;*.md" + "|"
                    + "All files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                    text = File.ReadAllText(dialog.FileName);
            }
            if (string.IsNullOrWhiteSpace(text))
                return;
            txtf.SetValue(sedt, null);
            sedt.InsertText(text);
        }
    }
}