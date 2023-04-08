using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeniceParking
{
    public class ParkingGarage
    {
        public int numFloors;
        public int numSpacesPerFloor;
        public Dictionary<int, Dictionary<int, (bool occupied, string vehicleId)>> occupancy;
        public ParkingGarage UpdatedGarage { get; private set; }

        public ParkingGarage(int numFloors, int numSpacesPerFloor)
        {
            this.numFloors = numFloors;
            this.numSpacesPerFloor = numSpacesPerFloor;
            occupancy = new Dictionary<int, Dictionary<int, (bool, string)>>();

            for (int i = 1; i <= this.numFloors; i++)
            {
                occupancy.Add(i, new Dictionary<int, (bool, string)>());
                for (int j = 1; j <= this.numSpacesPerFloor; j++)
                {
                    occupancy[i].Add(j, (false, null));
                }
            }
        }

        public string TryParkVehicle(Vehicle vehicle, out bool isParked)
        {
            // Check if vehicle ID already exists in the parking garage
            if (FindVehiclePosition(vehicle.Id).Contains("befindet sich auf Ebene"))
            {
                isParked = false;
                return "Das Fahrzeug existiert bereits, kann nicht geparkt werden"; // Vehicle ID already exists, cannot park
            }

            for (int i = 1; i <= numFloors; i++)
            {
                for (int j = 1; j <= numSpacesPerFloor; j++)
                {
                    if (!occupancy[i][j].occupied)
                    {
                        occupancy[i][j] = (true, vehicle.Id);
                        vehicle.Floor = i;
                        vehicle.ParkingSpace = j;
                        isParked = true;
                        return "Fahrzeug auf Etage " + i + ", Parkplatz " + j + " geparkt.";
                    }
                }
            }
            isParked = false;
            return "Keine verfügbaren Parkplätze."; // No available parking spaces
        }

        public void RemoveVehicle(string id)
        {
            foreach (var floor in occupancy)
            {
                foreach (var space in floor.Value)
                {
                    if (space.Value.occupied && space.Value.vehicleId == id)
                    {
                        occupancy[floor.Key][space.Key] = (false, null);
                        return;
                    }
                }
            }
        }

        public string FindVehiclePosition(string id)
        {
            foreach (var floor in occupancy)
            {
                foreach (var space in floor.Value)
                {
                    if (space.Value.occupied)
                    {
                        if (space.Value.vehicleId == id)
                        {
                            return "Das Fahrzeug " + id + " befindet sich auf Ebene " + floor.Key + ", Parkplatz " + space.Key;
                        }
                    }
                }
            }
            return "Fahrzeug " + id + " nicht gefunden";
        }

        public Dictionary<int, int> GetAvailableSpacesPerFloor()
        {
            Dictionary<int, int> availableSpacesPerFloor = new Dictionary<int, int>();

            foreach (var floor in occupancy)
            {
                int emptySpacesOnFloor = 0;
                foreach (var space in floor.Value)
                {
                    if (!space.Value.occupied)
                    {
                        emptySpacesOnFloor++;
                    }
                }
                availableSpacesPerFloor.Add(floor.Key, emptySpacesOnFloor);
            }

            return availableSpacesPerFloor;
        }
        public void UpdateGarage(int newNumFloors, int newNumSpacesPerFloor)
        {
            // Add or remove floors as needed
            while (numFloors < newNumFloors)
            {
                numFloors++;
                occupancy.Add(numFloors, new Dictionary<int, (bool, string)>());
            }
            while (numFloors > newNumFloors)
            {
                occupancy.Remove(numFloors);
                numFloors--;
            }

            // Update the number of spaces per floor
            foreach (var floor in occupancy)
            {
                Dictionary<int, (bool, string)> newSpaces = new Dictionary<int, (bool, string)>();
                for (int j = 1; j <= newNumSpacesPerFloor; j++)
                {
                    // If the space already exists, copy its occupancy and registration status
                    if (j <= floor.Value.Count)
                    {
                        newSpaces.Add(j, floor.Value[j]);
                    }
                    // Otherwise, add a new empty space
                    else
                    {
                        newSpaces.Add(j, (false, null));
                    }
                }
                floor.Value.Clear();
                foreach (var space in newSpaces)
                {
                    floor.Value.Add(space.Key, space.Value);
                }
            }
            // Update instance variables
            numFloors = newNumFloors;
            numSpacesPerFloor = newNumSpacesPerFloor;
        }


    }
}
