
using Bestellservice4.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bestellservice4.Services.Services
{
    public class AllergenService : IAllergenService
    {
        public ApplicationDbContext context { get; }
        public AllergenService(ApplicationDbContext context)
        {
            this.context = context;
        }

    }
}
