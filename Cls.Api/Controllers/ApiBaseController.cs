using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class APIBaseController : ControllerBase
{
	protected IUnitOfWork _unitOfWork;

	public APIBaseController(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}


}
