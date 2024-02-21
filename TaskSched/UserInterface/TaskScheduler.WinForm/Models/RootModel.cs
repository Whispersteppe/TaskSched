using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TaskScheduler.WinForm.Models
{

    public class RootModel : ITreeItem
    {
        string _name;
        List<ITreeItem> _children;
        public RootModel(string name) 
        { 
            _name = name;
            _children = new List<ITreeItem>();
        }

        public string Name
        {
            get 
            { 
                return _name; 
            }

        }

        public List<ITreeItem>? Children
        {
            get
            {
                return _children;
            }

        }

        public void Revert()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }

}
