﻿using AutoMapper;
using GymTrackerApiReal.Dtos.CustomWorkout;
using GymTrackerApiReal.Dtos.Exercise;
using GymTrackerApiReal.Dtos.Muscle;
using GymTrackerApiReal.Dtos.SpecificExercise;
using GymTrackerApiReal.Dtos.WorkoutPlan;
using GymTrackerApiReal.Models;

namespace GymTrackerApiReal.Dtos.MapperProfiles
{
    public class DtosProfiles : Profile

    {
        public DtosProfiles()
        {
            CreateMap<Models.CustomWorkout, CustomWorkoutReadDto>().ReverseMap();
            CreateMap<Models.CustomWorkout, CustomWorkoutCreateUpdateDto>().ReverseMap();

            CreateMap<Models.Exercise, ExerciseCreateUpdateDto>().ReverseMap();
            CreateMap<Models.Exercise, ExerciseReadDto>().ReverseMap();

            CreateMap<Models.Muscle, MuscleReadDto>().ReverseMap(); 
            CreateMap<Models.Muscle, MuscleUpdateCreateDto>().ReverseMap();

            CreateMap<Models.SpecificExercise, SpecificExerciseUpdateCreateDto>().ReverseMap();
            CreateMap<Models.SpecificExercise, SpecificExerciseReadDto>().ReverseMap();

            CreateMap<Models.WorkoutPlan, WorkoutPlanCreateDto>().ReverseMap();
            CreateMap<Models.CustomWorkout, int>().ConvertUsing(c => c.Id);

            CreateMap<Models.WorkoutPlan, WorkoutPlanReadDto>()
                .ForMember(dest => dest.DoneWorkoutsIds, opt => opt.MapFrom(src => src.DoneWorkouts.Select(c => c.Id).ToList()));

        }
    }
}
 