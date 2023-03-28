﻿using System;
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

        public const string NotFoundPlane = "The plane is not found";
        public const string NotAddedPlane = "The plane is not added";
        public const string NotUpdatedPlane = "The plane is not updated";
        public const string NotDeletedPlane = "The plane is not deleted";

        public const string NotFoundFlightAttendant = "The flight attendant is not found";
        public const string NotAddedFlightAttendant = "The flight attendant is not added";
        public const string NotUpdatedFlightAttendant = "The flight attendant is not updated";
        public const string NotDeletedFlightAttendant = "The flight attendant is not deleted";

        public const string NotFoundCompany = "The company is not found";
        public const string NotAddedCompany = "The company is not added";
        public const string NotUpdatedCompany = "The company is not updated";
        public const string NotDeletedCompany = "The company is not deleted";

        public const string NotAssignedToken = "The token is not assigned to passenger";
        public const string NotAddedPassenger = "The passenger is not added";
        public const string NotMatchedUser = "The credentials of user is not matched";

    }
}
