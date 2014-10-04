using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Diagrams.Core;

namespace SettingsPaneSample
{
    public class MindmapShapeBase : RadDiagramShapeBase
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty AreChildrenVisibleProperty =
            DependencyProperty.Register("AreChildrenVisible", typeof(bool), typeof(MindmapShapeBase), new PropertyMetadata(true, OnAreChildrenVisiblePropertyChanged));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsSettingsOpenProperty =
            DependencyProperty.Register("IsSettingsOpen", typeof(bool), typeof(MindmapShapeBase), new PropertyMetadata(false, OnIsSettingsOpenPropertyChanged));

        private const string MouseOverPartName = "PART_mouseOverBorder";
        private FrameworkElement itemBorder;
        private bool isUndoRedo;
        private bool isMouseOverShape;

        internal MindmapDiagram ParentDiagram
        {
            get
            {
                return this.Diagram as MindmapDiagram;
            }
        }

        internal bool IsMouseOverShape
        {
            get
            {
                return this.isMouseOverShape;
            }
            set
            {
                if (this.isMouseOverShape != value)
                {
                    this.isMouseOverShape = value;
#if SILVERLIGHT
					this.IsMouseOver = value;
#endif
                    this.UpdateVisualStates();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MindmapShapeBase" /> class.
        /// </summary>
        public MindmapShapeBase()
        {
        }

        /// <summary>
        /// Gets or sets the is settings open.
        /// </summary>
        public bool IsSettingsOpen
        {
            get { return (bool)GetValue(IsSettingsOpenProperty); }
            set { SetValue(IsSettingsOpenProperty, value); }
        }

        /// <summary>
        /// Gets or sets AreChildrenVisible property.
        /// </summary>
        public bool AreChildrenVisible
        {
            get { return (bool)GetValue(AreChildrenVisibleProperty); }
            set { SetValue(AreChildrenVisibleProperty, value); }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate" />. In simplest terms, this means the method is called just before a UI element displays in an application. For more information, see Remarks.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.itemBorder != null)
                this.itemBorder.MouseLeftButtonDown -= this.OnItemBorderMouseLeftButtonDown;

            this.itemBorder = this.GetTemplateChild(MouseOverPartName) as FrameworkElement;

            if (this.itemBorder != null)
                this.itemBorder.MouseLeftButtonDown += this.OnItemBorderMouseLeftButtonDown;
        }

        public override void Deserialize(SerializationInfo info)
        {
            base.Deserialize(info);
            this.ClearValue(MindmapShapeBase.PositionProperty);
            this.ClearValue(MindmapShapeBase.IsSelectedProperty);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseEnter" /> attached event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            this.IsMouseOverShape = true;
            this.UpdateVisualStates();
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseLeave" /> attached event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.IsMouseOverShape = false;
            this.UpdateVisualStates();
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.UIElement.MouseLeftButtonUp" /> routed
        /// event reaches an element in its route that is derived from this class. Implement
        /// this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" />
        /// that contains the event data. The event data reports that the left mouse button
        /// was released.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            this.ParentDiagram.DraggingCandidate = null;
        }

        /// <summary>
        /// Called when the IsSelected property has changed.
        /// </summary>
        /// <param name="oldValue">The old value of the IsSelected property.</param>
        /// <param name="newValue">The new value of the IsSelected property.</param>
        protected override void OnIsSelectedChanged(bool oldValue, bool newValue)
        {
            base.OnIsSelectedChanged(oldValue, newValue);
            this.UpdateVisualStates();
        }

        private static void OnAreChildrenVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var shape = d as MindmapShapeBase;
            if (shape != null)
            {
                shape.ToggleChildrenVisibility((bool)e.NewValue);
            }
        }

        private static void OnIsSettingsOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var shape = d as MindmapShapeBase;
            if (shape != null)
            {
                shape.UpdateVisualStates();
            }
        }

        private void ToggleChildrenVisibility(bool areChildrenVisible)
        {
            Visibility nextVisibility = areChildrenVisible ? Visibility.Visible : Visibility.Collapsed;

            this.ToggleChildrenVisibilityRecursively(nextVisibility, this);

            if (!this.isUndoRedo)
                this.ParentDiagram.UndoRedoService.ExecuteCommand(new UndoableDelegateCommand("ToggleChildrenVisibility", s => this.ChangeAreChildrenVisible(areChildrenVisible), s => this.ChangeAreChildrenVisible(!areChildrenVisible)));
        }

        private void ToggleChildrenVisibilityRecursively(Visibility nextVisibility, MindmapShapeBase shape)
        {
            if (shape == null || nextVisibility == System.Windows.Visibility.Visible && shape.AreChildrenVisible == false)
                return;

            foreach (var connection in this.ParentDiagram.GetOutgoingConnectionsForShape(shape))
            {
                connection.Visibility = nextVisibility;
                if (!shape.Equals(connection.Target) && connection.Target != null)
                {
                    connection.Target.Visibility = nextVisibility;
                    this.ToggleChildrenVisibilityRecursively(nextVisibility, connection.Target as MindmapShapeBase);
                }
            }
        }

        private void ChangeAreChildrenVisible(bool areChildrenVisible)
        {
            this.isUndoRedo = true;
            this.AreChildrenVisible = areChildrenVisible;
            this.isUndoRedo = false;
        }

        private void OnItemBorderMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.ParentDiagram.DraggingCandidate = this;
            e.Handled = true;
        }

        private void UpdateVisualStates()
        {
            if (this.IsMouseOverShape || this.IsSelected || this.IsSettingsOpen)
                VisualStateManager.GoToState(this, "WithBorder", false);
            else
                VisualStateManager.GoToState(this, "Borderless", false); 
        }
    }
}
