using FocusFlow.Core.Models;

namespace FocusFlow.Tests.Factories
{
    public static class ProjectFactory
    {        
        public static Project Create(this Project self) {
            if (self.Id == Guid.Empty)
                self.Id = Guid.NewGuid();

            if (self.CreatedBy == default)
                self.CreatedAt = DateTimeOffset.UtcNow;

            if (self.UpdatedAt == default)
                self.UpdatedAt = DateTimeOffset.UtcNow;

            if (string.IsNullOrWhiteSpace(self.Name))
                self.Name = $"Project";

            if (string.IsNullOrWhiteSpace(self.Description))
                self.Name = $"Description";

            return self;
        }       
    }
}
