using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Diagrams.Core;

namespace SettingsPaneSample
{
    public class Link : BaseItem, ILink<Node>
    {
        private Node source;
        private Node target;

        public Node Source
        {
            get
            {
                return this.source;
            }
            set
            {
                if (this.source != value)
                {
                    this.source = value;
                    this.OnPropertyChanged("Source");
                }
            }
        }

        public Node Target
        {
            get
            {
                return this.target;
            }
            set
            {
                if (this.target != value)
                {
                    this.target = value;
                    this.OnPropertyChanged("Target");
                }
            }
        }

        object ILink.Source
        {
            get
            {
                return this.Source;
            }
            set
            {
                this.Source = value as Node;
            }
        }

        object ILink.Target
        {
            get
            {
                return this.Target;
            }
            set
            {
                this.Target = value as Node;
            }
        }
    }
}
