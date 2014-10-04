using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Controls.Diagrams.Extensions;
using Telerik.Windows.Diagrams.Core;

namespace SettingsPaneSample
{
    public partial class Example
    {
        private readonly FileManager fileManager;
        private MainViewModel maintViewModel;

        static Example()
        {
            var saveBinding = new CommandBinding(DiagramCommands.Save, ExecuteSave, CanExecutedSave);
            var openBinding = new CommandBinding(DiagramCommands.Open, ExecuteOpen);

            CommandManager.RegisterClassCommandBinding(typeof(Example), saveBinding);
            CommandManager.RegisterClassCommandBinding(typeof(Example), openBinding);
        }

        public Example()
        {
            InitializeComponent();

            this.fileManager = new FileManager(this.diagram);
            this.DataContext = this.maintViewModel = new MainViewModel();

            // the Bezier offset has to be increased a bit to give a more smooth appearance than the default
            DiagramConstants.BezierAutoOffset = 100d;
            this.Loaded += this.OnLoaded;
        }

        public MainViewModel ViewModel
        {
            get
            {
                if (this.maintViewModel == null)
                    this.maintViewModel = this.root.DataContext as MainViewModel;
                return this.maintViewModel;
            }
        }

        private static void CanExecutedSave(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            var owner = sender as Example;
            e.CanExecute = owner != null && owner.diagram != null && owner.diagram.Items.Count > 0;
        }

        private static void ExecuteSave(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            var owner = sender as Example;
            if (owner != null) owner.fileManager.SaveToFile();
        }

        private static void ExecuteOpen(object sender, ExecutedRoutedEventArgs e)
        {
            var owner = (Example)sender;
            owner.fileManager.LoadFromFile();
        }

        private void OnDiagramMetadataDeserialized(object sender, DiagramSerializationRoutedEventArgs e)
        {
            this.ViewModel.ClearData();
            this.ViewModel.Source.ShouldAddConnection = false;
        }

        private void OnDiagramDeserialized(object sender, RadRoutedEventArgs e)
        {
            this.ViewModel.Source.ShouldAddConnection = true;
        }

        private void OnDiagramConnectionManipulationStarted(object sender, ManipulationRoutedEventArgs e)
        {
            if (e.ManipulationStatus == ManipulationStatus.Detaching)
            {
                e.Handled = true;
            }
        }

        private void LayoutButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                TreeLayoutSettings settings = new TreeLayoutSettings()
                {
                    TreeLayoutType = TreeLayoutType.TreeLeft,
                };
                switch (button.Tag.ToString())
                {
                    case "0":
                        settings.TreeLayoutType = TreeLayoutType.MindmapHorizontal;
                        break;
                    case "1":
                        settings.TreeLayoutType = TreeLayoutType.MindmapVertical;
                        settings.HorizontalSeparation = 40;
                        break;
                    case "2":
                        settings.TreeLayoutType = TreeLayoutType.TreeLeft;
                        settings.VerticalSeparation = 30;
                        break;
                    case "3":
                        settings.TreeLayoutType = TreeLayoutType.TreeRight;
                        settings.VerticalSeparation = 30;
                        break;
                    case "4":
                        settings.TreeLayoutType = TreeLayoutType.TreeUp;
                        settings.VerticalSeparation = 30;
                        break;
                    case "5":
                        settings.TreeLayoutType = TreeLayoutType.TreeDown;
                        settings.VerticalSeparation = 30;
                        break;
                    default:
                        return;
                }
                settings.Roots.Add(this.diagram.ContainerGenerator.ContainerFromItem(this.maintViewModel.Source.Items.OfType<object>().ElementAt(0)) as IShape);
                this.diagram.Layout(LayoutType.Tree, settings);
                this.diagram.AutoFit();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //using (var stream = ExtensionUtilities.GetStream(string.Format("/MindMap/Resources/Career_planning.xml")))
            //{
            //    using (var reader = new StreamReader(stream))
            //    {
            //        var xml = reader.ReadToEnd();
            //        if (!string.IsNullOrEmpty(xml))
            //        {
            //            diagram.Load(xml);
            //        }
            //    }
            //}

            //CommandManager.InvalidateRequerySuggested();
        }
    }
}
