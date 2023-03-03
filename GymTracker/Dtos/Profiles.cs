using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTracker.Dtos
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<SpecificExercise,SpecificExerciseUpdateCreateDto>().ReverseMap();
        }
    }
}
