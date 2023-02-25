namespace GymTrackerApiReal.Exceptions
{
    public class EntityNotFoundException : BaseException
    {
        public EntityNotFoundException(Object entity)
            : base($"The entity of type {entity} with this identifier was not found.")
        {
        }
    }
}
