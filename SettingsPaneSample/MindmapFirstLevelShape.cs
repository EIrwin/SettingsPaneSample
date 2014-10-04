using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Telerik.Windows.Controls.Diagrams;

namespace SettingsPaneSample
{
    public class MindmapFirstLevelShape : MindmapShapeBase
    {
        public MindmapFirstLevelShape()
        {
            this.DefaultStyleKey = typeof(MindmapFirstLevelShape);

            this.Connectors.Add(new RadDiagramConnector() { Offset = new Point(0.4, 0.5), Name = "firstLevelItemLeft" });
            this.Connectors.Add(new RadDiagramConnector() { Offset = new Point(0.6, 0.5), Name = "firstLevelItemRight" });
        }
    }
}
