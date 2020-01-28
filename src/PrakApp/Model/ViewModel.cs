﻿using PrakApp.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PrakApp.Model
{
    public class ViewModel :BaseClass
    {
        static ViewModel()
        {
            //// Here you will need some Logic to load the Items from your DataBase at startup
            //// For now I will create some Test Items

            //for (int i = 1; i < 10; i++)
            //{
            //    ParkItems.Add(new ParkItem { ID = i, Name = char.ConvertFromUtf32(i + 64) });
            //    DockItems.Add(new ParkItem { ID = i, Name = char.ConvertFromUtf32(i + 64 + 15) });
            //}
            GetParkAndDockItems();
            Vehicles.Add(new Vehicle() { Number = 1, Name = "ABC", RegistrationCode = "abc-123" });
            Vehicles.Add(new Vehicle() { Number = 2, Name = "DEF", RegistrationCode = "def-456" });
            Vehicles.Add(new Vehicle() { Number = 3, Name = "GHI", RegistrationCode = "ghi-789" });

        }

        public static void GetParkAndDockItems()
        {
            ParkItems.Clear();
            DockItems.Clear();

            using var conn = new SqlConnection(Settings.Default.ConnectionString);

            conn.Open();
            string qry = "SELECT * FROM Parking;";
            var cmd = new SqlCommand(qry, conn);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var parkItem = new ParkItem(reader);

                // Place item based on Type
                switch (reader["slotType"].ToString())
                {
                    case "P":
                        ParkItems.Add(parkItem);
                        break;
                    case "D":
                        DockItems.Add(parkItem);
                        break;
                    default:
                        break;
                }
            }
        }

        // Let's make these Items static, so we can access them from everywhere
        public static ObservableCollection<ParkItem> ParkItems { get; set; } = new ObservableCollection<ParkItem>();
        public static ObservableCollection<ParkItem> DockItems { get; set; } = new ObservableCollection<ParkItem>();

        public static ObservableCollection<Vehicle> Vehicles { get; set; } = new ObservableCollection<Vehicle>();

        public static ObservableCollection<LogItem> LogItems { get; set; } = new ObservableCollection<LogItem>();
    }
}
