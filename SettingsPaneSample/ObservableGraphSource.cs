using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Telerik.Windows.Diagrams.Core;

namespace SettingsPaneSample
{
    /// <summary>
    /// Represents an Observable GraphSource.
    /// </summary>
    public partial class ObservableGraphSource : IObservableGraphSource
    {
        private static List<string> Colors;

        private int colorIndex;
        private readonly ObservableCollection<Node> items = new ObservableCollection<Node>();
        private readonly ObservableCollection<Link> mindmapLinks = new ObservableCollection<Link>();

        static ObservableGraphSource()
        {
            Colors = new List<string>();
            Colors.Add("FF80ad30");
            Colors.Add("FF309b46");
            Colors.Add("FF15aca9");
            Colors.Add("FF25a0da");
            Colors.Add("FF456be2");
            Colors.Add("FF7e51a1");
            Colors.Add("FFdf73ad");
            Colors.Add("FFda0780");
            Colors.Add("FFe61e26");
            Colors.Add("FFf47836");
            Colors.Add("FFf1c700");
            Colors.Add("FF767676");
        }

        public ObservableGraphSource()
        {
        }

        public IEnumerable Items
        {
            get { return this.items; }
        }

        public IEnumerable<ILink> Links
        {
            get { return this.mindmapLinks; }
        }

        public bool ShouldAddConnection { get; set; }

        public void AddLink(ILink link)
        {
            var myLink = link as Link;
            if (myLink != null)
            {
                if (myLink.Source != null)
                    myLink.Source.ChildrenCount++;
                this.mindmapLinks.Add(myLink);
            }
        }

        public bool RemoveLink(ILink link)
        {
            var myLink = link as Link;
            bool result = false;
            if (myLink != null)
            {
                if (myLink.Source != null)
                    myLink.Source.ChildrenCount--;
                result = this.mindmapLinks.Remove(myLink);
            }
            return result;
        }

        public ILink CreateLink(object source, object target)
        {
            var mindmapLink = new Link();
            var sourceNode = source as Node;
            var targetNode = target as Node;
            if (sourceNode != null && targetNode != null)
            {
                mindmapLink.Source = sourceNode;
                mindmapLink.Target = targetNode;
                targetNode.ShapeType = sourceNode.ShapeType == MindmapItemType.Root ? MindmapItemType.FirstLevelItem : MindmapItemType.SubItem;

                if (sourceNode.ShapeType == MindmapItemType.Root)
                {
                    targetNode.MainColor = Colors[colorIndex];
                    mindmapLink.MainColor = Colors[colorIndex];
                    colorIndex++;
                    colorIndex = colorIndex % Colors.Count;
                }
                else
                {
                    targetNode.MainColor = sourceNode.MainColor;
                    mindmapLink.MainColor = sourceNode.MainColor;
                }
            }
            return mindmapLink;
        }

        public void AddNode(object node)
        {
            var newItem = node as Node;
            this.items.Add(newItem);
        }

        public bool RemoveNode(object node)
        {
            var item = node as Node;
            bool result = false;
            if (items != null && item.ShapeType != MindmapItemType.Root)
            {
                result = this.items.Remove(item);
            }
            return result;
        }

        public object CreateNode(IShape shape)
        {
            this.ShouldAddConnection = true;
            return new Node() { Text = "New item" };
        }

        internal void Clear()
        {
            this.items.Clear();
            this.mindmapLinks.Clear();
            this.ClearCache();
        }

        internal void AddMainIdea(Point? position = null)
        {
            if (!position.HasValue)
                position = new Point(512, 225);
            this.items.Add(new Node() { Text = "Main Idea", Position = position.Value, ShapeType = MindmapItemType.Root });
        }
    }
}
