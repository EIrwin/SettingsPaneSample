using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace SettingsPaneSample
{
    public class Node : BaseItem
    {
        private ObservableCollection<IconItem> icons;
        private ObservableCollection<IconItem> numbers;
        private MindmapItemType shapeType;
        private Point position;
        private int childrenCount;
        private bool areChildrenVisible = true;
        private IconItem selectedNumber;

        public Node()
        {
            this.icons = new IconsCollection();
            this.numbers = new NumbersCollection();

            if (this.Numbers.Count > 0)
                this.SelectedNumber = this.Numbers[0];
        }

        public ObservableCollection<IconItem> Numbers
        {
            get
            {
                return this.numbers;
            }
        }

        public ObservableCollection<IconItem> Icons
        {
            get
            {
                return this.icons;
            }
        }
        public MindmapItemType ShapeType
        {
            get
            {
                return this.shapeType;
            }
            set
            {

                if (this.shapeType != value)
                {
                    this.shapeType = value;
                    this.OnPropertyChanged("ShapeType");
                }
            }
        }
        public int ChildrenCount
        {
            get
            {
                return this.childrenCount;
            }
            set
            {
                if (this.childrenCount != value)
                {
                    this.childrenCount = value;
                    this.OnPropertyChanged("ChildrenCount");
                }
            }
        }
        public Point Position
        {
            get
            {
                return this.position;
            }
            set
            {
                if (this.position != value)
                {
                    this.position = value;
                    this.OnPropertyChanged("Position");
                }
            }
        }
        public bool AreChildrenVisible
        {
            get
            {
                return this.areChildrenVisible;
            }
            set
            {
                if (this.areChildrenVisible != value)
                {
                    this.areChildrenVisible = value;
                    this.OnPropertyChanged("AreChildrenVisible");
                }
            }
        }
        public IconItem SelectedNumber
        {
            get
            {
                return this.selectedNumber;
            }
            set
            {
                if (this.selectedNumber != value)
                {
                    this.selectedNumber = value;
                    this.OnPropertyChanged("SelectedNumber");
                }
            }
        }

        public string GetId()
        {
            return this.ShapeType.ToString() + this.Position.ToString() + this.Text.ToString() + this.AreChildrenVisible.ToString();
        }
    }
}
