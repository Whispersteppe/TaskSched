﻿namespace OldDataLoader.OldDataModel
{
    public class RSSWatcherTemplateConfig : TaskTemplateBaseConfig
    {

        public override TaskTemplateType TemplateType => TaskTemplateType.RSSWatcher;
        public string ExecutablePath { get; set; }
        public string CommandLine { get; set; }
    }
}
