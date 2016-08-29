using FriendsWithPaws.Data;
using Windows.ApplicationModel.DataTransfer;
using System.Text;
using Windows.Storage.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace FriendsWithPaws
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class GroupDetailPage : FriendsWithPaws.Common.LayoutAwarePage
    {
        public GroupDetailPage()
        {
            this.InitializeComponent();
        }
        void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var request = args.Request;
            var item = (BreedDataItem)this.itemGridView.SelectedItem;
            request.Data.Properties.Title = item.Group.Title;
            request.Data.Properties.Description = "Breed group photo  and description";

            var breed =  "\r\n" +item.Group.Title + "\r";        
            breed += ("\r\n\r\nDescription\r\n" + item.Group.Description);
            request.Data.SetText(breed);
            var reference = RandomAccessStreamReference.CreateFromUri(new Uri(item.Group.ImagePath.AbsoluteUri));
            request.Data.Properties.Thumbnail = reference;
            request.Data.SetBitmap(reference);

        }
        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var group = BreedDataSource.GetGroup((String)navigationParameter);
            this.DefaultViewModel["Group"] = group;
            this.DefaultViewModel["Items"] = group.Items;
            DataTransferManager.GetForCurrentView().DataRequested += OnDataRequested;
        }
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            var selectedItem = (BreedDataItem)this.itemGridView.SelectedItem;
            pageState["SelectedItem"] = selectedItem.UniqueId;
            DataTransferManager.GetForCurrentView().DataRequested -= OnDataRequested;
        }
        /// <summary>
        /// Invoked when an item is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((BreedDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }
    }
}
