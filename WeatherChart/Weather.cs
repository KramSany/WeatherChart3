using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Windows.Controls;
using HtmlAgilityPack;
namespace WeatherChart
{
    class Weather
    {
        public Dictionary<string, double[]> CollectionWindAndDays
        {
            get { return _collection; }
        }

        private static HtmlWeb? _web;
        private static HtmlDocument? _doc;
        public static HtmlNodeCollection? _windNode;
        public static Dictionary<string, double[]> _collection = new Dictionary<string, double[]>();
        public static int _index = 0;

        public Weather()
        {
            _collection.Add("today", new double[] {});
            _collection.Add("tomorrow", new double[] {});
            _collection.Add("afterTomorrow", new double[] {});
            _collection.Add("FourthDay", new double[] {});
            _web = new HtmlWeb();
            _doc = _web.Load("https://www.meteoprog.com/ru/meteograms/Mounteverest/");
            _windNode = _doc.DocumentNode.SelectNodes("//div[@class='wind-speed__column']/span");
        }

        public static Dictionary<string, double[]> CoollectionWindAndDays
        {
            get { return _collection; }
            set { _collection = value; }
        }

        //public void GetHtmlSource()
        //{
        //    _web = new HtmlWeb();
        //    _doc = _web.Load("https://www.meteoprog.com/ru/meteograms/Mounteverest/");
        //    _windNode = _doc.DocumentNode.SelectNodes("//div[@class='wind-speed__column']/span");
        //}

        public void GetWindForthDays()
        {
            int index = 0;
            double[] valueDaysToday = new double[24];
            double[] valueDaysTomorrow = new double[24];
            double[] valueDaysDayAfterTomorrow = new double[24];
            double[] valueDaysFourthDay = new double[24];

            foreach (var wind in _windNode)
            {
                if (index < 24)
                {
                    
                    valueDaysToday[index] = Double.Parse(wind.InnerText);
                }
                else if (index < 48)
                {
                    
                    valueDaysTomorrow[index - 24] = Double.Parse(wind.InnerText);
                }
                else if (index < 72)
                {
                    
                    valueDaysDayAfterTomorrow[index - 48] = Double.Parse(wind.InnerText);
                }
                else if (index < 96)
                {
                    
                    valueDaysFourthDay[index - 72] = Double.Parse(wind.InnerText);
                }
                
                index++;
            }
            _collection["today"] = valueDaysToday;
            _collection["tomorrow"] = valueDaysTomorrow;
            _collection["afterTomorrow"] = valueDaysDayAfterTomorrow;
            _collection["FourthDay"] = valueDaysFourthDay;
            CoollectionWindAndDays = _collection;
        }

        public double[]? DaysWeather(string day)
        {
            foreach (var wind in _collection)
            {
                if (_collection.ContainsKey(day))
                {
                    return _collection[day];
                }
            }
            return null;
        }

        public void UpdateSpeedsNewData()
        {

        }

    }



}
