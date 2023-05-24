using ModelsApi;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheFool.Tools;
using TheFool.Views;

namespace TheFool.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {

        #region Search
        private string searchText = "";
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                Search();
            }
        }
        private void Search()
        {
            var search = SearchText.ToLower();
            searchResult = mysearch.Where(s => s.HouseType.Contains(search)).ToList();
            Announcements = searchResult;
        }

        public List<AnnouncementApi> searchResult;

        public List<AnnouncementApi> mysearch { get; set; }
        #endregion

        #region Commands
        public CustomCommand AddAnnouncement { get; set; }
        public CustomCommand EditAnnouncement { get; set; }
        #endregion

        #region Lego
        public ProfilePageViewModel()
        {
            Task.Run(TakeListAnnouncements).ContinueWith(s =>
            {
                //InitPagination();
                //Pagination();
            });

            #region добавляй удаляй пиратствуй
            AddAnnouncement = new CustomCommand(() =>
            {
                EditProfilePageWindow editAnnouncement = new EditProfilePageWindow();
                editAnnouncement.ShowDialog();
                Thread.Sleep(200);
                Task.Run(TakeListAnnouncements);
            });

            EditAnnouncement = new CustomCommand(() =>
            {
                if (SelectedAnnouncement == null) return;
                EditProfilePageWindow editAnnouncement = new EditProfilePageWindow(SelectedAnnouncement);
                editAnnouncement.ShowDialog();
                Thread.Sleep(200);
                Task.Run(TakeListAnnouncements);
            });
            #endregion
        }
        #endregion

        #region свойства
        public AnnouncementApi selectedAnnouncement;
        public AnnouncementApi SelectedAnnouncement
        {
            get => selectedAnnouncement;
            set
            {
                selectedAnnouncement = value;
                SignalChanged();
            }
        }

        public List<AnnouncementApi> announcements { get; set; }
        public List<AnnouncementApi> Announcements
        {
            get => announcements;
            set
            {
                announcements = value;
                SignalChanged();
            }
        }
        #endregion

        #region task
        public async Task TakeListAnnouncements()
        {
            var result = await Api.GetListAsync<AnnouncementApi[]>("Announcement");
            Announcements = new List<AnnouncementApi>(result);
            SignalChanged("Announcements");
            searchResult = new List<AnnouncementApi>(result);
            mysearch = new List<AnnouncementApi>(result);
        }
        #endregion

        public class MainViewModel : BaseViewModel
        {
            private object _currentPage;
            public object CurrentPage
            {
                get { return _currentPage; }
                set
                {
                    _currentPage = value;
                    SignalChanged("CurrentPage");
                }
            }

            #region commands
            public CustomCommand AnnouncementPage { get; set; }
            public CustomCommand ProfilePage { get; set; }
            public CustomCommand HomePage { get; set; }

            #endregion

            public MainViewModel()
            {
                CurrentPage = new HomePage();

                HomePage = new CustomCommand(() =>
                {
                    CurrentPage = new HomePage();
                    SignalChanged("CurrentPage");
                });

            }
        }
    }
}
