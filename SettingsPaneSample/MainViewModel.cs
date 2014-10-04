using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Diagrams.Core;

namespace SettingsPaneSample
{
    public class MainViewModel : ViewModelBase
	{
		private readonly ObservableGraphSource source = new ObservableGraphSource();

		public MainViewModel()
		{
			this.ClearCommand = new DelegateCommand(OnClear);
		}

		public ObservableGraphSource Source
		{
			get
			{
				return this.source;
			}
		}
		public DelegateCommand ClearCommand { get; private set; }

		internal void ClearData()
		{
			this.Source.Clear();
		}

		private void OnClear(object obj)
		{
			this.ClearData();

			var diagram = obj as MindmapDiagram;
			if (diagram == null)
				return;

			diagram.UndoRedoService.Clear();
#if !WPF
			CommandManager.InvalidateRequerySuggested();
#endif
			var rect = diagram.Viewport as Rect?;
			if (rect.HasValue)
			{
				var center = rect.Value.Center();
				this.Source.AddMainIdea(new Point(center.X - 50, center.Y - 50));
			}
			else
			{
				this.Source.AddMainIdea();
			}
		}
	}
}
