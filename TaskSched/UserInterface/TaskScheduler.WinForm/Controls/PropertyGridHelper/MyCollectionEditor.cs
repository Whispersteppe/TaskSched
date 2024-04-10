using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler.WinForm.Controls.PropertyGridHelper
{

    //TODO - may need a separate one of these for each item type, or filter it through the sender
    //[Editor(typeof(MyCollectionEditor), typeof(UITypeEditor))]
    //  will need to hook this static event in the control someplace.
    //  hence the need to 

    public class MyCollectionEditor : CollectionEditor

    {
        // Define a static event to expose the inner PropertyGrid's
        // PropertyValueChanged event args...
        public delegate void MyPropertyValueChangedEventHandler(object sender,PropertyValueChangedEventArgs e);
        public static event MyPropertyValueChangedEventHandler MyPropertyValueChanged;

        // Inherit the default constructor from the standard
        // Collection Editor...
        public MyCollectionEditor(Type type) : base(type) { }

        // Override this method in order to access the containing user controls
        // from the default Collection Editor form or to add new ones...
        protected override CollectionForm CreateCollectionForm()
        {
            // Getting the default layout of the Collection Editor...
            CollectionForm collectionForm = base.CreateCollectionForm();

            Form frmCollectionEditorForm = collectionForm as Form;
            TableLayoutPanel tlpLayout = frmCollectionEditorForm.Controls[0] as TableLayoutPanel;

            if (tlpLayout != null)
            {
                // Get a reference to the inner PropertyGrid and hook
                // an event handler to it.
                if (tlpLayout.Controls[5] is PropertyGrid propertyGrid)
                {
                    //PropertyGrid propertyGrid = tlpLayout.Controls[5] as PropertyGrid;
                    propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(propertyGrid_PropertyValueChanged);
                }
            }
            return collectionForm;
        }

        void propertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            MyPropertyValueChanged?.Invoke(this, e);
            //// Fire our customized collection event...
            //if (MyCollectionEditor.MyPropertyValueChanged != null)
            //{
            //    MyCollectionEditor.MyPropertyValueChanged(this, e);
            //}
        }
    }
}
