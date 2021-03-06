﻿using Data.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Model.External
{
    public class ExternalGoldDataModel
    {
        //names comes from external api json structure
        public const string NEWEST_AVAILALE_DATE = "newest_available_date";
        public const string OLDEST_AVAILABLE_DATE = "oldest_available_date";
        public const string DATA = "data";

        [JsonProperty(NEWEST_AVAILALE_DATE)]
        public string NewestAvailaleDate { get; set; }

        [JsonProperty(OLDEST_AVAILABLE_DATE)]
        public string OldestAvailableDate { get; set; }

        [JsonProperty(DATA)]
        public List<List<object>> Data { get; set; }

        public static explicit operator GoldPrices(ExternalGoldDataModel externalGoldDataModel)
        {
            return new GoldPrices
            {
                Prices = GetDailyGoldPricesFromExternalData(externalGoldDataModel.Data)
                .Select(d => new GoldPriceDay
                {
                    Date = d.Key,
                    Price = d.Value
                }).ToList()
            };
        }

        private static Dictionary<DateTime, double> GetDailyGoldPricesFromExternalData(List<List<object>> data)
        {
            var dailyGoldPrices = new Dictionary<DateTime, double>();

            foreach (var dayData in data)
            {
                if (!DateTime.TryParse(dayData.First().ToString(), out DateTime key) ||
                    !double.TryParse(dayData.Last().ToString(), out double value)) continue;

                dailyGoldPrices.Add(key, value);
            }

            return dailyGoldPrices;
        }
    }
}
