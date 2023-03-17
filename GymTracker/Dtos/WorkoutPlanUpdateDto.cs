using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTracker.Dtos
{
    [ApiEndpoint("WorkoutPlan")]
    public class WorkoutPlanUpdateDto : BaseEntity
    {
        public IEnumerable<int> ExercisesIds { get; set; }
        public IList<int>? DoneWorkoutsIds { get; set; }
        public string Name { get; set; }
    }
}
