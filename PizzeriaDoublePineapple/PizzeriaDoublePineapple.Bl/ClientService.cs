using PizzeriaDoublePineapple.Bl.Models;
using PizzeriaDoublePineapple.Data;
using PizzeriaDoublePineapple.Data.Models;
using System;

namespace PizzeriaDoublePineapple.Bl
{
    public class ClientService
    {
        private readonly ClientsRepository _clientsRepository = new ClientsRepository();

        public bool AddClient(Client newClient)
        {
            ClientData clientData = new ClientData
            {
                PhoneNumber = newClient.PhoneNumber,
                Email = newClient.Email,
                Name = newClient.Name,
                Surname = newClient.Surname,
                Address = newClient.Address,
            };

            bool success = _clientsRepository.AddClient(clientData);
            return success;
        }

        public bool CheckClient(string phoneNumber)
        {
            ClientData clientData = _clientsRepository.GetClientPhoneNumber(phoneNumber);

            if (clientData == null)
            {
                Console.WriteLine("client don't exist, You have to add new client");
                return false;
            }
            return true;
        }
    }
}
