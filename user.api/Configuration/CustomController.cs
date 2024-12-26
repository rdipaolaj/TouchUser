﻿using Microsoft.AspNetCore.Mvc;
using user.common.Responses;

namespace user.api.Configuration;
public class CustomController : ControllerBase
{
    /// <summary>
    /// Método para devolver un Ok o BadRequest en base a la evaluación del resultado de un api response
    /// </summary>
    /// <typeparam name="T">Tipo de dato de api response</typeparam>
    /// <param name="apiResponse">Objeto ApiResponse</param>
    /// <returns>Retorna IActionResult (Ok o BadRequest)</returns>
    private protected IActionResult OkorBadRequestValidationApiResponse<T>(ApiResponse<T> apiResponse)
        => apiResponse.Success ? Ok(apiResponse) : BadRequest(apiResponse);
}
