using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsUser
    {
        public enum enMode { AddNew, Update };
        
        public enMode Mode { get; private set; } = enMode.AddNew;

        public int UserID { get; private set; }

        public int PersonID {  get; set; }

        private clsPerson _personInfo;

        public clsPerson PersonInfo 
        {
            get 
            {
                // So as not to be called every time for no reason 
                if (_personInfo == null && PersonID > 0)
                    _personInfo = clsPerson.Find(PersonID);

                return _personInfo;
            }

            private set { _personInfo = value; }
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsActive {  get; set; } = true;


        public clsUser()
        {
            UserID = -1;
            PersonID = -1;
            PersonInfo = null;
            UserName = string.Empty;
            Password = string.Empty;
            IsActive = true;

            Mode = enMode.AddNew;
        }

        public clsUser(int userID, int personID, string userName, string password, bool isActive)
        {
            UserID = userID;
            PersonID = personID;

            // Get Person Object By ID
            PersonInfo = clsPerson.Find(personID);

            UserName = userName;
            Password = password;
            IsActive = isActive;

            Mode = enMode.Update;
        }

        public static clsUser FindByUserID(int userID)
        {
            int personID = -1;
            string username = string.Empty;
            string password = string.Empty;
            bool isActive = false;

            if(clsUserData.GetUserInfoByUserID(userID, ref personID, ref username, ref password, ref isActive))
            {
                return new clsUser(userID, personID, username, password, isActive);
            }

            return null;
        }

        public static clsUser FindByPersonID(int personID)
        {
            int userID = -1;
            string username = string.Empty;
            string password = string.Empty;
            bool isActive = false;

            if(clsUserData.GetUserInfoByPersonID(personID, ref userID, ref username, ref password, ref isActive))
            {
                return new clsUser(userID, personID, username, password, isActive);
            }

            return null;
        }

        public static clsUser FindByUsernameAndPassword(string username, string password)
        {
            int userID = -1;
            int personID = -1;  
            bool isActive = false;

            if(clsUserData.GetUserInfoByUsernameAndPassword(username, password, ref userID, ref personID,  ref isActive))
            {
                return new clsUser(userID, personID, username, password, isActive);
            }

            return null;
        }

        private bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(this.PersonID, this.UserName, this.Password,
                                                 this.IsActive);

            return this.UserID > 0;
        }

        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(this.UserID, this.PersonID, this.UserName,
                                          this.Password, this.IsActive);
        }

        public bool Save()
        {
            switch (this.Mode)
            {
                case enMode.AddNew:

                    if (_AddNewUser())
                    {
                        this.Mode = enMode.Update;
                    }

                    return this.UserID > 0;

                case enMode.Update:
                    return _UpdateUser();

                default:
                    return false;
            }
        }

        public static bool DeleteUser(int userID)
        {
            return clsUserData.DeleteUser(userID);
        }

        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();   
        }

        public static bool IsUserExist(int userID)
        {
            return clsUserData.IsUserExist(userID);
        }

        public static bool IsUserExist(string username)
        {
            return clsUserData.IsUserExist(username);
        }

        public static bool DoesPersonHaveUser(int personID)
        {
            return clsUserData.DoesPersonHaveUser(personID);
        }

        public static bool ChangePassword(int userID, string newPassword)
        {
            return clsUserData.ChangePassword(userID, newPassword); 
        }

    }
}
