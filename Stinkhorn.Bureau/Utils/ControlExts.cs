using AdamsLair.WinForms.PropertyEditing;
using System;

namespace Stinkhorn.Bureau.Utils
{
    public static class ControlExts
    {
        public static void SetSelectedObject(this PropertyGrid grid, object obj, bool readOnly = false)
        {
            grid.SelectObject(obj);
            grid.ReadOnly = readOnly;
            grid.ExpandAll();
        }

        static void ExpandAll(this PropertyGrid grid, bool state = true)
            => grid.MainEditor.ExpandAll(state);

        static void ExpandAll(this PropertyEditor editor, bool state = true)
        {
            editor.Expand(state);
            foreach (var child in editor.ChildEditors)
                child.ExpandAll(state);
        }

        static void Expand(this PropertyEditor editor, bool state = true)
        {
            var grouped = editor as GroupedPropertyEditor;
            if (grouped != null && grouped.PropertyName != "Bytes")
                grouped.Expanded = state;
        }
    }
}