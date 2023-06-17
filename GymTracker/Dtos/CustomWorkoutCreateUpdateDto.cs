using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTracker.Dtos
{
    [ApiEndpoint("CustomWorkout")]
    public class CustomWorkoutCreateUpdateDto : BaseEntity
    {
        public List<int> SpecificExercisesIds { get; set; }
        public DateTime DateOfWorkout { get; set; }
        public string Name { get; set; }
        public Guid? Guid { get; set; }
    }
}
