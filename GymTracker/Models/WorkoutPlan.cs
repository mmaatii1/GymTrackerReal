using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTracker.Models
{
    public class WorkoutPlan : BaseEntity
    {
        public IEnumerable<int> ExercisesIds { get; set; }
        public IEnumerable<int>? DoneWorkoutsIds { get; set; }
        public string Name { get; set; }
    }
}
