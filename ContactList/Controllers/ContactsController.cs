using ContactList.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using Swashbuckle.Swagger.Annotations;
using System.Net;

namespace ContactList.Controllers
{
    public class ContactsController : ApiController
    {
        private const string Filename = "contacts.json";
        private readonly GenericStorage storage;

        public ContactsController()
        {
            this.storage = new GenericStorage();
        }

        private async Task<IEnumerable<Contact>> GetContacts()
        {
            var contacts = await this.storage.Get(Filename);
            if (contacts != null) return contacts;

            await this.storage.Save(new Contact[]{}, Filename);
            return await this.storage.Get(Filename);
        }

        /// <summary>
        /// Creates a new contact
        /// </summary>
        /// <param name="contact">The new contact</param>
        /// <returns>The saved contact</returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.Created,
            Description = "Created",
            Type = typeof(Contact))]
        [Route("~/contacts")]
        public async Task<Contact> Post([FromBody] Contact contact)
        {
            var contacts = await this.GetContacts();
            var contactList = contacts.ToList();
            contactList.Add(contact);
            await this.storage.Save(contactList, Filename);
            return contact;
        }
    }
}