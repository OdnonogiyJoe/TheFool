using ModelsApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TheFool.Tools;

namespace TheFool.ViewModels
{
    public class EditProfilePageWindowViewModel : BaseViewModel
    {

        #region properties Берем и назначаем
        private AnnouncementApi addAnnouncement;
        public AnnouncementApi AddAnnouncement
        {
            get => addAnnouncement;
            set
            {
                addAnnouncement = value;
                SignalChanged();
            }
        }

        public List<AnnouncementApi> announcements { get; set; }

        public List<DistrictApi> districts { get; set; }


        #region commands
        public CustomCommand Save { get; set; }

        public CityApi selectedCity;
        public CityApi SelectedCity
        {
            get => selectedCity;
            set
            {
                selectedCity = value;
                SignalChanged();
            }
        }
        #endregion


        public List<CityApi> cities { get; set; }

        #endregion

        #region Lego
        public EditProfilePageWindowViewModel(AnnouncementApi announcement)
        {
            Task.Run(TakeListAnnouncements).ContinueWith(s =>
            {
                if (announcement == null)
                {
                    AddAnnouncement = new AnnouncementApi();
                }
                else
                {
                    AddAnnouncement = new AnnouncementApi
                    {
                        Id = announcement.Id,
                        UserId = announcement.UserId,
                        Description = announcement.Description,
                        CityId = announcement.CityId,
                        DistrictId = announcement.DistrictId,
                        StreetNameId = announcement.StreetNameId,
                        TypeOfApartment = announcement.TypeOfApartment,
                        Area = announcement.Area,
                        Price = announcement.Price,
                        HouseType = announcement.HouseType,
                        State = announcement.State,
                        ApartmentFloor = announcement.ApartmentFloor,
                        Offer = announcement.Offer,
                        Repair = announcement.Repair,
                        Window = announcement.Window,
                        BalconyOrLoggia = announcement.BalconyOrLoggia,
                        Bathroom = announcement.Bathroom,
                        Elevator = announcement.Elevator,
                        Parking = announcement.Parking,
                        FloorsInTheApartment = announcement.FloorsInTheApartment,
                        TotalArea = announcement.TotalArea,
                        KitchenArea = announcement.KitchenArea,
                        HallwayArea = announcement.HallwayArea,
                        LivingArea = announcement.LivingArea,
                    };
                    if (announcement.CityId != null)
                    {
                        SelectedCity = cities.First(s => s.Id == announcement.CityId);
                    }
                }
            });

            Save = new CustomCommand(() =>
            {
                if (AddAnnouncement.Id == 0)
                    Task.Run(CreateNewAnnouncement);
                else
                    Task.Run(EditAnnouncement);


                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this) CloseWin(window);
                }

            });
        }

        #endregion


        #region task
        public async Task CreateNewAnnouncement()
        {
            if (AddAnnouncement.CityId != null)
            {
                AddAnnouncement.CityId = selectedCity.Id;
            }
            await Api.PostAsync<AnnouncementApi>(AddAnnouncement, "Announcementr");
        }

        public async Task EditAnnouncement()
        {
            if (SelectedCity != null)
            {
                AddAnnouncement.CityId = SelectedCity.Id;
            }
            await Api.PutAsync<AnnouncementApi>(AddAnnouncement, "Announcement");
        }

        public async Task TakeListAnnouncements()
        {
            var result = await Api.GetListAsync<AnnouncementApi[]>("Announcement");
            announcements = new List<AnnouncementApi>(result);
            SignalChanged("announcements");

            var result2 = await Api.GetListAsync<CityApi[]>("City");
            cities = new List<CityApi>(result2);
            SignalChanged("cities");
        }
#endregion

        public void CloseWin(object obj)
        {
            Window win = obj as Window;
            win.Close();
        }

    }
}
