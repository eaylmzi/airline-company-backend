using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Flights.dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Logic.Logics.JoinTables
{
    public class JoinTableLogic : IJoinTableLogic
    {
        AirlineCompanyDBContext _context = new AirlineCompanyDBContext();
        private DbSet<Company> CompanyTable { get; set; }
        private DbSet<FlightAttendant> FlightAttendantTable { get; set; }
        private DbSet<Flight> FlightTable { get; set; }
        private DbSet<Point> PointTable { get; set; }
        private DbSet<Plane> PlaneTable { get; set; }
        private DbSet<SessionPassenger> SessionPassengerTable { get; set; }
        private DbSet<Passenger> PassengerTable { get; set; }





        public JoinTableLogic()
        {
            CompanyTable = _context.Set<Company>();
            FlightAttendantTable = _context.Set<FlightAttendant>();
            FlightTable = _context.Set<Flight>();
            PointTable = _context.Set<Point>();
            PlaneTable = _context.Set<Plane>();
            SessionPassengerTable = _context.Set<SessionPassenger>();
            PassengerTable = _context.Set<Passenger>();

        }
        public List<FlightDetails> FindFlightsByDestinationJoinTables(string startPoint,string finalPoint,DateTime date)

        {

            var result = (from pointTable in PointTable
                          join flightTable in FlightTable on pointTable.Id equals flightTable.StartPoint
                          join companyTable in CompanyTable on flightTable.CompanyId equals companyTable.Id
                          join planeTable in PlaneTable on flightTable.PlaneNumber equals planeTable.Id
                          join finalPointTable in PointTable on flightTable.FinalPoint equals finalPointTable.Id

                          where pointTable.Name.ToLower().Contains(startPoint.ToLower()) &&
                           finalPointTable.Name.ToLower().Contains(finalPoint.ToLower()) &&
                           planeTable.SeatNumber > flightTable.PassengerCount &&
                           flightTable.Date == date

                          select new FlightDetails
                          {
                              FlightId = flightTable.Id,
                              CompanyName = companyTable.Name,
                              FlightNumber = flightTable.FlightNumber,
                              Price = flightTable.Price,
                              BeginningName = pointTable.Name,
                              FinalName = finalPointTable.Name,
                              Date = flightTable.Date,

                          }).ToList(); 




            return result;


        }
        /*
        public List<Plane> PassengerSessionPassengerJoinTables(int sessionId)

        {

            var result = (from t1 in PassengerTable
                          join t2 in PickupPointTable on t1.Id equals t2.UserId
                          join t3 in SessionPassengerTable on t2.Id equals t3.PickupId
                          where t3.SessionId == sessionId

                          select new PassengerDetailsDto
                          {
                              ProfilePic = t1.ProfilePic,
                              Name = t1.Name,
                              Surname = t1.Surname,
                              PhoneNumber = t1.PhoneNumber,
                              Email = t1.Email,
                              City = t1.City,
                              PassengerAddress = t1.PassengerAddress,
                          }).ToList();




            return result;


        }
        */
        /*
        public List<Flight> CheckPlaneAvaibalityJoinTables(int planeId)

        {

            var result = (from t1 in PlaneTable
                          join t2 in FlightTable on t1.Id equals t2.PlaneNumber
                          where t1.Id == t2.PlaneNumber

                          select new Flight
                          {
                              Id = t2.Id,
                              CompanyId = t2.CompanyId,
                              PlaneNumber = t2.PlaneNumber,
                              FlightNumber = t2.FlightNumber,
                              Price = t2.Price,
                              StartPoint = t2.StartPoint,   
                              FinalPoint = t2.FinalPoint,
                              Date = t2.Date,
                              FlightAttendantId = t2.FlightAttendantId
                          }).ToList();




            return result;


        }
        */
    }
}
