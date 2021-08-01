using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FishSellingOnline.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the FishSellingOnlineUser class
    public class FishSellingOnlineUser : IdentityUser
    {
        //add extra information such as first name, last name, address, contact number
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        [PersonalData]
        public string Address { get; set; }

        [PersonalData]
        public int ContactNumber { get; set; }

   

    }
}
