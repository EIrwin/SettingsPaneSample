using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SettingsPaneSample
{
    public class MindmapStyleSelector : StyleSelector
    {
        public Style RootStyle { get; set; }
        public Style FirstLevelStyle { get; set; }
        public Style OuterStyle { get; set; }

        public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container)
        {
            if (container != null)
            {
                if (container is MindmapFirstLevelShape)
                    return this.FirstLevelStyle;

                if (container is MindmapOuterShape)
                    return this.OuterStyle;
            }

            return this.RootStyle;
        }
    }
}
