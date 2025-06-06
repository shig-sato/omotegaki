using System;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace OmoOmotegaki
{
    public class DescriptiveCollectionEditor : CollectionEditor
    {
        public DescriptiveCollectionEditor(Type type) : base(type) { }

        protected override CollectionForm CreateCollectionForm()
        {
            CollectionForm form = base.CreateCollectionForm();
            form.Shown += delegate
            {
                ShowDescription(form);
            };
            return form;
        }

        private static void ShowDescription(Control control)
        {
            if (control is PropertyGrid grid)
            {
                grid.HelpVisible = true;
            }

            foreach (Control child in control.Controls)
            {
                ShowDescription(child);
            }
        }
    }
}
