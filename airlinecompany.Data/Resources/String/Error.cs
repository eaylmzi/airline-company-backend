using airlinecompany.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Resources.String
{
    public static class Error
    {
        public const string AlreadyAddedPassenger = "The username is already taken";
        public const string AlreadyAddedUsername = "The username is already taken";

        public const string NotDeletedPassenger = "The passenger not deleted";
        public const string NotFoundUser = "The credentials doesn't match";
        public const string NotFoundPassenger = "The passenger is not found";
        public const string NotFoundPassengerCredential = "The username and password did not match";
        public const string NotUpdatedPassenger = "The passenger is not updated";

        public const string FullPlane = "The plane is not available, it has full capacity";
        public const string NotFoundPlane = "The plane is not found";
        public const string NotAddedPlane = "The plane is not added";
        public const string NotUpdatedPlane = "The plane is not updated";
        public const string NotDeletedPlane = "The plane is not deleted";
        public const string NotAvailablePlane = "The plane is not available, it has a flight";
        

        public const string NotFoundFlightAttendant = "The flight attendant is not found";
        public const string NotAddedFlightAttendant = "The flight attendant is not added";
        public const string NotUpdatedFlightAttendant = "The flight attendant is not updated";
        public const string NotDeletedFlightAttendant = "The flight attendant is not deleted";
        public const string NotAvailableFlightAttendant = "The flight attendant is not available, it has a flight";

        public const string NotFoundCompany = "The company is not found";
        public const string NotAddedCompany = "The company is not added";
        public const string NotUpdatedCompany = "The company is not updated";
        public const string NotDeletedCompany = "The company is not deleted";
        public const string NotAvailableCompany = "The company is not available, it has a flight";

        public const string NotFoundPoint = "The point is not found";
        public const string NotAddedPoint = "The point is not added";
        public const string NotUpdatedPoint = "The point is not updated";
        public const string NotDeletedPoint = "The point is not deleted";
        public const string NotAvailablePoint = "The point is not available, flight includes that point";

        public const string NotFoundFlight = "The flight is not found";
        public const string NotAddedFlight = "The flight is not added";
        public const string NotUpdatedFlight = "The flight is not updated";
        public const string NotDeletedFlight = "The flight is not deleted";
        public const string NotAvailableFlight = "The flight is not available, it has a flight";
        public const string NotOccuredTransaction = "The transaction failed to start";
        public const string NotReceivedMoneyCompany = "The company not received money";
        public const string NotReceivedMoneyPassenger = "The passenger not received money";

        public const string NotFoundSessionPassenger = "The session passenger is not found";
        public const string NotAddedSessionPassenger = "The session passenger is not added";
        public const string NotUpdatedSessionPassenger = "The session passenger is not updated";
        public const string NotDeletedSessionPassenger = "The session passenger is not deleted";

        public const string ForeignKeyConstraints = "Foreign keys are not in database";
        public const string NotAssignedToken = "The token is not assigned to passenger";
        public const string NotAddedPassenger = "The passenger is not added";
        public const string NotMatchedUser = "The credentials of user is not matched";

        public const string NotPaidPayment = "Passenger could not pay the airfare";
        public const string NotReceivedPaymentForCompany = "The passenger fee could not be collected by the company.Fee refunded";
        public const string NotReceivedPaymentForPassenger = "The passenger fee could not be collected by the company.Fee not refunded. Please contact the company";
        public const string NotRegisteredToFlight = "The passenger did not register to flight. Money returned";
        public const string NotEnoughMoney = "Insufficient balance";
      




    }
}
