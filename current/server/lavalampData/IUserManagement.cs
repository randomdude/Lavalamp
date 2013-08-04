using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lavalampData
{
    using System.Diagnostics.Contracts;

    public interface IUserManagement
    {
        /// <summary>
        /// Creates or updates a user in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool createOrUpdateUser(IUser user);


        /// <summary>
        /// Checks if the user data corrisponds to an existing user
        /// based on the data in the user object (facial recognition, password, user id etc.)
        /// </summary>
        /// <param name="user"></param>
        /// <returns>the full user data on succes null on faliure</returns>
        [Pure]
        IUser isUser(IUser user);

        /// <summary>
        /// Returns a user
        /// </summary>
        /// <returns></returns>
        [Pure]
        IUser getUser();

        /// <summary>
        /// Removes a user from the system
        /// </summary>
        /// <param name="user">The user to delete, must have the users id present</param>
        void deleteUser(IUser user);

    }
}
