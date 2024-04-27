using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler.WinForm
{
    public class ResourceManager
    {
        static ResourceManager _singletonInstance;
        public static ResourceManager GlobalInstance
        {
            get
            {
                if (_singletonInstance == null)
                {
                    _singletonInstance = new ResourceManager();
                }

                return _singletonInstance;
            }
        }

        public ResourceManager()
        {

        }

        public ImageList Icons 
        { 
            get
            {

            } 
        }
    }
}
