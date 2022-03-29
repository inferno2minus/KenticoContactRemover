using System.Collections.Generic;

namespace I2M.KenticoContactRemover.Interfaces
{
    public interface IContactService
    {
        List<int> GetAllContacts();
        void DeleteContacts(IEnumerable<int> contactIds);
    }
}
