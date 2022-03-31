using I2M.KenticoContactRemover.Models;
using System.Collections.Generic;

namespace I2M.KenticoContactRemover.Interfaces
{
    public interface IContactService
    {
        List<Contact> GetAllContacts();
        void DeleteContacts(IEnumerable<Contact> contacts);
    }
}
