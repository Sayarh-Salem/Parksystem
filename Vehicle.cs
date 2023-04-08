using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeniceParking
{
    public class Vehicle
    {
        public string Id { get; set; }
        public VehicleType Type { get; set; }
        public int Floor { get; set; }
        public int ParkingSpace { get; set; }
    }

    public enum VehicleType
    {
        Autos,
        Motorräder
    }

}

