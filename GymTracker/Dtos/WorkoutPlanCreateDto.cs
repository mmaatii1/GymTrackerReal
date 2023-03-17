using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTracker.Dtos
{
    [ApiEndpoint("WorkoutPlan")]
    public class WorkoutPlanCreateDto : BaseEntity
    {
        public IEnumerable<int> ExercisesIds { get; set; }
        public string Name { get; set; }
    }
}
