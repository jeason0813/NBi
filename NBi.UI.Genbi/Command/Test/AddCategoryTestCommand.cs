﻿using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Command.Test
{
	class AddCategoryTestCommand : CommandBase
	{
		private readonly TestListPresenter presenter;
		private readonly NewCategoryWindow window;

		public AddCategoryTestCommand(TestListPresenter presenter, NewCategoryWindow newCategoryWindow)
		{
			this.presenter = presenter;
			window = newCategoryWindow;
		}

		/// <summary>
		/// Refreshes the command state.
		/// </summary>
		public override void Refresh()
		{
			this.IsEnabled = presenter.SelectedTest != null;
		}

		/// <summary>
		/// Executes the command logics.
		/// </summary>
		public override void Invoke()
		{
			DialogResult result = window.ShowDialog();
			if (result == DialogResult.OK)
			{
				presenter.AddCategory(window.CategoryName);
			}
		}
	}
}
