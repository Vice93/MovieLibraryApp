using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MovieLibraryApp.ViewModels;

namespace MovieLibraryApp.Views
{
    /// <summary>
    /// The explore search page.
    /// </summary>
    public sealed partial class ExploreSearch : Page
    {
        /// <summary>
        /// The explore search viewmodel.
        /// </summary>
        private readonly ExploreSearchViewModel _esv = new ExploreSearchViewModel();
        /// <summary>
        /// Initializes a new instance of the <see cref="ExploreSearch"/> class.
        /// </summary>
        public ExploreSearch()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            PopulateYearCombo(50);
        }

        /// <summary>
        /// Performs a search based on the selected parameters.
        /// </summary>
        private async void Search()
        {
            if (GenreCombo.SelectionBoxItem == null || YearCombo.SelectionBoxItem == null) return;

            try
            {
                LoadingIndicator.IsActive = true;

                var checkedButton = TypeRadioGroup.Children.OfType<RadioButton>()
                .FirstOrDefault(r => (bool)r.IsChecked);
                var res = await _esv.Explore((string)GenreCombo.SelectionBoxItem, YearCombo.SelectionBoxItem.ToString(),
                          (string)checkedButton.Content);

                MainGrid.ItemsSource = res;
                EmptyList.Text = MainGrid.Items.Any() ? "" : "Nothing was found. Try some other parameters!";
            }
            finally
            {
                LoadingIndicator.IsActive = false;
            }
        }

        /// <summary>
        /// Handles the OnClick event of the SearchButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            Search();
        }

        /// <summary>
        /// Populates the year combo with the last X years.
        /// </summary>
        /// <param name="yearsToAdd">The number of years to add to the combo</param>
        private void PopulateYearCombo(int yearsToAdd)
        {
            for(var i=0; i<yearsToAdd; i++)
            {
                var item = new ComboBoxItem
                {
                    Content = DateTime.Now.Year - i
                };
                YearCombo.Items.Add(item);
            }
        }
    }
}
