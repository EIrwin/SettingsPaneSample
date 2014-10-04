using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;

namespace SettingsPaneSample
{
    public enum MindmapItemType
    {
        None,
        Root,
        FirstLevelItem,
        SubItem
    }

    public class BaseItem : ViewModelBase
    {
        private string text;
        private bool isVisible;
        private string mainColor;

        public BaseItem()
        {
            this.isVisible = true;
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.OnPropertyChanged("Text");
                }
            }
        }
        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                if (this.isVisible != value)
                {
                    this.isVisible = value;
                    this.OnPropertyChanged("IsVisible");
                }
            }
        }
        public string MainColor
        {
            get
            {
                return this.mainColor;
            }
            set
            {
                if (this.mainColor != value)
                {
                    this.mainColor = value;
                    this.OnPropertyChanged("MainColor");
                }
            }
        }
    }
}
