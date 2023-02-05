using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchForApi.Models.Dtos;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Exceptions;
using SearchForApi.Models.Inputs;
using SearchForApi.Repositories;
using SearchForApi.Services;
using SearchForApi.Utilities;

namespace SearchForApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("plan")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class PlanController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly PlanRepository _planRepository;
        private readonly IPlanService _planService;

        public PlanController(IMapper mapper, ISceneService sceneService, PlanRepository planRepository, IPlanService planService)
        {
            _mapper = mapper;
            _planRepository = planRepository;
            _planService = planService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ListResponseDto<PlansDto>> GetPlans()
        {
            var result = await _planRepository.GetAll();
            return new ListResponseDto<PlansDto>(_mapper.Map<List<PlansDto>>(result));
        }

        [HttpPost("discount")]
        public async Task<ResponseDto<DiscountDto>> GetDiscount(DiscountModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            var result = await _planService.GetDiscount(User.Id(), model.DiscountCode);
            return new ResponseDto<DiscountDto>(_mapper.Map<DiscountDto>(result));
        }

        [HttpPost]
        public async Task<ResponseDto<RequestPlanDto>> RequestPlan(RequestPlanModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            var (result, needToPay) = await _planService.RequestPlan(User.Id(), model.PlanId, model.is3Months, model.RedirectUrl, model.DiscountCode);
            return new ResponseDto<RequestPlanDto>(_mapper.Map<RequestPlanDto>(result, opt => opt.AfterMap((src, dest) => dest.NeedToPay = needToPay)));
        }

        [AllowAnonymous]
        [HttpPost("verify/{refId}")]
        [HttpGet("verify/{refId}")]
        public async Task<ActionResult> VerifyPlanPayment(string refId)
        {
            var formData = Request.Method == "POST" ? Request.Form : null;
            var queryString = Request.Method == "GET" ? Request.Query : null;

            var (succeeded, redirectUrl) = await _planService.VerifyPlanPayment(refId, formData, queryString);

            return Redirect($"{redirectUrl}?succeeded={(succeeded ? "y" : "n")}");
        }
    }
}

