using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Diagrams.Core;

namespace SettingsPaneSample
{
    public partial class ObservableGraphSource : ISerializableGraphSource
    {
        private IList<Node> deserializedEmployees = new List<Node>();

        public void SerializeLink(ILink link, SerializationInfo info)
        {
            var myLink = link as Link;
            if (myLink.Source != null)
                info["SourceEmployeeId"] = myLink.Source.GetId();
            if (myLink.Target != null)
                info["TargetEmployeeId"] = myLink.Target.GetId();
            info["IsVisible"] = myLink.IsVisible;
            info["Text"] = myLink.Text;
            info["MainColor"] = myLink.MainColor;
        }

        public void SerializeNode(object model, SerializationInfo info)
        {
            Node node = model as Node;

            info["Text"] = node.Text;
            info["ShapeType"] = node.ShapeType;
            info["Position"] = node.Position.ToInvariant();
            info["MainColor"] = node.MainColor;
            info["IsVisible"] = node.IsVisible;
            info["AreChildrenVisible"] = node.AreChildrenVisible;
            info["UniqueId"] = node.GetId();
            if (node.SelectedNumber != null)
                info["SelectedNumberIndex"] = node.Numbers.IndexOf(node.SelectedNumber);
            if (node.Icons.Count > 0)
            {
                string indexes = string.Empty;
                foreach (var icon in node.Icons.Where(i => i.IsSelected))
                {
                    indexes += node.Icons.IndexOf(icon) + ";";
                }
                info["SelectedIconsIndexes"] = indexes;
            }
        }

        public ILink DeserializeLink(IConnection connection, SerializationInfo info)
        {
            Link link = new Link();
            string sourceEmployeeId = string.Empty;
            string targeteEmployeeId = string.Empty;
            if (info["SourceEmployeeId"] != null)
                sourceEmployeeId = info["SourceEmployeeId"].ToString();
            if (info["TargetEmployeeId"] != null)
                targeteEmployeeId = info["TargetEmployeeId"].ToString();
            if (info["IsVisible"] != null)
                link.IsVisible = bool.Parse(info["IsVisible"].ToString());
            if (info["Text"] != null)
                link.Text = info["Text"].ToString();
            if (info["MainColor"] != null)
                link.MainColor = info["MainColor"].ToString();

            var sourceModel = this.deserializedEmployees.FirstOrDefault(x => x.GetId().Equals(sourceEmployeeId));
            if (sourceModel != null)
            {
                link.Source = sourceModel;
            }

            var targetModel = this.deserializedEmployees.FirstOrDefault(x => x.GetId().Equals(targeteEmployeeId));
            if (targetModel != null)
            {
                link.Target = targetModel;
            }

            return link;
        }

        public object DeserializeNode(IShape shape, SerializationInfo info)
        {
            Node newNode = new Node();
            if (info["ShapeType"] != null)
                newNode.ShapeType =
                    (MindmapItemType) Enum.Parse(typeof (MindmapItemType), info["ShapeType"].ToString(), true);
            if (info["Position"] != null)
                newNode.Position = Utils.ToPoint(info["Position"].ToString()).Value;
            if (info["Text"] != null)
                newNode.Text = info["Text"].ToString();
            if (info["MainColor"] != null)
                newNode.MainColor = info["MainColor"].ToString();
            if (info["AreChildrenVisible"] != null)
                newNode.AreChildrenVisible = bool.Parse(info["AreChildrenVisible"].ToString());
            if (info["IsVisible"] != null)
                newNode.IsVisible = bool.Parse(info["IsVisible"].ToString());

            if (info["SelectedNumberIndex"] != null)
            {
                int selectedIndex = int.Parse(info["SelectedNumberIndex"].ToString());
                if (newNode.Numbers.Count > selectedIndex)
                {
                    newNode.SelectedNumber = newNode.Numbers[selectedIndex];
                }
            }

            if (info["SelectedIconsIndexes"] != null)
            {
                string[] indexes = info["SelectedIconsIndexes"].ToString()
                                                               .Split(";".ToCharArray(),
                                                                      StringSplitOptions.RemoveEmptyEntries);
                foreach (var stringIndex in indexes)
                {
                    int index = int.Parse(stringIndex);
                    if (newNode.Icons.Count > index)
                    {
                        newNode.Icons[index].IsSelected = true;
                    }
                }
            }

            this.deserializedEmployees.Add(newNode);

            return newNode;
        }

        public void ClearCache()
        {
            this.deserializedEmployees.Clear();
        }
    }
}
