using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using TaskSched.Common.FieldValidator;
using TaskScheduler.WinForm.Configuration;
using TaskScheduler.WinForm.Controls.PropertyGridHelper;

namespace TaskScheduler.WinForm.Models
{
    public class AboutModel : ITreeItem
    {
        [ReadOnly(true)]
        [Category("Versioning")]
        public string Version { get; private set; } = "Unknown";

        [ReadOnly(true)]
        [Category("Versioning")]
        public string Configuration { get; private set; } = "Unknown";

        [ReadOnly(true)]
        [Category("Licensing")]
        public string Copyright { get; private set; } = "Copyright 2024 Whispersteppe Consulting.";

        [Category("Program")]
        public string Title { get; private set; } = "Task Scheduler";
        [TypeConverter(typeof(DictionaryConverter<string, string>))]
        [Category("Program")]
        public InternalDictionary<string, string> Authors { get; private set; } = new InternalDictionary<string, string>("Authors") 
        { 
            { "Chris Arterburn", "Chief cook and bottle washer" } 
        };

        [Category("Versioning")]
        public string BuildTime { get; private set; }
        [Category("Program")]

        public string Company { get; private set; } = "Whispersteppe Consulting";
        [Category("Program")]
        public string Product { get; private set; } = "Task Scheduler";
        [Category("Program")]
        public string Description { get; private set; } = "Open documents, urls, and other tasks on a schedule";
        [Category("Release")]
        public string ProjectUrl { get; private set; } = "https://github.com/Whispersteppe/TaskSched";
        [Category("Release")]
        public string Readme { get; private set; } = "https://github.com/Whispersteppe/TaskSched/blob/main/README.md";
        [Category("Release")]
        public string RepositoryUrl { get; private set; } = "https://github.com/Whispersteppe/TaskSched";
        [Category("Release")]
        public string ReleaseNotes { get; private set; } = "No current notes";
        [Category("Licensing")]
        [TypeConverter(typeof(DictionaryConverter<string, string>))]
        public InternalDictionary<string, string> Licenses { get; private set; } = new InternalDictionary<string, string>("Licenses")
        {
            { "Task Scheduler", "MIT License." },
        };

        public AboutModel()
        {
            Assembly currentAssem = Assembly.GetExecutingAssembly();
            AssemblyName name = currentAssem.GetName();
            Version = name.Version.ToString();

            var attributes = currentAssem.GetCustomAttributes();
            foreach(var attribute in attributes)
            {
                if (attribute is AssemblyCompanyAttribute companyAttribute)
                {
                    Company = companyAttribute.Company;
                }
                else if (attribute is AssemblyCopyrightAttribute copyrightAttribute)
                {
                    Copyright = copyrightAttribute.Copyright;
                }
                else if (attribute is AssemblyDescriptionAttribute descriptionAttribute)
                {
                    Description = descriptionAttribute.Description;
                }
                else if (attribute is AssemblyProductAttribute productAttribute)
                {
                    Product = productAttribute.Product;
                }
                else if (attribute is AssemblyTitleAttribute titleAttribute)
                {
                    Title = titleAttribute.Title;
                }
                else if (attribute is BuildDateTimeAttribute buildDateTimeAttribute)
                {
                    BuildTime = buildDateTimeAttribute.Built.ToString();
                }
                else if (attribute is AssemblyConfigurationAttribute configurationAttribute)
                {
                    Configuration = configurationAttribute.Configuration;
                }
            }
        }


        [Browsable(false)]
        public string DisplayName => "About";

        [Browsable(false)]
        public Guid? ID => Guid.Empty;

        [Browsable(false)]
        public Guid? ParentId => Guid.Empty;

        [Browsable(false)]
        public TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.AboutItem;

        [Browsable(false)]
        public List<TreeItemTypeEnum> AllowedMoveToParentTypes => [];

        [Browsable(false)]
        public List<TreeItemTypeEnum> AllowedChildTypes => [];

        [Browsable(false)]
        public List<ITreeItem> Children => [];

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool CanAddCreateChild(TreeItemTypeEnum itemType)
        {
            return false;
        }

        public bool CanAddItem(ITreeItem possibleNewChild)
        {
            return false;
        }

        public bool CanDeleteItem()
        {
            return false;
        }

        public bool CanEdit()
        {
            return false;
        }

        public bool CanHaveChildren()
        {
            return false;
        }

        public bool CanMoveItem(ITreeItem possibleNewParent)
        {
            return false;
        }

        public bool ContainsText(string text)
        {
            return true;
        }
    }


}
