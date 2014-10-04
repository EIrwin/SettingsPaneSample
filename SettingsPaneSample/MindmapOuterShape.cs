using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Telerik.Windows.Controls.Diagrams;

namespace SettingsPaneSample
{
    public class MindmapOuterShape : MindmapShapeBase
    {
        public MindmapOuterShape()
        {
            this.DefaultStyleKey = typeof(MindmapOuterShape);

            this.Connectors.Add(new RadDiagramConnector() { Offset = new Point(0, 1), Name = "subItemItemLeft" });
            this.Connectors.Add(new RadDiagramConnector() { Offset = new Point(1, 1), Name = "subItemItemRight" });
        }
    }
}
