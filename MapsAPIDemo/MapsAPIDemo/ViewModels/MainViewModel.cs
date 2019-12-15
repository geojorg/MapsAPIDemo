using MapsAPIDemo.Models;
using MapsAPIDemo.Services;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapsAPIDemo.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string _direction;
        private string _name;
        public SearchResultModel SearchResultModel { get; }
        public Map DemoMap { get; private set; }
        public double CurrentLatitude { get; set; }
        public double CurrentLongitude { get; set; }

        public MainViewModel()
        {
            SearchResultModel = new SearchResultModel();
            DemoMap = new Map();
            DemoMap.HasScrollEnabled = true;
            DemoMap.HasZoomEnabled = true;
            DemoMap.IsShowingUser = true;
            GetLocalPosition();
        }

        public string Direction
        {
            get { return _direction; }
            set { SetProperty(ref _direction, value); }
        }
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public async void GetLocalPosition()
        {
            try
            {
                var location = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    CurrentLatitude = location.Latitude;
                    CurrentLongitude = location.Longitude;
                    Position currentposition = new Position(CurrentLatitude, CurrentLongitude);
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(currentposition, Distance.FromKilometers(1));
                    DemoMap.MoveToRegion(mapSpan);
                }
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
        public ICommand SearchCommand => new Command(Search);
        private async void Search()
        {
            GetLocalPosition();
            var model = new SearchModel
            {
                Name = "------ THE PLACE YOU WANT TO SEARCH-------------",
                InputType = "textquery",
                Fields = "name,geometry,formatted_address",
                //IS SET TO 4000 meters and the current latituted and current longituted from the xamarin essential
                LocationBias = $"circle:4000@{CurrentLatitude},{CurrentLongitude}",
            };
            IMapsService mapsService = new MapsService();
            SearchResultModel resultModel = await mapsService.GetTextSearch(model);
            Direction = resultModel.Candidates.FirstOrDefault().FormattedAddress;
            Name = resultModel.Candidates.FirstOrDefault().Name;
            var Lat = resultModel.Candidates.FirstOrDefault().Geometry.Location.Lat;
            var Lng = resultModel.Candidates.FirstOrDefault().Geometry.Location.Lng;
            Pin pin = new Pin
            {
                Label = Name,
                Address = Direction,
                Type = PinType.SearchResult,
                Position = new Position(Lat, Lng)
            };
            DemoMap.Pins.Add(pin);
        }
    }
}



