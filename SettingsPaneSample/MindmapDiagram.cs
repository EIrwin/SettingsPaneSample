using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Diagrams.Core;
using Telerik.Windows.DragDrop;
using DragEventArgs = Telerik.Windows.DragDrop.DragEventArgs;

namespace SettingsPaneSample
{
    public class MindmapDiagram : RadDiagram
    {
        internal MindmapShapeBase DraggingCandidate { get; set; }

        public MindmapDiagram()
        {
            DragDropManager.AddDragInitializeHandler(this, this.OnDragInitialized);
            DragDropManager.AddGiveFeedbackHandler(this, OnGiveFeedback);
            DragDropManager.AddDropHandler(this, this.OnElementDragDropCompleted);
        }

        protected override Telerik.Windows.Diagrams.Core.IShape GetShapeContainerForItemOverride(object item)
        {
            return ShapeSelector.GetShape(item);
        }

        protected override bool IsItemItsOwnShapeContainerOverride(object item)
        {
            return item is MindmapShapeBase;
        }

        protected override void OnCanExecuteDeleteCommandOverride(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        protected override void OnDeleteCommandExecutedOverride(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter != null)
            {
                var container = this.ContainerGenerator.ContainerFromItem(e.Parameter) as IShape;
                if (container != null)
                {
                    this.DeleteShape(container);
                    return;
                }
            }

            this.DeleteSelectedShapes();
        }

        private void DeleteSelectedShapes()
        {
            foreach (var shape in this.SelectedItems.ToList().Select(item => this.ContainerGenerator.ContainerFromItem(item) as IShape).Where(shape => shape != null && !(shape is MindmapRootShape)))
                this.DeleteShape(shape);
        }

        private void DeleteShape(IShape shape)
        {
            var compositeRemoveShapeCommand = new CompositeCommand(CommandNames.RemoveShapes);
            var observableGraphSource = this.GraphSource as IObservableGraphSource;
            if (observableGraphSource != null)
            {
                foreach (var connection in this.GetConnectionsForShape(shape))
                {
                    compositeRemoveShapeCommand.AddCommand(this.CreateRemoveConnectionCommand(connection, observableGraphSource));
                    if (!shape.Equals(connection.Target))
                    {
                        compositeRemoveShapeCommand.AddCommand(this.CreateRemoveShapeCommand(connection.Target, observableGraphSource));
                    }
                }

                compositeRemoveShapeCommand.AddCommand(this.CreateRemoveShapeCommand(shape, observableGraphSource));

                this.UndoRedoService.ExecuteCommand(compositeRemoveShapeCommand);
            }
        }

        private UndoableDelegateCommand CreateRemoveShapeCommand(IShape shape, IObservableGraphSource observableGraphSource)
        {
            var node = this.ContainerGenerator.ItemFromContainer(shape);
            return new UndoableDelegateCommand(CommandNames.RemoveShape, s => observableGraphSource.RemoveNode(node), s => observableGraphSource.AddNode(node));
        }

        private UndoableDelegateCommand CreateRemoveConnectionCommand(IContainerChild connection, IObservableGraphSource observableGraphSource)
        {
            var link = this.ContainerGenerator.ItemFromContainer(connection) as ILink;
            var currentLink = link;
            return new UndoableDelegateCommand(CommandNames.RemoveConnection, s => observableGraphSource.RemoveLink(link), s => observableGraphSource.AddLink(currentLink));
        }

        private void OnElementDragDropCompleted(object sender, DragEventArgs e)
        {
            MindmapShapeBase newShape;
            if (this.DraggingCandidate is MindmapRootShape)
                newShape = new MindmapFirstLevelShape();
            else
                newShape = new MindmapOuterShape();

            this.AddShape(newShape, this.GetTransformedPoint(e.GetPosition(this)), true);
            this.AddConnection(new RadDiagramConnection
            {
                Source = this.DraggingCandidate,
                Target = newShape
            }, true);

            this.DraggingCandidate = null;
            e.Handled = true;
        }

        private void OnDragInitialized(object sender, DragInitializeEventArgs args)
        {
            if (this.DraggingCandidate != null)
            {
                this.DraggingCandidate.IsMouseOverShape = false;

                args.DragVisual = new Rectangle
                {
                    Width = 70,
                    Height = 32,
                    RadiusY = 2,
                    RadiusX = 2,
                    StrokeThickness = 2,
                    Fill = new SolidColorBrush(Color.FromArgb(0x19, 0x33, 0x33, 0x33)),
                    Stroke = new SolidColorBrush(Color.FromArgb(0x7F, 0x33, 0x33, 0x33))
                };
                args.DragVisualOffset = args.RelativeStartPoint;

                args.AllowedEffects = DragDropEffects.All;
                args.Handled = true;
            }
            else
            {
                args.Cancel = true;
            }
        }

        private static void OnGiveFeedback(object sender, Telerik.Windows.DragDrop.GiveFeedbackEventArgs e)
        {
            e.SetCursor(Cursors.Arrow);
            e.Handled = true;
        }
    }
}
