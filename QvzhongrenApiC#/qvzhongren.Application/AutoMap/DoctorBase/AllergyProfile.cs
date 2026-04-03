namespace qvzhongren.Application.AutoMapper
{
    /// <summary>
    /// 映射配置文件
    /// </summary>
    public class AllergyProfile: Profile
    {
        public AllergyProfile() { 
            CreateMap<FinIprAllergy, FinIprAllergyResponseDto>()
                .ForMember(dest => dest.PatientNo, opt => opt.MapFrom(src => src.PatientNo))
                .ForMember(dest => dest.DrugCode, opt => opt.MapFrom(src => src.DrugCode))
                .ForMember(dest => dest.DrugName, opt => opt.MapFrom(src => src.DrugName))
                .ForMember(dest => dest.SkinResult, opt => opt.MapFrom(src => src.SkinResult))
                .ReverseMap();

        }

    }
}
