using airlinecompany.Data.Models;
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





        public JoinTableLogic()
        {
            CompanyTable = _context.Set<Company>();
            FlightAttendantTable = _context.Set<FlightAttendant>();
            FlightTable = _context.Set<Flight>();
            PointTable = _context.Set<Point>();
            PlaneTable = _context.Set<Plane>();

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
    }
}
