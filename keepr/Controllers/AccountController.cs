using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using keepr.Models;
using keepr.Services;

namespace keepr.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class AccountController : ControllerBase
	{
		private readonly ProfilesService _ps;
		private readonly VaultsService _vs;


		public AccountController(ProfilesService ps, VaultsService vs)
		{
			_ps = ps;
			_vs = vs;

		}

		[HttpGet]
		public async Task<ActionResult<Profile>> Get()
		{
			try
			{
				Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
				return Ok(_ps.GetOrCreateProfile(userInfo));
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpGet("/vaults")]
		public async Task<ActionResult<IEnumerable<Vault>>> GetVaults(string id)
		{
			Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
			return Ok(_vs.GetVaultsByUser(userInfo.Id));
		}
	}
}