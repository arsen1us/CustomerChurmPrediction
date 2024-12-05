﻿using CustomerChurmPrediction.Entities.ProductEntity;
using CustomerChurmPrediction.Entities.UserActionEntity;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("api/user-action")]
    public class UserActionController : Controller
    {
        /// <summary>
        /// Добавить действие пользователя
        /// </summary>
        // POST: api/product/{productId}
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] UserActionAdd userActionAdd)
        {
            try
            {
                return Ok();
            }
            catch
            {
                return Ok();
            }
        }
    }
}