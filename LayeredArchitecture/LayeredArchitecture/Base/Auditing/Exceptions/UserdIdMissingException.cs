using System;

namespace LayeredArchitecture.Base.Auditing
{
    /// <summary>
    /// Is thrown if the User identifier cannot be resolved for entities implementing the <see cref="IAuditedEntity"/>.
    /// </summary>
    public class MissingUserIdException : Exception
    {
    }
}
