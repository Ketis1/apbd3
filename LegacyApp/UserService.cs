using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (IsFirstNameCorrect(firstName) || IsLastNameCorrect(lastName))
            {
                return false;
            }

            if (IsEmailCorrect(email))
            {
                return false;
            }

            var age = CalculateAgeUsingDateOfBirth(dateOfBirth);

            if (IsUnderage(age))
            {
                return false;
            }

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = CreateUser(firstName, lastName, email, dateOfBirth, client);

            if (IsClientTypeVeryImportantClient(client))
            {
                user.HasCreditLimit = false;
            }
            else if (IsClientTypeImportantClient(client))
            {
                SetImportantUserCreditLimit(user);
            }
            else
            {
                SetNormalUserCreditLimit(user);
            }

            if (IsUserCreditUnder500AndExists(user))
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }

        private static void SetNormalUserCreditLimit(User user)
        {
            user.HasCreditLimit = true;
            using (var userCreditService = new UserCreditService())
            {
                int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                user.CreditLimit = creditLimit;
            }
        }

        private static bool IsUserCreditUnder500AndExists(User user)
        {
            return user.HasCreditLimit && user.CreditLimit < 500;
        }

        private static void SetImportantUserCreditLimit(User user)
        {
            using (var userCreditService = new UserCreditService())
            {
                int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                creditLimit = creditLimit * 2;
                user.CreditLimit = creditLimit;
            }
        }

        private static bool IsClientTypeImportantClient(Client client)
        {
            return client.Type == "ImportantClient";
        }

        private static bool IsClientTypeVeryImportantClient(Client client)
        {
            return client.Type == "VeryImportantClient";
        }

        private static User CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth, Client client)
        {
            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
            return user;
        }

        private static bool IsUnderage(int age)
        {
            return age < 21;
        }

        private static int CalculateAgeUsingDateOfBirth(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            bool isBirthMonthPastCurrentMonth = now.Month < dateOfBirth.Month;
            bool isBirthDayPastCurrentDayIfSameMonth = now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day;
            if (isBirthMonthPastCurrentMonth || isBirthDayPastCurrentDayIfSameMonth) age--;
            return age;
        }

        private static bool IsEmailCorrect(string email)
        {
            return !email.Contains("@") && !email.Contains(".");
        }

        private static bool IsLastNameCorrect(string lastName)
        {
            return string.IsNullOrEmpty(lastName);
        }

        private static bool IsFirstNameCorrect(string firstName)
        {
            return string.IsNullOrEmpty(firstName);
        }
    }
}
