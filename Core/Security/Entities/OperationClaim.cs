using Core.DataAccess;

namespace Core.Security.Entities
{
    public class OperationClaim : Entity
    {
        public string Name { get; set; } // Test.Add, Test.Update, Test.Delete, Test.Read, Test.Write, Admin
    }
}
