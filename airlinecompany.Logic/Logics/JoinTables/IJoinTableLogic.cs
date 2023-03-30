using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Flights.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Logic.Logics.JoinTables
{
    public interface IJoinTableLogic
    {
        public List<FlightDetails> FindFlightsByDestinationJoinTables(string startPoint, string finalPoint, DateTime date);
        //public List<Flight> CheckPlaneAvaibalityJoinTables(int planeId);
    }
}
