using Facade.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facade.Data.MockData
{
    public static class MockUserData
    {
        public static List<User> GetUsers()
        {
            return new List<User>
        {
            new User { Name = "John Doe", Email = "john.doe@example.com", Phone = "+1-555-1234" },
            new User { Name = "Jane Smith", Email = "jane.smith@example.com", Phone = "+1-555-5678" },
            new User { Name = "Alice Johnson", Email = "alice.johnson@example.com", Phone = "+1-555-9101" }
        };
        }
    }
}
