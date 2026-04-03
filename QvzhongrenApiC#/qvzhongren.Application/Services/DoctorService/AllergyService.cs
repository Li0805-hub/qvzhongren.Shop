namespace qvzhongren.Application.Services
{
    public class AllergyService : CrudService<FinIprAllergy, FinIprAllergyResponseDto, FinIprAllergyResponseDto>
    {
        public AllergyService(IBaseRepository<FinIprAllergy> repository,
            IMapper mapper) : base(repository, mapper)
        {
        }

        /// <summary>
        /// 根据患者id获取患者过敏信息
        /// </summary>
        /// <param name="patientNo"></param>
        /// <returns></returns>
        [HttpPost("GetPatientAllergyList")]
        public async Task<ResultDto<List<FinIprAllergyResponseDto>>> GetPatientAllergyListAynsc([FromBody] string patientNo)
        {
            try
            {
                if (string.IsNullOrEmpty(patientNo))
                { 
                    return ResultDto<List<FinIprAllergyResponseDto>>.Error("参数不能为空");       
                }
                List<FinIprAllergy> data = await _repository.GetListAsync(a=>a.PatientNo==patientNo);
                List<FinIprAllergyResponseDto> result = _mapper.Map<List<FinIprAllergy>,List< FinIprAllergyResponseDto>>(data);
                return ResultDto<List<FinIprAllergyResponseDto>>.Success(result, "获取成功");
            }
            catch (Exception ex)
            {
                return ResultDto<List<FinIprAllergyResponseDto>>.Error($"获取失败: {ex.Message}");
            }
        }


    }
}
